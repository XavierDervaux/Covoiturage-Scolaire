using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using covoiturage.main.singleton;

namespace covoiturage.main.user
{
    public class Voyager{

        protected int Statut { get; set; }
        protected int UserId { get; set; }
        protected int TrajetId { get; set; }

        public int _Statut { get {return Statut;} }// 0 Conducteur / 1 Passager
        public int _UserId { get { return UserId; } }
        public int _TrajetId { get { return TrajetId; } }


        public Voyager(int statut, int userid, int trajetid)
        {
            this.Statut = statut;
            this.UserId = userid;
            this.TrajetId = trajetid;
        }
    }
}