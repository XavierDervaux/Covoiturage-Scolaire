using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using covoiturage.main;
using covoiturage.main.user;

namespace covoiturage
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean suivant = false;

        public MainWindow()
        {
            InitializeComponent();
            reinitialiser.Visibility = Visibility.Hidden;
            try
            {
                SqlRequest.openSqlRequest();
                //Z.AjouterUtilisateur("Bianco","Andy","1010580350","andy.bianco@condorcet.be","admin");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Shutdown();           
            }
        }

        private void but_connecion_Click(object sender, RoutedEventArgs e)
        {
            PrincipalWindow windowPr;
            int Type;
            String Ident;
            String Pass;
            Boolean erreur = false;
            Utilisateur util;

            if (String.IsNullOrWhiteSpace(boxIdentification.Text)){
                erreur = true;
                MessageBox.Show("Le champ Identification est incorrect.");
            }
            if (String.IsNullOrWhiteSpace(boxMdp.Password)){
                erreur = true;
                MessageBox.Show("Le champ Mot de passe est incorrect.");
            }


            if (!erreur) { 
                Ident = boxIdentification.Text;
                Pass = boxMdp.Password;
                if (radioConducteur.IsChecked == true){ Type = 0; } else { Type = 1; }
                util = SqlRequest.connexionUtilisateur(Ident, Pass, Type);
            
                if (util != null){
                    this.suivant = true;

                    if (util is Administrateur){
                        windowPr = new PrincipalWindow( (Administrateur)util );
                        windowPr.Title = "Panneau d'administration";
                        windowPr.ongletAdmin.Visibility = Visibility.Visible;
                    } else if (util is Conducteur){
                        windowPr = new PrincipalWindow( (Conducteur)util );
                        windowPr.Title = "Conducteur";
                        windowPr.backgroundLogo.Fill = new SolidColorBrush(Color.FromRgb(0, 132, 255));
                        windowPr.backgroundLogo.Stroke = new SolidColorBrush(Color.FromRgb(0, 132, 255));
                        windowPr.bandeau_admin.Fill = new SolidColorBrush(Color.FromRgb(0, 132, 255));
                        windowPr.bandeau_admin.Stroke = new SolidColorBrush(Color.FromRgb(0, 132, 255));
                        windowPr.ongletConducteur.Visibility = Visibility.Visible;
                    } else { //Passager
                        windowPr = new PrincipalWindow( (Passager)util );
                        windowPr.Title = "Passager";
                        windowPr.logo_voiture.Visibility = Visibility.Hidden;
                        windowPr.logo_passager.Visibility = Visibility.Visible;
                        windowPr.backgroundLogo.Fill = new SolidColorBrush(Color.FromRgb(34, 176, 0));
                        windowPr.backgroundLogo.Stroke = new SolidColorBrush(Color.FromRgb(34, 176, 0));
                        windowPr.bandeau_admin.Fill = new SolidColorBrush(Color.FromRgb(34, 176, 0));
                        windowPr.bandeau_admin.Stroke = new SolidColorBrush(Color.FromRgb(34, 176, 0));
                        windowPr.ongletPassager.Visibility = Visibility.Visible;
                    }

                    windowPr.Show();
                    this.Close(); 
                }
            }
        }

        private void but_mdp_oublie_Click(object sender, RoutedEventArgs e)
        {
            boxIdentification.Clear();
            boxMdp.Clear();

            connexion.Visibility = Visibility.Hidden;
            reinitialiser.Visibility = Visibility.Visible;
        }

        private void but_retour_Click(object sender, RoutedEventArgs e)
        {
            reinitialiser.Visibility = Visibility.Hidden;
            connexion.Visibility = Visibility.Visible;

            box_email.Clear();
            box_regNat.Clear();
        }

        private void but_REINITIALISER_Click(object sender, RoutedEventArgs e)
        {
            String ident;
            String regNat;
            Boolean erreur = false;

            if (String.IsNullOrWhiteSpace(box_email.Text))
            {
                erreur = true;
                MessageBox.Show("Erreur, le champ E-mail est vide.");
            }
            if (String.IsNullOrWhiteSpace(box_regNat.Text))
            {
                erreur = true;
                MessageBox.Show("Erreur, le champ registre nationnal est vide.");
            }

            if (!erreur)
            {
                ident = box_email.Text;
                regNat = box_regNat.Text;
                Utilisateur user = SqlRequest.regNatCorrect(ident, regNat);


                if (user != null)
                {
                    SqlRequest.modifierMdp(user, regNat);
                    MessageBox.Show("Votre mot de passe à bien été réinitialisé !");
                }
                else
                {
                    MessageBox.Show("Erreur, La combinaison E-Mail/Registre nationnal est incorrecte.");
                }
            }
        }

        ~MainWindow()
        {
            try
            {
                if (SqlRequest._Statut && !this.suivant)
                    SqlRequest.closeSqlRequest();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
