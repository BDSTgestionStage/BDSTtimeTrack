using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace TimeTrack
{
    public partial class Menu : ContentPage
    {
        private string RoleLibelle;
        private int UserId;
        private DataService _dataService;
        private bool isStartButtonEnabled = true;
        public DateTime SelectedDate { get; set; } = DateTime.Today;

        public Menu(string role, int userId)
        {
            RoleLibelle = role;
            UserId = userId;
            _dataService = new DataService();
            InitializeComponent();
            SetupUIBasedOnRole();
            LoadPointageData();
            SelectedDateLabel.Text = SelectedDate.ToString("dd MMMM yyyy");
        }

        private void LoadPointageData()
        {
            RefreshPointageData();
        }

        private void SetupUIBasedOnRole()
        {
            GestBtn.IsVisible = RoleLibelle == "Administrateur";
        }

        private void RefreshPointageData()
        {
            var pointages = _dataService.GetPointagesForUser(UserId)
                .Where(p => DateTime.Parse(p.Date).Date == SelectedDate.Date)
                .OrderByDescending(p => DateTime.Parse(p.Date + " " + p.Heure))
                .ToList();

            PointageListView.ItemsSource = pointages;
        }

        private async void OnStart_Clicked(object sender, EventArgs e)
        {
            // Handle start button click
        }

        private async void OnEnd_Clicked(object sender, EventArgs e)
        {
            // Handle end button click
        }

        private async void OnGestionClicked(object sender, EventArgs e)
        {
            // Handle gestion button click
        }

        private void DecoBtn_Clicked(object sender, EventArgs e)
        {
            // Handle déconnexion button click
        }

        private void OnPreviousDayClicked(object sender, EventArgs e)
        {
            SelectedDate = SelectedDate.AddDays(-1);
            SelectedDateLabel.Text = SelectedDate.ToString("dd MMMM yyyy");
            RefreshPointageData();
        }

        private void OnNextDayClicked(object sender, EventArgs e)
        {
            SelectedDate = SelectedDate.AddDays(1);
            SelectedDateLabel.Text = SelectedDate.ToString("dd MMMM yyyy");
            RefreshPointageData();
        }


    }
}
