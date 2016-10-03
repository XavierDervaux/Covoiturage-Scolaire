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
using covoiturage.main;
namespace covoiturage
{
	/// <summary>
	/// Logique d'interaction pour myWindow.xaml
	/// </summary>
	public partial class myWindow : Window
	{
        private Utilisateur Utilisateur { get; set; }
        private List<Trajet> ListTra = new List<Trajet>();

		public myWindow(Utilisateur util)
		{
			this.InitializeComponent();
            my_datagrid.ColumnHeaderHeight = 30;
            my_datagrid.RowHeaderWidth = 30;
            this.Utilisateur = util;

            ListTrajet trajet = ListTrajet.Instance();
            ListVoyager voyager = ListVoyager.Instance();
            List<Voyager> myVoyage  = voyager.MyVoyage(util);
           
            
            if (util is Conducteur) //Conducteur
            {
                List<Trajet> results = trajet.Listl.FindAll(delegate(Trajet tr){
                    foreach (Voyager element in myVoyage)
                    {
                        if (element._TrajetId == tr._Id && element._Statut == 0)
                        {
                            return true;
                        }
                    }
                    return false;
                });
                ListTra = results;
                my_datagrid.ItemsSource = ListTra;
            }
            else //Passager
            {
                List<Trajet> results = trajet.Listl.FindAll(delegate(Trajet tr){
                    foreach (Voyager element in myVoyage)
                    {
                        if (element._TrajetId == tr._Id && element._Statut == 1)
                        {
                            return true;
                        }

                    }
                    return false;
                });
                ((Passager)util).chargerInscription(results);
                ListTra = results;
                my_datagrid.ItemsSource = ListTra;
            }
		}

        private void resultDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                if (this.Utilisateur is Conducteur) //Conducteur
                {
                    MessageBoxResult modifier = MessageBox.Show("Voulez-vous modifier votre trajet ? ",
                       "Modifier trajet", MessageBoxButton.YesNo);
                    if (modifier == MessageBoxResult.Yes)//On modifie
                    {
                        Trajet trajet = (Trajet)my_datagrid.SelectedItem;
                        if (trajet != null)
                        {
                            ajouterTrajet ajouterTraj = new ajouterTrajet((Conducteur)this.Utilisateur,trajet);
                            ajouterTraj.ShowDialog();
                            ListTrajet ltTraj = ListTrajet.Instance();
                            ListTra.Remove(trajet);
                            if (trajet._Id < 0)
                                ltTraj.Remove(trajet);
                            ListTra.Add(ltTraj.trouveTrajet(trajet._Id));
                            my_datagrid.Items.Refresh();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer votre trajet ? ",
                       "Supprimer trajet", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)//On supprime
                        {
                            Trajet trajet = (Trajet)my_datagrid.SelectedItem;
                            if (trajet != null)
                            {
                                ListVoyager listVoy = ListVoyager.Instance();
                                Voyager condVoy = listVoy.rechercheConducteur(trajet);
                                listVoy.Remove(condVoy);
                                ListTra.Remove(trajet);
                                if (trajet._Id < 0){
                                    SqlRequest.retirerStackRequest(trajet);
                                }
                                else
                                    SqlRequest.supprimerTrajet(trajet);

                                
                                my_datagrid.Items.Refresh();
                            }
                        }
                    }
                   
                }
                else //Passager
                {
                    MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer votre participation ? ",
                        "Supprimer participation", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)//On supprime
                    {
                        Trajet trajet = (Trajet)my_datagrid.SelectedItem;
                        if (trajet != null)
                        {
                            ((Passager)this.Utilisateur).supprimerInscription(trajet);
                            ListTra.Remove(trajet);
                            my_datagrid.Items.Refresh();
                        }
                    }
                }
            }
        }
	}
}