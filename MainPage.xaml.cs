using System;
using System.Data;
using System.Text;
using Microsoft.Maui.Controls;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TimeTrack
{
    public partial class LoginPage : ContentPage
    {
        private string roleId;
        public LoginPage()
        {
            InitializeComponent();

        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;
            var dataservice = new DataService();
            // La fonction retourne maintenant un Tuple
            var authResult = dataservice.AuthenticateUser(username, GenerateSHA256Hash(password));

            // Utilisation de authResult.Item1 pour vérifier si l'utilisateur est authentifié
            if (authResult.Item1)
            {
                // authResult.Item2 contient maintenant le roleId
                string roleId = authResult.Item2;

                if (roleId == "Administrateur")
                {
                    await Navigation.PushAsync(new AdminPage());
                }
                else if (roleId == "Utilisateur")
                {
                    await Navigation.PushAsync(new UserPage());
                }
            }
            else
            {
                // Afficher un message d'erreur.
                await DisplayAlert("Erreur", "Nom d'utilisateur ou mot de passe incorrect.", "OK");
            }
        }



        private static string GenerateSHA256Hash(string input)
        {
            // Utilisation de SHA256.Create pour obtenir une instance de SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convertir le mot de passe en bytes
                byte[] bytes = Encoding.UTF8.GetBytes(input);

                // Hasher les bytes du mot de passe
                byte[] hashBytes = sha256Hash.ComputeHash(bytes);

                // Convertir le hash en string hexadécimal
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

                return hash;
            }
        }


    }


       
}
