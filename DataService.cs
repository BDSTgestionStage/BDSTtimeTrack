using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace TimeTrack
{
    public class DataService
    {
        private readonly SqlConnection _connection;

        public DataService()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.UserID = "sa";
            builder.Password = "Info76240#";
            builder.InitialCatalog = "MLR1";
            builder.DataSource = "localhost";
            _connection = new SqlConnection(builder.ConnectionString);
        }

        public Tuple<bool, string> AuthenticateUser(string username, string password)
        {
            try
            {
                _connection.Open();
                // Note : Il est préférable de ne sélectionner que les champs nécessaires plutôt que d'utiliser SELECT *
                string query = "SELECT UTI_Role, UTI_Prenom, UTI_MotDePasse FROM Utilisateur WHERE UTI_Prenom=@username AND UTI_MotDePasse=@password";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                // ExecuteScalar retourne la première colonne de la première ligne dans le jeu de résultats retourné par la requête
                // Dans ce cas, il s'agit de UTI_Role
                var roleIdObject = command.ExecuteScalar();

                _connection.Close();

                // Si roleIdObject n'est pas null, cela signifie que l'utilisateur a été authentifié avec succès
                if (roleIdObject != null)
                {
                    string roleId = roleIdObject.ToString();
                    // Authentification réussie, retourner true et le roleId
                    return Tuple.Create(true, roleId);
                }
                else
                {
                    // Authentification échouée, retourner false et null pour le roleId
                    return Tuple.Create(false, (string)null);
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici (par exemple, en loguant l'erreur)
                // Ne pas oublier de fermer la connexion dans un bloc finally pour s'assurer qu'elle sera fermée quelle que soit l'issue de la tentative de connexion
                return Tuple.Create(false, (string)null);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }



        public bool AddUser(int id, string nom, string prenom, string motDePasse, string auth, string roleId)
        {
            try
            {
                _connection.Open();
               
                // Préparation de la commande SQL avec des paramètres
                string query = "INSERT INTO Utilisateur (UTI_ID, UTI_Nom, UTI_Prenom, UTI_MotDePasse, UTI_Auth, UTI_Role) VALUES (@ID, @Nom, @Prenom, @MotDePasse, @Auth, @RoleId)";
                SqlCommand command = new SqlCommand(query, _connection);

                // Ajout des valeurs aux paramètres
                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@Nom", nom);
                command.Parameters.AddWithValue("@Prenom", prenom);
                command.Parameters.AddWithValue("@MotDePasse", motDePasse);
                command.Parameters.AddWithValue("@Auth", auth);
                command.Parameters.AddWithValue("@RoleId", roleId);

                // Exécution de la commande
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici (log l'erreur, etc.)
                return false;
            }
            finally
            {
                _connection.Close();
            }
        }

        public Utilisateur GetUserByAuth(string auth)
        {
            try
            {
                _connection.Open();
                string query = "SELECT UTI_ID, UTI_Nom, UTI_Prenom, UTI_Auth, UTI_Role FROM Utilisateur WHERE UTI_Auth=@Auth";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@Auth", auth);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Utilisateur user = new Utilisateur()
                        {
                            Id = Convert.ToInt32(reader["UTI_ID"]),
                            Nom = reader["UTI_Nom"].ToString(),
                            Prenom = reader["UTI_Prenom"].ToString(),
                            Username = reader["UTI_Auth"].ToString(),
                            RoleId = reader["UTI_Role"].ToString()
                        };
                        return user;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                // Handle the exception (log the error, etc.)
                return null;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }

        public bool UpdateUser(string auth, string nom, string prenom, string motDePasseHash, string roleId)
        {
            try
            {
                _connection.Open();

                string query = "UPDATE Utilisateur SET UTI_Nom = @Nom, UTI_Prenom = @Prenom, UTI_MotDePasse = @MotDePasse, UTI_Role = @RoleId WHERE UTI_Auth = @Auth";
                SqlCommand command = new SqlCommand(query, _connection);

                command.Parameters.AddWithValue("@Auth", auth);
                command.Parameters.AddWithValue("@Nom", nom);
                command.Parameters.AddWithValue("@Prenom", prenom);
                command.Parameters.AddWithValue("@MotDePasse", motDePasseHash);
                command.Parameters.AddWithValue("@RoleId", roleId);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici
                return false;
            }
            finally
            {
                _connection.Close();
            }
        }

        public bool DeleteUser(string auth)
        {
            try
            {
                _connection.Open();

                string query = "DELETE FROM Utilisateur WHERE UTI_Auth = @Auth";
                SqlCommand command = new SqlCommand(query, _connection);

                command.Parameters.AddWithValue("@Auth", auth);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Handle the exception
                return false;
            }
            finally
            {
                _connection.Close();
            }
        }



    }

}

