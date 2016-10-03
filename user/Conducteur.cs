using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using covoiturage.main.singleton;

namespace covoiturage.main.user
{
    public class Conducteur : Utilisateur
    {
        public Conducteur(int id, String identifiant, String nom, String prenom)
            : base(id, identifiant, nom, prenom)
        {
        }

        public void ajouterTrajet (Trajet trajet)
        {
            ListTrajet list = ListTrajet.Instance();
            ListVoyager lVoyage = ListVoyager.Instance();
            
            Voyager voyage = new Voyager(0,this._Id,trajet._Id);

            list.Add(trajet);
            lVoyage.Add(voyage);
           
            SqlRequest.ajouterTrajet(trajet, this);
           
        }

        public void updateTrajet(Trajet ancT, Trajet newT)
        {
            ListTrajet list = ListTrajet.Instance();
            list.Remove(ancT);
            list.Add(newT);
            SqlRequest.updateTrajet(newT);
        }

        public void supprimerTrajet(Trajet trajet)
        {
            ListTrajet list = ListTrajet.Instance();
            list.Remove(trajet);

            ListVoyager lVoyage = ListVoyager.Instance();

            Voyager result = lVoyage.Listl.Find(
                   delegate(Voyager bk)
                   {
                       return (bk._TrajetId == trajet._Id && bk._Statut == 0 && bk._UserId == this._Id);
                   }
             );
            if(trajet._Id < 0 )
            {
                SqlRequest.retirerStackRequest(trajet);
                //Z.retirerStackRequest(result,0);
            }
            else
            {
                SqlRequest.supprimerTrajet(trajet);
                SqlRequest.supprimerVoyager(result);
            }
        }
    }
}