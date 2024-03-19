using System;
using Microsoft.Maui.Controls;

namespace TimeTrack
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            DataServiceSQL dataService = new DataServiceSQL();
            bool isValidUser = dataService.AuthenticateUser(username, password);

            if (isValidUser)
            {
                // Si l'utilisateur est authentifié, naviguez vers la page principale ou le tableau de bord.
                await Navigation.PushAsync(new DashboardPage());
            }
            else
            {
                // Afficher un message d'erreur.
                await DisplayAlert("Erreur", "Nom d'utilisateur ou mot de passe incorrect.", "OK");
            }
        }

        private Task<bool> AuthenticateUser(string username, string password)
        {
            // Logique pour authentifier l'utilisateur avec la base de données.
            // Cette méthode devrait interroger la base de données et retourner un booléen.
            throw new NotImplementedException();
        }
    }
}
