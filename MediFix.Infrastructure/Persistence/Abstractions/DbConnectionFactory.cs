using MediFix.Application.Abstractions.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MediFix.Infrastructure.Persistence.Abstractions;

internal class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MediFix")
            ?? throw new InvalidOperationException("DbConnectionFactory: ConnectionString not found.");
    }

    public IDbConnection CreateOpenConnection()
    {
        IDbConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
