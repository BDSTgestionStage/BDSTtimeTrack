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

                // Convertir le hash en string hexad�cimal
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
                // Afficher un message d'erreur si la conversion �choue
                await DisplayAlert("Erreur", "L'ID doit �tre un nombre entier.", "OK");
                return; // Sortir de la m�thode si la conversion a �chou�
            }

            if (!rolesValides.Contains(roleId))
            {
                await DisplayAlert("Erreur", "Veuillez saisir un r�le valide (Administrateur ou Utilisateur).", "OK");
                return;
            }

            if (string.IsNullOrEmpty(motDePasse))
            {
                await DisplayAlert("Erreur", "Le mot de passe ne peut pas �tre vide.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(motDePasseHash))
            {
                await DisplayAlert("Erreur", "Le hachage du mot de passe a �chou�.", "OK");
                return;
            }

            var dataservice = new DataService();
            bool userAdded = dataservice.AddUser(id, nom, prenom, motDePasseHash, auth, roleId);

            if (userAdded)
            {
                await DisplayAlert("Succ�s", "Utilisateur enregistr� avec succ�s.", "OK");
            }
            else
            {
                await DisplayAlert("Erreur", "L'enregistrement a �chou�. diya", "OK");
            }
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            var dataservice = new DataService();
            string auth = AuthSearchEntry.Text;
            if (string.IsNullOrWhiteSpace(auth))
            {
                await DisplayAlert("Erreur", "Le champ de recherche Auth ne peut pas �tre vide.", "OK");
                return;
            }

            Utilisateur user = dataservice.GetUserByAuth(auth);
            if (user != null)
            {
                await DisplayAlert("D�tails de l'utilisateur",
                    $"ID: {user.Id}\nNom: {user.Nom}\nPr�nom: {user.Prenom}\nRole: {user.RoleId}",
                    "OK");
            }
            else
            {
                await DisplayAlert("Erreur", "Aucun utilisateur trouv� avec cet Auth.", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            // Impl�mentation de la logique de suppression
            string auth = AuthSearchEntry.Text;
            if (string.IsNullOrWhiteSpace(auth))
            {
                await DisplayAlert("Erreur", "Le champ de recherche Auth pour la suppression ne peut pas �tre vide.", "OK");
                return;
            }

            var dataservice = new DataService();
            bool isDeleted = dataservice.DeleteUser(auth);
            if (isDeleted)
            {
                await DisplayAlert("Succ�s", "Utilisateur supprim� avec succ�s.", "OK");
            }
            else
            {
                await DisplayAlert("Erreur", "La suppression a �chou�.", "OK");
            }
        }

        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            // R�cup�rer l'Auth saisi
            string authSearch = AuthSearchEntry.Text;

            if (string.IsNullOrWhiteSpace(authSearch))
            {
                await DisplayAlert("Erreur", "Le champ Auth ne peut pas �tre vide.", "OK");
                return;
            }

            // R�cup�rer les donn�es de l'utilisateur depuis la base de donn�es
            var dataservice = new DataService();
            Utilisateur user = dataservice.GetUserByAuth(authSearch);

            if (user != null)
            {
                // R�cup�ration des donn�es de l'utilisateur
                string auth = AuthSearchEntry.Text;
                string nom = await DisplayPromptAsync("Modification", "Nom", initialValue: user.Nom);
                string prenom = await DisplayPromptAsync("Modification", "Pr�nom", initialValue: user.Prenom);
                string roleId = await DisplayPromptAsync("Modification", "Role", initialValue: user.RoleId);
                string motDePasse = await DisplayPromptAsync("Modification", "Mot de passe");


                string motDePasseHash = GenerateSHA256Hash(motDePasse);
                // ... d'autres champs si n�cessaire

                // Ici, vous pouvez mettre � jour les valeurs si elles ont �t� modifi�es
                if (!string.IsNullOrWhiteSpace(nom) && !string.IsNullOrWhiteSpace(prenom) && !string.IsNullOrWhiteSpace(roleId))
                {
                    bool isUpdated = dataservice.UpdateUser(auth,nom, prenom, motDePasseHash, roleId);
                    if (isUpdated)
                    {
                        await DisplayAlert("Succ�s", "Utilisateur mis � jour avec succ�s.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Erreur", "La mise � jour a �chou�.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Erreur", "Les informations ne peuvent pas �tre vides.", "OK");
                }
            }

            else
            {
                await DisplayAlert("Erreur", "Aucun utilisateur trouv� avec cet Auth.", "OK");
            }
        }


    }
}
