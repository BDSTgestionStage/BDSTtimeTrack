using System;
using Microsoft.Maui.Controls;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;


namespace TimeTrack
{
    public partial class AdminPage : ContentPage
    {
        private string roleId;
        public AdminPage()
        {
            InitializeComponent();
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

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            int id;

            string nom = NomEntry.Text;
            string prenom = PrenomEntry.Text;
            string auth = AuthEntry.Text;
            roleId = RoleEntry.Text;
            string motDePasse = PasswordEntryCreate.Text;
            string motDePasseHash = GenerateSHA256Hash(motDePasse);
            var rolesValides = new List<string> { "Administrateur", "Utilisateur" };

            if (!int.TryParse(Id.Text, out id))
            {
                // Afficher un message d'erreur si la conversion échoue
                await DisplayAlert("Erreur", "L'ID doit être un nombre entier.", "OK");
                return; // Sortir de la méthode si la conversion a échoué
            }

            if (!rolesValides.Contains(roleId))
            {
                await DisplayAlert("Erreur", "Veuillez saisir un rôle valide (Administrateur ou Utilisateur).", "OK");
                return;
            }

            if (string.IsNullOrEmpty(motDePasse))
            {
                await DisplayAlert("Erreur", "Le mot de passe ne peut pas être vide.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(motDePasseHash))
            {
                await DisplayAlert("Erreur", "Le hachage du mot de passe a échoué.", "OK");
                return;
            }

            DataService dataService = new DataService();
            bool userAdded = dataService.AddUser(id, nom, prenom, motDePasseHash, auth, roleId);

            if (userAdded)
            {
                await DisplayAlert("Succès", "Utilisateur enregistré avec succès.", "OK");
            }
            else
            {
                await DisplayAlert("Erreur", "L'enregistrement a échoué. diya", "OK");
            }
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            var dataservice = new DataService();
            string auth = AuthSearchEntry.Text;
            if (string.IsNullOrWhiteSpace(auth))
            {
                await DisplayAlert("Erreur", "Le champ de recherche Auth ne peut pas être vide.", "OK");
                return;
            }

            Utilisateur user = dataservice.GetUserByAuth(auth);
            if (user != null)
            {
                await DisplayAlert("Détails de l'utilisateur",
                    $"ID: {user.Id}\nNom: {user.Nom}\nPrénom: {user.Prenom}\nRole: {user.RoleId}",
                    "OK");
            }
            else
            {
                await DisplayAlert("Erreur", "Aucun utilisateur trouvé avec cet Auth.", "OK");
            }
        }




    }



}
