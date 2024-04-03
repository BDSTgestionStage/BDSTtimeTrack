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
        private DataService _dataService;
        private bool isStartButtonEnabled = true;

        public Menu(string role, int userId)
        {
            RoleLibelle = role;
            UserId = userId;
            _dataService = new DataService();
            InitializeComponent();  
            SetupUIBasedOnRole();
            LoadPointageData();
        }

        private void LoadPointageData()
        {
            // Récupérer les données de pointage de l'utilisateur connecté à partir du service de données
            DataService dataService = new DataService();
            List<Pointage> pointages = dataService.GetPointagesForUser(UserId);

            // Associer les données à la ListView
            PointageListView.ItemsSource = pointages;
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
        private void RefreshPointageData()
        {
            // Rechargez les données d'historique en utilisant votre service de données
            List<Pointage> pointages = _dataService.GetPointagesForUser(UserId);

            // Mettez à jour la source de données de votre vue
            PointageListView.ItemsSource = pointages;
        }

        private async void OnStart_Clicked(object sender, EventArgs e)
        {
            // Enregistrer l'heure de début dans la base de données
            DateTime heureDebut = DateTime.Now;
            bool pointageAdded = _dataService.AddPointage(UserId, heureDebut);

            if (pointageAdded)
            {
                EntreeBtn.IsEnabled = false;
                SortieBtn.IsEnabled = true;
                isStartButtonEnabled = false;
                // Afficher un message de succès si le pointage est ajouté avec succès
                await DisplayAlert("Succès", "Pointage de début enregistré avec succès.", "OK");

                // Rafraîchir les données d'historique
                RefreshPointageData();
            }
            else
            {
                // Afficher un message d'erreur si le pointage n'a pas pu être ajouté
                await DisplayAlert("Erreur", "Impossible d'enregistrer le pointage de début.", "OK");
            }
        }

        private async void OnEnd_Clicked(object sender, EventArgs e)
        {
            // Enregistrer l'heure de fin dans la base de données
            DateTime heureFin = DateTime.Now;
            bool pointageAdded = _dataService.AddPointage(UserId, heureFin);

            if (pointageAdded)
            {
                SortieBtn.IsEnabled = false;
                EntreeBtn.IsEnabled = true;
                isStartButtonEnabled = true;
                // Afficher un message de succès si le pointage est ajouté avec succès
                await DisplayAlert("Succès", "Pointage de fin enregistré avec succès.", "OK");

                // Rafraîchir les données d'historique
                RefreshPointageData();
            }
            else
            {
                // Afficher un message d'erreur si le pointage n'a pas pu être ajouté
                await DisplayAlert("Erreur", "Impossible d'enregistrer le pointage de fin.", "OK");
            }
        }


        private async void OnGestionClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminPage());
        }
    }
}
