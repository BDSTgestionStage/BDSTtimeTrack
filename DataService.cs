using System.Data;
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
            builder.InitialCatalog = "BDST_TimeTrack";
            builder.DataSource = "localhost";
            _connection = new SqlConnection(builder.ConnectionString);
        }

        public bool AuthenticateUser(string username, string password)
        {
            try
            {
                SqlCommand command = new SqlCommand("TimeTrack_Connexion", _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@username", "username_value");
                command.Parameters.AddWithValue("@password", "password_value");

                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    bool isValidUser = Convert.ToBoolean(reader["IsValidUser"]);
                    int userId = Convert.ToInt32(reader["UTI_ID"]);

                    if (isValidUser)
                    {
                        Console.WriteLine("Login successful! User ID: " + userId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid username or password.");
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Gérer l'exception (log l'erreur, par exemple)
                return false;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}

