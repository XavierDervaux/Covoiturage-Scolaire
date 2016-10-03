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
	/// Logique d'interaction pour Window1.xaml
	/// </summary>
	public partial class PrincipalWindow : Window
	{
        private Utilisateur Utilisateur { get; set; }
        private static Boolean closeSqlRequest = true;

		public PrincipalWindow(Utilisateur util)
		{
            this.InitializeComponent();
            txt_nomUtil.Text = util._Nom + " " + util._Prenom;
            this.Utilisateur = util;

            datagrid_utilisateur.ColumnHeaderHeight = 30;
            datagrid_utilisateur.RowHeaderWidth = 30;

            if (util is Administrateur) //Coucou, je suis un administrateur
            {
                SqlRequest.chargerUtilisateur();
                ListUtilisateur utilisateur = ListUtilisateur.Instance();
                datagrid_utilisateur.ItemsSource = utilisateur.GetList;
            }
            else // Coucou, je suis un conducteur ou un passager
            {
                ListNotification listNotif = ListNotification.Instance();

                SqlRequest.chargerUtilisateur();
                SqlRequest.chargerTrajet();
                SqlRequest.chargerVoyager();
                SqlRequest.chargerNotification(util._Id);

                ListTrajet trajet = ListTrajet.Instance();
                datagrid_utilisateur.ItemsSource = trajet.Listl;
            }
		}
               
        private void but_addMembre_Click(object sender, RoutedEventArgs e)
        {
            addMembre membre = new addMembre((Administrateur)this.Utilisateur);
            membre.ShowDialog();
            datagrid_utilisateur.Items.Refresh();
        }

        private void but_offline_Click(object sender, RoutedEventArgs e)
        {
            MainWindow launcher = new MainWindow();
            launcher.Show();
            closeSqlRequest = false;
            ListNotification notif = ListNotification.Instance();
            ListTrajet trajet = ListTrajet.Instance();
            ListUtilisateur util = ListUtilisateur.Instance();
            ListVoyager voyage = ListVoyager.Instance();

            notif.Listl.Clear();
            trajet.Listl.Clear();
            util.Listl.Clear();
            voyage.Listl.Clear();
            this.Close();
        }

        private void but_parametre_Click(object sender, RoutedEventArgs e)
        {
            Profil profil = new Profil(Utilisateur); 
            profil.ShowDialog();
        }

        private void but_addTrajet_Click(object sender, RoutedEventArgs e)
        {
            ajouterTrajet trajet = new ajouterTrajet((Conducteur)this.Utilisateur);
            trajet.ShowDialog();
            datagrid_utilisateur.Items.Refresh();
        }

        private void but_voirTrajet_Click(object sender, RoutedEventArgs e)
        {
            myWindow mywidow = new myWindow(this.Utilisateur);
            mywidow.Title = "Mes trajets";
            mywidow.ShowDialog();
            datagrid_utilisateur.Items.Refresh();
        }

        private void resultDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                if (this.Utilisateur is Administrateur)
                {
                    Utilisateur util = (Utilisateur)datagrid_utilisateur.SelectedItem;
                    if (util != null)
                    {
                        MessageBoxResult result = MessageBox.Show("Voulez-vous supprimer l'utilisateur " + util._Identifiant, "Supprimer Utilisateur", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)//On supprime
                        {
                            ((Administrateur)this.Utilisateur).supprimerUtilisateur(util);
                            datagrid_utilisateur.Items.Refresh();
                        }
                    }
                }
                else
                {
                    Trajet trajet = (Trajet)datagrid_utilisateur.SelectedItem;
                    detailTrajet detailTra = new detailTrajet(this.Utilisateur,trajet);
                    detailTra.ShowDialog();
                }
            }
        }

        private void but_participation_Click(object sender, RoutedEventArgs e)
        {
            myWindow mywidow = new myWindow(this.Utilisateur);
            mywidow.Title = "Mes participations";
            mywidow.ShowDialog();
        }

        ~PrincipalWindow()
        {
            SqlRequest.viderStackRequest();
            if (SqlRequest._Statut && closeSqlRequest)
                SqlRequest.closeSqlRequest();
        }

        private void but_notification_Click(object sender, RoutedEventArgs e)
        {
            if (notification.Visibility == Visibility.Visible)
                notification.Visibility = Visibility.Hidden;
            else
                notification.Visibility = Visibility.Visible;

            ListNotification notif = ListNotification.Instance();
            datagrid_notification.ItemsSource = notif.Listl;
        }

        private void Notification_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null){
                Notification notif = (Notification)datagrid_notification.SelectedItem;
                if (notif != null){
                    SqlRequest.supprimerNotif(notif);
                    datagrid_notification.Items.Refresh();
                }
            }
        }
	}
}