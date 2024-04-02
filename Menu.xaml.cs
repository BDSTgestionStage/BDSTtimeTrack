using System;
using Microsoft.Identity.Client;
using Microsoft.Maui.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace TimeTrack
{
    public partial class Menu : ContentPage
    {
        private string RoleLibelle;
        private int UserId;


        public Menu(string role, int userId)
        {
            RoleLibelle = role;
            UserId = userId;
            InitializeComponent();

            SetupUIBasedOnRole();
        }

        private void SetupUIBasedOnRole()
        {
            // Vérifiez si l'utilisateur a le rôle d'administrateur
            if (RoleLibelle == "Administrateur")
            {
                // Si c'est le cas, affichez le bouton d'administration
                GestBtn.IsVisible = true;
            }
            else
            {
                // Sinon, masquez le bouton d'administration
                GestBtn.IsVisible = false;
            }
        }


        private async void OnLoginClicked(object sender, EventArgs e)
        {

        }

        private void DecoBtn_Clicked(object sender, EventArgs e)
        {

            try
            {
                RoleLibelle = null;
                UserId = -1;
                SetupUIBasedOnRole();

                Navigation.PushAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                DisplayAlert("Déconnexion échouée", "Une erreur s'est produite lors de la déconnexion. Veuillez essayer de nouveau.", "OK");
            }
        }

        private async void OnGestionClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminPage());
        }
    }
}
