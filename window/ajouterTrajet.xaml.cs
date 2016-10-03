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
	/// Logique d'interaction pour ajouterTrajet.xaml
	/// </summary>
	public partial class ajouterTrajet : Window
	{
        private String depart_adress, depart_numero, depart_ville, depart_cp;
        private String desti_adress, desti_numero, desti_ville, desti_cp;
        private int nbrPlace, nbrLitre, typeCarbu;
        private int tsDepart, tsArrivee;
        private int distance;
        private Conducteur Conduct { get; set;}
        private static int IdTrajet = -1;
        private Boolean Update = false;
        private Trajet Traj { get; set; }

		public ajouterTrajet(Conducteur conducteur)
		{
			this.InitializeComponent();
            this.Conduct = conducteur;
		}

        public ajouterTrajet(Conducteur conducteur, Trajet trajet)
		{
			this.InitializeComponent();
            this.Conduct = conducteur;
            this.Update = true;
            this.Traj = trajet;

            box_depart_adress.Text = trajet._Depart._Adress;
            box_depart_numero.Text = trajet._Depart._Numero;
            box_depart_ville.Text = trajet._Depart._Ville;
            box_depart_cp.Text = trajet._Depart._Cp;

            box_desti_adress.Text = trajet._Arrivee._Adress;
            box_desti_numero.Text = trajet._Arrivee._Numero;
            box_desti_ville.Text = trajet._Arrivee._Ville;
            box_desti_cp.Text = trajet._Arrivee._Cp;

            box_nbrPlace.Text = trajet._NbrPlaceVoiture.ToString();
            box_nbrLitre.Text = trajet._NbrLitreVoiture.ToString();
            box_carburant.Text = trajet._Carburant._Nom;

            box_km.Text = trajet._NbrKm.ToString();

            DateTime timeD = trajet.transformerTimestamp(trajet._HDepart);
            box_heure_depart.Text = timeD.Hour.ToString();
            box_minute_depart.Text = timeD.Minute.ToString();
            box_date_depart.SelectedDate = timeD.Date;

            DateTime timeA = trajet.transformerTimestamp(trajet._HArrivee);
            box_heure_arrivee.Text = timeA.Hour.ToString();
            box_minute_arrivee.Text = timeA.Minute.ToString();
            box_date_arrivee.SelectedDate = timeA.Date;

            but_suivant_info.Content = "Modifier";
            
		}

        private void but_annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void but_suivant_dep_Click(object sender, RoutedEventArgs e)
        {
            Boolean correct = true;

            if (String.IsNullOrWhiteSpace(box_depart_adress.Text)){
                MessageBox.Show("Le champ Adresse est incorrect.");
                correct = false;
            }
            if (String.IsNullOrWhiteSpace(box_depart_numero.Text)){
                MessageBox.Show("Le champ Numero est incorrect.");
                correct = false;
            }
            if (String.IsNullOrWhiteSpace(box_depart_ville.Text)){
                MessageBox.Show("Le champ Ville est incorrect.");
                correct = false;
            }
            if (String.IsNullOrWhiteSpace(box_depart_cp.Text)){
                MessageBox.Show("Le champ Code Postal est incorrect.");
                correct = false;
            }

            if (correct){
                depart_adress = box_depart_adress.Text;
                depart_numero = box_depart_numero.Text;
                depart_ville = box_depart_ville.Text;
                depart_cp = box_depart_cp.Text;

                depart.Visibility = Visibility.Hidden;
                destination.Visibility = Visibility.Visible;
            }
        }

        private void but_suivant_desti_Click(object sender, RoutedEventArgs e)
        {
            Boolean correct = true;

            if (String.IsNullOrWhiteSpace(box_desti_adress.Text)){
                MessageBox.Show("Le champ Adresse est incorrect.");
                correct = false;
            }
            if (String.IsNullOrWhiteSpace(box_desti_numero.Text))
            {
                MessageBox.Show("Le champ Numero est incorrect.");
                correct = false;
            }
            if (String.IsNullOrWhiteSpace(box_desti_ville.Text))
            {
                MessageBox.Show("Le champ Ville est incorrect.");
                correct = false;
            }
            if (String.IsNullOrWhiteSpace(box_desti_cp.Text))
            {
                MessageBox.Show("Le champ Code Postal est incorrect.");
                correct = false;
            }

            if (correct)
            {
                desti_adress = box_desti_adress.Text;
                desti_numero = box_desti_numero.Text;
                desti_ville = box_desti_ville.Text;
                desti_cp = box_desti_cp.Text;

                destination.Visibility = Visibility.Hidden;
                text_information.Visibility = Visibility.Hidden;
                voiture.Visibility = Visibility.Visible;
            }
        }

        private void but_suivant_info_Click(object sender, RoutedEventArgs e)
        {
            Boolean erreur = false;

            String places = box_nbrPlace.Text;
            String litres = box_nbrLitre.Text;
            String carburant = box_carburant.Text;
            if (String.IsNullOrWhiteSpace(places)){
                MessageBox.Show("Le champ Nombre de places est incorrect.");
                erreur = true;
            }
            if (String.IsNullOrWhiteSpace(litres)){
                MessageBox.Show("Le champ Nombre de litres est incorrect.");
                erreur = true;
            }
            
            for (int i = 0; i < places.Length && !erreur; i++){
                if (!char.IsNumber(places[i])){
                    MessageBox.Show("Le champ Nombre de places est incorrect.");
                    erreur = true;
                }
            }
            for (int i = 0; i < litres.Length && !erreur; i++){
                if (!char.IsNumber(litres[i])){
                    MessageBox.Show("Le champ Nombre de litres est incorrect.");
                    erreur = true;
                }
            }
            if (String.IsNullOrWhiteSpace(carburant))
            {
                MessageBox.Show("Le champ Carburant de litres est incorrect.");
                erreur = true;
            }

            if (erreur == false){
                nbrPlace = int.Parse(places);
                nbrLitre = int.Parse(litres);
                switch (carburant)
                {
                    case "Super 95": typeCarbu = 0; break;
                    case "Super 98": typeCarbu = 1; ; break;
                    case "Diesel": typeCarbu = 2; ; break;
                }
                voiture.Visibility = Visibility.Hidden;
                information.Visibility = Visibility.Visible;
            }
        }

        private void but_terminer_Click(object sender, RoutedEventArgs e)
        {
            DateTime depart, arrivee;
            int heure, minute;
            TimeSpan timestamp;
            Boolean erreur = false;
            String km = box_km.Text;

        /* ----------------- GESTION D'ERREUR ---------------*/
            if(box_date_depart.SelectedDate == null 
                || box_heure_depart.SelectedValue == null
                || box_minute_depart.SelectedValue == null)
            {
                erreur = true;
                MessageBox.Show("Les champs Date de départ ne représentent pas une date correcte.");
            }
            if(box_date_arrivee.SelectedDate == null 
                || box_heure_arrivee.SelectedValue == null
                || box_minute_arrivee.SelectedValue == null)
            {
                erreur = true;
                MessageBox.Show("Les champs Date d'arrivée ne représentent pas une date correcte.");
            }
            
            if (String.IsNullOrWhiteSpace(km)){
                MessageBox.Show("Le champ Distance(KM) est incorrect.");
                erreur = true;
            }

            for (int i = 0; i < km.Length && !erreur; i++)
            {
                if (!char.IsNumber(km[i]))
                {
                    MessageBox.Show("Le champ Distance(KM) est incorrect.");
                    erreur = true;
                }
            } 
            

        /* ------------- FIN GESTION D'ERREUR --------------- */
            if(!erreur){
                //Gestion départ
                heure  = int.Parse(box_heure_depart.Text);
                minute = int.Parse(box_minute_depart.Text);

                depart = (DateTime)box_date_depart.SelectedDate;
                depart = depart.AddHours(heure);
                depart = depart.AddMinutes(minute);
                
                //Gestion arrivée
                heure  = int.Parse(box_heure_arrivee.Text);
                minute = int.Parse(box_minute_arrivee.Text);

                arrivee = (DateTime)box_date_arrivee.SelectedDate;
                arrivee = arrivee.AddHours(heure);
                arrivee = arrivee.AddMinutes(minute);

                if (DateTime.Compare(DateTime.Now, depart) > 0){
                    MessageBox.Show("La date de départ est antérieure à aujourd'hui.");
                }
                else if (DateTime.Compare(depart, arrivee) > 0 || DateTime.Compare(depart, arrivee) == 0)
                {
                    MessageBox.Show("La date d'arrivée est antérieure à la date de départ.");
                } else { 
                    timestamp = (depart - new DateTime(1970, 1, 1, 0, 0, 0));
                    tsDepart = (int)timestamp.TotalSeconds;

                    timestamp = (arrivee - new DateTime(1970, 1, 1, 0, 0, 0));
                    tsArrivee = (int)timestamp.TotalSeconds;

                    distance = int.Parse(km);

                    //Ajouter BDDs
                    if (this.Update)
                    {
                        if (this.Traj._Id < 0)
                        {
                            SqlRequest.retirerStackRequest(this.Traj);
                            Adresse adresseDepart = new Adresse(depart_adress, depart_numero, depart_ville, depart_cp);
                            Adresse adresseArrivee = new Adresse(desti_adress, desti_numero, desti_ville, desti_cp);

                            Trajet trajet = new Trajet(this.Traj._Id, adresseDepart, adresseArrivee, distance,
                                nbrPlace, nbrLitre, ListCarburant.trouverCarburant(typeCarbu), tsDepart, tsArrivee);

                            this.Conduct.ajouterTrajet(trajet);
                        }
                        else
                        {
                            Adresse adresseDepart = new Adresse(depart_adress, depart_numero, depart_ville, depart_cp);
                            Adresse adresseArrivee = new Adresse(desti_adress, desti_numero, desti_ville, desti_cp);

                            Trajet Ntrajet = new Trajet(this.Traj._Id, adresseDepart, adresseArrivee, distance,
                                nbrPlace, nbrLitre, ListCarburant.trouverCarburant(typeCarbu), tsDepart, tsArrivee);

                            this.Conduct.updateTrajet(this.Traj, Ntrajet);
                        }
                    }
                    else
                    {
                        Adresse adresseDepart = new Adresse(depart_adress, depart_numero, depart_ville, depart_cp);
                        Adresse adresseArrivee = new Adresse(desti_adress, desti_numero, desti_ville, desti_cp);

                        Trajet trajet = new Trajet(IdTrajet, adresseDepart, adresseArrivee, distance,
                            nbrPlace, nbrLitre, ListCarburant.trouverCarburant(typeCarbu), tsDepart, tsArrivee);
                        IdTrajet--;

                        this.Conduct.ajouterTrajet(trajet);
                    }
                    this.Close();
                }
            }
        }

        private void but_retour_voiture_Click(object sender, RoutedEventArgs e)
        {
            voiture.Visibility = Visibility.Hidden;
            text_information.Visibility = Visibility.Visible;
            destination.Visibility = Visibility.Visible;      
        }

        private void but_retour_destination_Click(object sender, RoutedEventArgs e)
        {
            destination.Visibility = Visibility.Hidden;
            depart.Visibility = Visibility.Visible;
        }

        private void but_retour_info_Click(object sender, RoutedEventArgs e)
        {
            information.Visibility = Visibility.Hidden;
            voiture.Visibility = Visibility.Visible;   
        }
	}
}