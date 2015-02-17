using System.Configuration;
using System.Data.SqlClient;

namespace SIGCOMT.Persistence
{
    public class ConnectionFactory
    {
        public static SqlConnection Create(string connectionStringKey)
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString);
            connection.Open();
            return connection;
        }

        public static SqlConnection DefaultConnection()
        {
            var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings[PersistenceConfigurator.ConnectionStringKey].ConnectionString);
            connection.Open();
            return connection;
        }
    }
}