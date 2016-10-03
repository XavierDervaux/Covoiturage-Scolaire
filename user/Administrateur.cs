using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using covoiturage.main.singleton;
namespace covoiturage.main.user
{
    public class Administrateur : Utilisateur
    {
        public Administrateur(int id, String identifiant, String nom, String prenom)
            : base(id,identifiant, nom, prenom)
        {

        }

        public void ajouterUtilisateur(int id, String nom, String prenom, String numNational, String identifiant, String mdp, Boolean admin)
        {
            SqlRequest.AjouterUtilisateur(nom, prenom, numNational, identifiant, mdp, admin, id);
            ListUtilisateur liste = ListUtilisateur.Instance();
            Passager user = new Passager(id,identifiant, nom, prenom);
            liste.Add(user);
        }

        public void supprimerUtilisateur(Utilisateur util)
        {
            ListUtilisateur liste = ListUtilisateur.Instance();
            if (util._Id < 0)
            {
                SqlRequest.retirerStackRequest(util);
            }
            else
            {
                SqlRequest.supprimerUtilisateur(util);
            }
            liste.Remove(util);         
        }
    }
}
