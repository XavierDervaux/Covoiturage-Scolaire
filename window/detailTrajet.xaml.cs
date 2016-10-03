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

using covoiturage.main;
using covoiturage.main.user;
using covoiturage.main.singleton;

namespace covoiturage
{
	/// <summary>
	/// Logique d'interaction pour detailTrajet.xaml
	/// </summary>
	public partial class detailTrajet : Window
	{
        private Utilisateur Util { get; set; }
        private Conducteur Conduct { get; set; }
        private Trajet Traj { get; set; }

		public detailTrajet(Utilisateur util, Trajet trajet)
		{
			this.InitializeComponent();
            this.Util = util;
            this.Traj = trajet;

            if (util is Conducteur)
            {
                but_contact.Visibility = Visibility.Hidden;
                but_participe.Visibility = Visibility.Hidden;
            }
            else
            {
                ListUtilisateur listUtil = ListUtilisateur.Instance();
                ListVoyager listVoyage = ListVoyager.Instance();
                Voyager voyage = listVoyage.rechercheConducteur(trajet);
                this.Conduct = (Conducteur)listUtil.trouverUtilisateur(voyage);

                if (voyage._UserId == util._Id)
                {
                    this.Title = "Détail de mon trajet";
                    but_contact.Visibility = Visibility.Hidden;
                    but_participe.Visibility = Visibility.Hidden;
                    txt_participe.Text = "C'est votre trajet !";
                }
                else
                {
                    this.Title = "Détail du trajet de" + this.Conduct._Nom + " " + this.Conduct._Prenom;
                    if (listVoyage.recherchePassager(trajet,util) == true)
                    {
                        txt_participe.Text = "Vous participez déja à ce trajet !";
                        but_participe.Visibility = Visibility.Hidden;
                    }
                }
            }

            txt_depart_adresse.Text = trajet._Depart._Numero +", " + trajet._Depart._Adress;
            txt_depart_ville.Text = trajet._Depart._Cp + ", " + trajet._Depart._Ville;
            txt_arivee_adresse.Text = trajet._Arrivee._Numero + ", " + trajet._Arrivee._Adress;
            txt_arrivee_ville.Text = trajet._Arrivee._Cp + ", " + trajet._Arrivee._Ville;
            txt_depart_heure.Text = trajet.transformerTimestamp(trajet._HDepart).ToString();
            txt_arrivee_heure.Text = trajet.transformerTimestamp(trajet._HArrivee).ToString();
            txt_place.Text = placeDispo(trajet).ToString();
            txt_prix_actuel.Text = trajet.calculerPrix().ToString() + "€";
            txt_prix_min.Text = trajet.calculerPrixMin().ToString() + "€";
		}

        private void but_contact_Click(object sender, RoutedEventArgs e)
        {
            mail email = new mail(this.Util, this.Conduct);
            email.ShowDialog();
        }

        private void but_participe_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment participer au trajet de " +
                this.Conduct._Nom + " " + this.Conduct._Prenom + " ?", "Participer au trajet ?", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                ((Passager)this.Util).ajouterInscription(Traj);
                this.Close();
            }
        }

        private int placeDispo(Trajet trajet)
        {
            int retour = trajet.placeDispo();

            if (retour == 0)
            {
                but_participe.IsEnabled = false;
            }

            return retour;
        }
	}
}