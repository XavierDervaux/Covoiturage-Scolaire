using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace covoiturage.main.user
{
    public abstract class Utilisateur
    {

        protected String Identifiant { get; set; }
        protected String Nom { get; set; }
        protected String Prenom { get; set; }
        protected int Id { get; set; }

        public String _Nom { get {return Nom;} }
        public String _Prenom { get { return Prenom; } }
        public String _Identifiant { get { return Identifiant; } }
        public int _Id { get { return Id; } }

        public Utilisateur(int id, String identifiant, String nom, String prenom)
        {
            this.Identifiant = identifiant;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Id = id;
        }

        public int modifierMdp(String ancien, String nouveau)
        {
            int retour = 0; //0 ok    1 ancien pas ok

            if (SqlRequest.mdpCorrect(Identifiant, ancien)){
                SqlRequest.modifierMdp(this, nouveau);
            } else {
                MessageBox.Show("Erreur, le mot de passe actuel est erroné.");
                retour = 1;
            }

            return retour;
        }
    }
}