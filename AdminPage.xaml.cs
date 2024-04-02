using System;
using Microsoft.Maui.Controls;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace TimeTrack
{
    public partial class AdminPage : ContentPage
    {
        private string RoleLibelle;
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

            string nom = NomEntry.Text;
            string prenom = PrenomEntry.Text;
            string auth = AuthEntry.Text;
            RoleLibelle = RoleEntry.Text;
            string motDePasse = PasswordEntryCreate.Text;
            string motDePasseHash = GenerateSHA256Hash(motDePasse);
            var rolesValides = new List<string> { "Administrateur", "Utilisateur" };


            if (!rolesValides.Contains(RoleLibelle))
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

            var dataservice = new DataService();
            bool userAdded = dataservice.AddUser(nom, prenom, motDePasseHash, auth, RoleLibelle);

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

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            // Implémentation de la logique de suppression
            string auth = AuthSearchEntry.Text;
            if (string.IsNullOrWhiteSpace(auth))
            {
                await DisplayAlert("Erreur", "Le champ de recherche Auth pour la suppression ne peut pas être vide.", "OK");
                return;
            }

            var dataservice = new DataService();
            bool isDeleted = dataservice.DeleteUser(auth);
            if (isDeleted)
            {
                await DisplayAlert("Succès", "Utilisateur supprimé avec succès.", "OK");
            }
            else
            {
                await DisplayAlert("Erreur", "La suppression a échoué.", "OK");
            }
        }

        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            // Récupérer l'Auth saisi
            string authSearch = AuthSearchEntry.Text;

            if (string.IsNullOrWhiteSpace(authSearch))
            {
                await DisplayAlert("Erreur", "Le champ Auth ne peut pas être vide.", "OK");
                return;
            }

            // Récupérer les données de l'utilisateur depuis la base de données
            var dataservice = new DataService();
            Utilisateur user = dataservice.GetUserByAuth(authSearch);

            if (user != null)
            {
                // Récupération des données de l'utilisateur
                string auth = AuthSearchEntry.Text;
                string nom = await DisplayPromptAsync("Modification", "Nom", initialValue: user.Nom);
                string prenom = await DisplayPromptAsync("Modification", "Prénom", initialValue: user.Prenom);
                string RoleLibelle = await DisplayPromptAsync("Modification", "Role", initialValue: user.RoleId);
                string motDePasse = await DisplayPromptAsync("Modification", "Mot de passe");


                string motDePasseHash = GenerateSHA256Hash(motDePasse);
                // ... d'autres champs si nécessaire

                // Ici, vous pouvez mettre à jour les valeurs si elles ont été modifiées
                if (!string.IsNullOrWhiteSpace(nom) && !string.IsNullOrWhiteSpace(prenom) && !string.IsNullOrWhiteSpace(RoleLibelle))
                {
                    bool isUpdated = dataservice.UpdateUser(auth,nom, prenom, motDePasseHash, RoleLibelle);
                    if (isUpdated)
                    {
                        await DisplayAlert("Succès", "Utilisateur mis à jour avec succès.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Erreur", "La mise à jour a échoué.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Erreur", "Les informations ne peuvent pas être vides.", "OK");
                }
            }

            else
            {
                await DisplayAlert("Erreur", "Aucun utilisateur trouvé avec cet Auth.", "OK");
            }
        }


    }
}
