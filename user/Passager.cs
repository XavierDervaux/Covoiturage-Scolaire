using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using covoiturage.main.singleton;

namespace covoiturage.main.user
{
    public class Passager : Utilisateur
    {
        private List<Inscription> listInscription = new List<Inscription>();

        public Passager(int id, String identifiant, String nom, String prenom)
            : base(id, identifiant, nom, prenom)
        {
               
        }

        public void chargerInscription(List<Trajet> list)
        {
            int i;
            Inscription inscri;
            for (i = 0; i < list.Count(); i++)
            {
                inscri = new Inscription(this, list[i]);
                this.listInscription.Add(inscri);
            }
        }

        public void ajouterInscription(Trajet traj)
        {
            Inscription inscri = new Inscription(this, traj);
            inscri.ajouterTrajet();
            this.listInscription.Add(inscri);
        }

        public void supprimerInscription(Trajet traj)
        {
            this.listInscription.Count();
                Inscription result = this.listInscription.Find(
                delegate(Inscription bk)
                {
                    return (bk._Util.Equals(this) && bk._Traj.Equals(traj));
                }
            );
            this.listInscription.Remove(result);
            result.retirerTrajet();
        }
    }
}