using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using covoiturage.main.user;
using covoiturage.main.singleton;

namespace covoiturage
{
	/// <summary>
	/// Logique d'interaction pour addMembre.xaml
	/// </summary>
    
	public partial class addMembre : Window
	{
        private Administrateur admin { get; set; }
        private static int IdMembre = -1;

        public addMembre(Administrateur admin)
        {
            this.InitializeComponent();
            this.admin = admin;
        }

        private void but_annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void but_ajouter_Click(object sender, RoutedEventArgs e)
        {
            
            String nom, prenom, numNational, identifiant, mdp;
            Boolean admin = checkAdmin.IsChecked == true;
            Boolean correct = true;

            if (String.IsNullOrWhiteSpace(box_nom.Text)){
                MessageBox.Show("Le champ Nom est incorrect.");
                correct = false;
            }
            if (String.IsNullOrWhiteSpace(box_prenom.Text)){
                MessageBox.Show("Le champ Prenom est incorrect.");
                correct = false;
            }
            if (String.IsNullOrWhiteSpace(box_numNat.Text)){
                MessageBox.Show("Le champ Numero Nationnal est incorrect.");
                correct = false;
            }
            if (String.IsNullOrWhiteSpace(box_identifiant.Text)){
                MessageBox.Show("Le champ Identifiant est incorrect.");
                correct = false;
            }
            if (String.IsNullOrWhiteSpace(box_password.Password)){
                MessageBox.Show("Le champ Mot de Passe est incorrect.");
                correct = false;
            }

            if (correct){
                nom = box_nom.Text;
                prenom = box_prenom.Text;
                numNational = box_numNat.Text;
                identifiant = box_identifiant.Text;
                mdp = box_password.Password;

                this.admin.ajouterUtilisateur(IdMembre,nom, prenom, numNational, identifiant, mdp, admin);
                IdMembre--;
                this.Close();
            }
        }
	}
}