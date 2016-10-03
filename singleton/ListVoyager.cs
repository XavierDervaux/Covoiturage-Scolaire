using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using covoiturage.main.user;

namespace covoiturage.main.singleton
{
    class ListVoyager
    {
        private List<Voyager> listL;
        private static ListVoyager _instance = null;

        public List<Voyager> GetList
        {
            get { return listL; }
        }

        public List<Voyager> Listl
        {
            get { return listL; }
            set { listL = value; }
        }

        private ListVoyager()
        {
            listL = new List<Voyager>();
        }

        public static ListVoyager Instance()
        {
            if (_instance == null)
            {
                _instance = new ListVoyager();
            }
            return _instance;
        }

        public void Add(Voyager voy)
        {
            listL.Add(voy);
        }

        public void Remove(Voyager voy)
        {
            listL.Remove(voy);
        }

        public Voyager rechercheConducteur(Trajet trajet)
        {
            Voyager result = this.Listl.Find(
               delegate(Voyager bk)
               {
                   return (bk._TrajetId == trajet._Id && bk._Statut == 0);
               }
            );
            return result;
        }

        public List<Voyager> rchPassager(Trajet trajet)
        {
            List<Voyager> results = this.Listl.FindAll(delegate(Voyager bk)
            {
                return (bk._TrajetId == trajet._Id && bk._Statut == 1);
            });
            return results;
        }

        public Boolean recherchePassager(Trajet trajet, Utilisateur util)
        {
            Voyager result = this.Listl.Find(
               delegate(Voyager bk)
               {
                   return (bk._TrajetId == trajet._Id && bk._UserId == util._Id && bk._Statut == 1);
               }
            );
            return result != null;
        }

        public List<Voyager> MyVoyage(Utilisateur util)
        {
            List<Voyager> results = this.Listl.FindAll(delegate(Voyager voy)
            {
                return voy._UserId == util._Id;
            });
            return results;
        }
    }
}
