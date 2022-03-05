using Npgsql;
using System.Data;
using System.Data.SqlClient;

namespace pet_service_v1.Database.Interfaces
{
    public interface IConnectionFactory
    {
        SqlConnection CreateDBConnection();
    }
}
