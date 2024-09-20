using Dapper;
using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Dashboard;

internal sealed class GetDashboardRequestHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetDashboardRequest, DashboardResponse>
{
    public async Task<Result<DashboardResponse>> Handle(GetDashboardRequest request, CancellationToken cancellationToken)
    {
        var query = GetQuery(request);
        var queryParameters = request.IsValid
            ? new { request.From, request.To }
            : null;

        using var dbConnection = dbConnectionFactory.CreateOpenConnection();

        SqlMapper.GridReader gridReader = await dbConnection.QueryMultipleAsync(query, queryParameters);

        return GetResponse(gridReader);
    }

    private static string GetQuery(GetDashboardRequest request)
    {
        string filter = request.IsValid
            ? "WHERE sc.DateCreated BETWEEN @From AND @To"
            : string.Empty;

        return $"""
                DROP TABLE IF EXISTS #serviceCalls
                CREATE TABLE #serviceCalls
                (
                	Id				uniqueidentifier	NOT NULL,
                	LocationId		uniqueidentifier	NOT NULL,
                	ServiceCallType tinyint			NOT NULL,
                	SubCategoryId	uniqueidentifier	NOT NULL,
                	Priority		tinyint				NOT NULL,
                	DateCreated		datetime2(7)		NOT NULL,
                	Status			tinyint				NOT NULL,
                	PractitionerId	uniqueidentifier	NULL
                )

                INSERT INTO #serviceCalls
                (Id, LocationId, ServiceCallType, SubCategoryId, Priority, DateCreated, Status, PractitionerId)
                SELECT	sc.Id, sc.LocationId, sc.ServiceCallType, sc.SubCategoryId, sc.Priority, sc.DateCreated, sc.Status, sc.PractitionerId
                FROM	dbo.ServiceCalls AS sc
                {filter}


                SELECT	v.Status, Count( sc.Id ) AS Count
                FROM(VALUES(1), (2), (3), (4), (5)) AS v(Status)
                LEFT OUTER JOIN #serviceCalls AS sc
                ON v.Status = sc.Status
                GROUP BY v.Status

                SELECT	v.Priority, Count( sc.Id ) AS Count
                FROM(VALUES(1), (2), (3), (4)) AS v(Priority)
                LEFT OUTER JOIN #serviceCalls AS sc
                ON v.Priority = sc.Priority
                GROUP BY v.Priority

                SELECT	v.ServiceCallType AS Type, Count( sc.Id ) AS Count
                FROM(VALUES(1), (2)) AS v(ServiceCallType)
                LEFT OUTER JOIN #serviceCalls AS sc
                ON v.ServiceCallType = sc.ServiceCallType
                GROUP BY v.ServiceCallType

                SELECT	c.Name AS CategoryName, Count( sc.Id ) AS Count
                FROM	dbo.Categories AS c
                LEFT OUTER JOIN dbo.SubCategories AS sub
                ON c.Id = sub.CategoryId
                LEFT OUTER JOIN #serviceCalls AS sc
                ON sc.SubCategoryId = sub.Id
                GROUP BY c.Name


                SELECT	p.Id AS PractitionerId,
                		anu.FirstName,
                		anu.LastName,
                		(
                			SELECT	Count( 'benny' )
                			FROM	#serviceCalls AS sc
                			WHERE	sc.PractitionerId = p.Id
                			AND		sc.Status = 2
                		) AS Assigned,
                		(
                			SELECT	Count( 'benny' )
                			FROM	#serviceCalls AS sc
                			WHERE	sc.PractitionerId = p.Id
                			AND		sc.Status = 3
                		) AS Started,
                		(
                			SELECT	Count( 'benny' )
                			FROM	#serviceCalls AS sc
                			WHERE	sc.PractitionerId = p.Id
                			AND		sc.Status = 4
                		) AS Finished,
                		(
                			SELECT	Avg( DateDiff( MINUTE, assign_time.DateTime, IsNull( finish_time.DateTime, GetDate())))
                			FROM	dbo.Practitioners AS prac
                			INNER JOIN #serviceCalls AS sc
                			ON prac.Id = sc.PractitionerId
                			INNER JOIN dbo.ServiceCallStatusUpdate AS assign_time
                			ON	sc.Id = assign_time.ServiceCallId
                			AND assign_time.PractitionerId = prac.Id
                			AND assign_time.Status = 2
                			LEFT OUTER JOIN dbo.ServiceCallStatusUpdate AS finish_time
                			ON	sc.Id = finish_time.ServiceCallId
                			AND finish_time.PractitionerId = prac.Id
                			AND finish_time.Status = 4
                			WHERE prac.Id = p.Id
                		) AS AvgDurationMinutes
                FROM	dbo.Practitioners AS p
                INNER JOIN dbo.AspNetUsers AS anu
                ON p.Id = anu.Id
                GROUP BY p.Id, anu.FirstName, anu.LastName


                SELECT	bldng.Name AS BuildingName,
                		Count( sc.Id ) AS Total,
                		Sum( Iif(sc.Status = 1, 1, 0)) AS [Open],
                		Sum( Iif(sc.Status = 2, 1, 0)) AS Assigned,
                		Sum( Iif(sc.Status = 3, 1, 0)) AS Started,
                		Sum( Iif(sc.Status = 4, 1, 0)) AS Finished,
                		Sum( Iif(sc.Status = 5, 1, 0)) AS Cancelled,
                		Avg( DateDiff( MINUTE, sc.DateCreated, finish_time.DateTime )) AS AvgDurationMinutes
                FROM	dbo.Locations AS bldng
                INNER JOIN dbo.Locations AS flr
                ON bldng.Id = flr.ParentId
                INNER JOIN dbo.Locations AS dep
                ON flr.Id = dep.ParentId
                INNER JOIN dbo.Locations AS room
                ON dep.Id = room.ParentId
                LEFT OUTER JOIN #serviceCalls AS sc
                ON sc.LocationId = room.Id
                LEFT OUTER JOIN dbo.ServiceCallStatusUpdate AS finish_time
                ON	sc.Id = finish_time.ServiceCallId
                AND finish_time.Status = 4
                GROUP BY bldng.Name
                ORDER BY BuildingName
                """;
    }

    private static DashboardResponse GetResponse(SqlMapper.GridReader gridReader)
    {
        var countByStatus = gridReader.Read<ServiceCallsCountByStatus>();
        var countByPriorities = gridReader.Read<ServiceCallsCountByPriority>();
        var countByTypes = gridReader.Read<ServiceCallsCountByType>();
        var countByCategories = gridReader.Read<ServiceCallsCountByCategory>();
        var practitioners = gridReader.Read<Practitioner>();
        var buildings = gridReader.Read<Building>();

        var response = new DashboardResponse(
            DashboardCountBoxes.FromServiceCallsCountByStatus(countByStatus),
            practitioners.Select(DashboardPractitioner.FromPractitioner),
            countByPriorities.Select(x => new DashboardNameValue(x.Priority.ToString(), x.Count)),
            countByCategories.Select(x => new DashboardNameValue(x.CategoryName, x.Count)),
            countByTypes.Select(x => new DashboardNameValue(x.Type.ToString(), x.Count)),
            buildings.Select(DashboardBuilding.FromBuilding)
        );

        return response;
    }
}