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
	/// Logique d'interaction pour profil.xaml
	/// </summary>
    public partial class Profil : Window
	{
        Utilisateur util;
		public Profil(Utilisateur Util)
		{
			this.InitializeComponent();
            this.util = Util;
		}

        private void but_annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void but_ajouter_Click(object sender, RoutedEventArgs e)
        {
            String mdpAct;
            String newMdp;
            String newMdpConf;
            Boolean erreur = false;
            
            if (String.IsNullOrWhiteSpace(box_passActu.Password)){
                erreur = true;
                MessageBox.Show("Erreur, le champ Mot de passe actuel est éronné.");
            } 
            if (String.IsNullOrWhiteSpace(box_newPass.Password)){
                erreur = true;
                MessageBox.Show("Erreur, le champ Nouveau mot de passe est éronné.");
            } 
            if (String.IsNullOrWhiteSpace(box_newPassConf.Password)){
                erreur = true;
                MessageBox.Show("Erreur, le champ Répéter mot de passe est éronné.");
            }

            if (!erreur){
                mdpAct = box_passActu.Password;
                newMdp = box_newPass.Password;
                newMdpConf = box_newPassConf.Password;
                
                if (newMdp == newMdpConf)
                {
                    util.modifierMdp(mdpAct, newMdp);
                    MessageBox.Show("Votre mot de passe a été modifié avec succès !");
                    this.Close();
                }
            }
        }
	}
}