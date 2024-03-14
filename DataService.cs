using System.Data.SqlClient;

namespace TimeTrack
{
    public class DataServiceSQL
    {
        private readonly SqlConnection _connection;

        public DataServiceSQL()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.UserID = "sa";
            builder.Password = "Info76240#";
            builder.InitialCatalog = "MLR1";
            builder.DataSource = "localhost";
            _connection = new SqlConnection(builder.ConnectionString);
        }
    }
}