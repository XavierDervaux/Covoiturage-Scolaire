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
using System.Net;
using System.Net.Mail;

using covoiturage.main.user;
using covoiturage.main.singleton;

namespace covoiturage
{
	/// <summary>
	/// Logique d'interaction pour mail.xaml
	/// </summary>
	public partial class mail : Window
	{
        private Utilisateur destinataire { get; set; }
        private Utilisateur envoyeur { get; set; }


		public mail(Utilisateur envoy, Utilisateur desti)
		{
			this.InitializeComponent();
            this.destinataire = desti;
            this.envoyeur = envoy;

            box_sujet.Text = envoyeur._Prenom + " " + envoyeur._Nom + " vous a envoyé un message via CondoCars !";
		}

        private void but_annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void but_envoyer_Click(object sender, RoutedEventArgs e)
        {
            String des = destinataire._Identifiant;
            Boolean erreur = false;

            MailMessage mm = new MailMessage("testenvoidemail179@gmail.com", des);
            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.UseDefaultCredentials = false;
            sc.Credentials = new NetworkCredential("testenvoidemail179@gmail.com", "jetestlesmails", "");
            sc.EnableSsl = true;
            sc.DeliveryMethod = SmtpDeliveryMethod.Network;

            mm.Subject = box_sujet.Text;

            if (String.IsNullOrWhiteSpace(box_message.Text)){
                erreur = true;
                MessageBox.Show("Le champ message est vide.");
            }

            if (erreur == false){
                mm.Body = box_message.Text;

                try{
                    sc.Send(mm);
                    MessageBox.Show("Le message a été envoyé !");
                    this.Close();
                } catch (Exception ex) {
                    MessageBox.Show("Erreur: " + ex.Message);
                }
            }
        }
	}
}