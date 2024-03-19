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

        public bool AuthenticateUser(string username, string password)
        {
            try
            {
                _connection.Open();
                // Vous devriez utiliser des requêtes paramétrées pour éviter les injections SQL
                string query = "SELECT COUNT(1) FROM Utilisateur WHERE UTI_Prenom=@username AND UTI_MotDePasse=@password";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password); // Ici, 'password' doit être hashé de la même manière qu'il est stocké en BDD
                int result = (int)command.ExecuteScalar();
                return result > 0;
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