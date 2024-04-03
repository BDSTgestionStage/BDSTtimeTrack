using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Windows.Devices.Printers;

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
            builder.InitialCatalog = "BDST_TimeTrack";
            builder.DataSource = "localhost";
            _connection = new SqlConnection(builder.ConnectionString);
        }

        public Tuple<bool, string, int> AuthenticateUser(string username, string password)
        {
            try
            {
                _connection.Open();
                // Note : Il est préférable de ne sélectionner que les champs nécessaires plutôt que d'utiliser SELECT *
                var command = new SqlCommand("BDST_AuthenticateUser", _connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int userId = Convert.ToInt32(reader["UTI_ID"]);
                    string roleLibelle = reader["UTI_Role"].ToString();
                    return Tuple.Create(true, roleLibelle, userId);
                }
                else
                {
                    return Tuple.Create(false, (string)null, -1); // Retourner -1 si l'authentification échoue
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici (par exemple, en loguant l'erreur)
                return Tuple.Create(false, (string)null, -1);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }



        public bool AddUser(string nom, string prenom, string motDePasse, string auth, string RoleLibelle)
        {
            try
            {
                _connection.Open();

                // Préparation de la commande SQL pour appeler la procédure stockée
                SqlCommand command = new SqlCommand("CreerNouvelUtilisateur", _connection);
                command.CommandType = CommandType.StoredProcedure;

                // Ajout des paramètres à la commande
                command.Parameters.AddWithValue("@UTI_ID", -1); // L'ID sera généré automatiquement dans la procédure stockée
                command.Parameters.AddWithValue("@UTI_NOM", nom);
                command.Parameters.AddWithValue("@UTI_PRENOM", prenom);
                command.Parameters.AddWithValue("@UTI_AUTH", auth);
                command.Parameters.AddWithValue("@UTI_MOTDEPASSE", motDePasse);
                command.Parameters.AddWithValue("@UTI_ROLE", RoleLibelle); // Utilisation du rôle fourni en paramètre

                // Exécution de la commande
                int rowsAffected = command.ExecuteNonQuery();

                // Vérification du nombre de lignes affectées
                if (rowsAffected > 0)
                {
                    Console.WriteLine("La procédure stockée a été exécutée avec succès.");
                    return true; // Retourne true lorsque l'ajout de l'utilisateur est réussi
                }
                else
                {
                    Console.WriteLine("Erreur lors de l'exécution de la procédure stockée.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici (log l'erreur, etc.)
                Console.WriteLine("Erreur lors de l'ajout de l'utilisateur : " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }

        public Utilisateur GetUserByAuth(string auth)
        {
            try
            {
                _connection.Open();
                SqlCommand command = new SqlCommand("GetUserByAuth", _connection);
                command.CommandType = CommandType.StoredProcedure;
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
                // Gérer l'exception ici (par exemple, en journalisant l'erreur)
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

        public bool UpdateUser(string auth, string nom, string prenom, string motDePasseHash, string roleLibelle)
        {
            try
            {
                _connection.Open();

                // Préparation de la commande SQL pour appeler la procédure stockée
                SqlCommand command = new SqlCommand("UpdateUtilisateur", _connection);
                command.CommandType = CommandType.StoredProcedure;

                // Ajout des paramètres à la commande
                command.Parameters.AddWithValue("@Auth", auth);
                command.Parameters.AddWithValue("@Nom", nom);
                command.Parameters.AddWithValue("@Prenom", prenom);
                command.Parameters.AddWithValue("@MotDePasse", motDePasseHash);
                command.Parameters.AddWithValue("@Role", roleLibelle);

                // Exécution de la commande
                int rowsAffected = command.ExecuteNonQuery();

                // Vérification du nombre de lignes affectées
                if (rowsAffected > 0)
                {
                    Console.WriteLine("La procédure stockée a été exécutée avec succès.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Erreur lors de l'exécution de la procédure stockée.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici (log l'erreur, etc.)
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }


        public bool DeleteUser(string auth)
        {
            try
            {
                _connection.Open();

                SqlCommand command = new SqlCommand("DeleteUtilisateur", _connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Auth", auth);

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



    }

}

