using Microsoft.Maui.Controls;

namespace TimeTrack
{
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Logique pour déconnecter l'utilisateur
            await Navigation.PopToRootAsync();
        }
    }
}
