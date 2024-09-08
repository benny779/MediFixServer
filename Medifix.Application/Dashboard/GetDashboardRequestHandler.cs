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
                		Count( svc_assign.Id ) AS Assigned,
                		Count( svc_start.Id ) AS Started,
                		Count( svc_finish.Id ) AS Finished,
                		Avg( dur.Duration ) AS AvgDurationMinutes
                FROM	dbo.Practitioners AS p
                INNER JOIN dbo.AspNetUsers AS anu
                ON p.Id = anu.Id
                LEFT OUTER JOIN #serviceCalls AS svc_assign
                ON	p.Id = svc_assign.PractitionerId
                AND svc_assign.Status = 2
                LEFT OUTER JOIN #serviceCalls AS svc_start
                ON	p.Id = svc_start.PractitionerId
                AND svc_assign.Status = 3
                LEFT OUTER JOIN #serviceCalls AS svc_finish
                ON	p.Id = svc_finish.PractitionerId
                AND svc_finish.Status = 4
                LEFT OUTER JOIN
                (
                	SELECT	p.Id, DateDiff( MINUTE, sc.DateCreated, IsNull( scsu.DateTime, GetDate())) AS Duration
                	FROM	dbo.Practitioners AS p
                	INNER JOIN #serviceCalls AS sc
                	ON p.Id = sc.PractitionerId
                	LEFT OUTER JOIN dbo.ServiceCallStatusUpdate AS scsu
                	ON	sc.Id = scsu.ServiceCallId
                	AND scsu.Status = 4
                ) AS dur
                ON p.Id = dur.Id
                GROUP BY p.Id, anu.FirstName, anu.LastName


                SELECT	bldng.Name AS BuildingName,
                		Count( sc.Id ) AS Total,
                		Sum( Iif(sc.Status = 1, 1, 0)) AS [Open],
                		Sum( Iif(sc.Status = 2, 1, 0)) AS Assigned,
                		Sum( Iif(sc.Status = 3, 1, 0)) AS Started,
                		Sum( Iif(sc.Status = 4, 1, 0)) AS Finished,
                		Sum( Iif(sc.Status = 5, 1, 0)) AS Cancelled,
                		Avg( dur.Duration ) AS AvgDurationMinutes
                FROM	dbo.Locations AS bldng
                INNER JOIN dbo.Locations AS flr
                ON bldng.Id = flr.ParentId
                INNER JOIN dbo.Locations AS dep
                ON flr.Id = dep.ParentId
                INNER JOIN dbo.Locations AS room
                ON dep.Id = room.ParentId
                LEFT OUTER JOIN #serviceCalls AS sc
                ON sc.LocationId = room.Id
                LEFT OUTER JOIN
                (
                	SELECT	sc.LocationId, DateDiff( MINUTE, sc.DateCreated, IsNull( scsu.DateTime, GetDate())) AS Duration
                	FROM	dbo.Practitioners AS p
                	INNER JOIN #serviceCalls AS sc
                	ON p.Id = sc.PractitionerId
                	LEFT OUTER JOIN dbo.ServiceCallStatusUpdate AS scsu
                	ON	sc.Id = scsu.ServiceCallId
                	AND scsu.Status = 4
                ) AS dur
                ON room.Id = dur.LocationId
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