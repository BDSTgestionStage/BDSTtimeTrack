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
        private string RoleLibelle;
        public LoginPage()
        {
            InitializeComponent();

        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;
            var dataservice = new DataService();
            var authResult = dataservice.AuthenticateUser(username, GenerateSHA256Hash(password));

            if (authResult.Item1)
            {
                string roleLibelle = authResult.Item2;
                int userId = authResult.Item3; // Récupération de l'ID de l'utilisateur

                await Navigation.PushAsync(new Menu(roleLibelle, userId)); // Passez l'ID de l'utilisateur au constructeur du Menu
            }
            else
            {
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
