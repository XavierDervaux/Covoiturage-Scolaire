using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using covoiturage.main.user;
using covoiturage.main.singleton;

namespace covoiturage.main
{
    public class Inscription
    {
        private Voyager Voyage { get; set; }
        private Utilisateur Util { get; set; }
        private Trajet Traj { get; set; }

        public Utilisateur _Util { get { return Util; } }
        public Trajet _Traj { get { return Traj; } }

        public Inscription(Utilisateur util, Trajet trajet)
        {
            this.Util = util;
            this.Traj = trajet;
            this.Voyage = new Voyager(1,util._Id,trajet._Id);
        }

        public void ajouterTrajet()
        {
            ListVoyager list = ListVoyager.Instance();
            SqlRequest.ajouterVoyage(this.Voyage);
            list.Add(this.Voyage);
        }

        public void retirerTrajet()
        {
            ListVoyager list = ListVoyager.Instance();
            list.Remove(this.Voyage);
            SqlRequest.retirerStackRequest(this.Voyage, 1);
        }
    }
}