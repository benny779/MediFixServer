using System.Data;

namespace MediFix.Application.Abstractions.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateOpenConnection();
}
