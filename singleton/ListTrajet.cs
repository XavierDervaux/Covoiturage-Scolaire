
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using covoiturage.main;

namespace covoiturage.main.singleton 
{
    class ListTrajet
    {
        private List<Trajet> listL;
        private static ListTrajet _instance = null;

        public List<Trajet> Listl
        {
            get { return listL; }
            set { listL = value; }
        }

        private ListTrajet()
        {
            listL = new List<Trajet>();
        }

        public static ListTrajet Instance()
        {
            if (_instance == null)
            {
                _instance = new ListTrajet();
            }
            return _instance;
        }

        public void Add(Trajet util)
        {
            listL.Add(util);
        }

        public void Remove(Trajet util)
        {
            listL.Remove(util);
        }

        public Trajet trouveTrajet(int id)
        {
            Trajet result = this.Listl.Find(
               delegate(Trajet bk)
               {
                   return (bk._Id == id);
               }
            );
            return result;
        }
    }
}
