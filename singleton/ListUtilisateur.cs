using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using covoiturage.main.user;

namespace covoiturage.main.singleton
{
    class ListUtilisateur
    {
        private List<Utilisateur> listL;
        private static ListUtilisateur _instance = null;

        public List<Utilisateur> GetList
        {
            get{ return listL;  }
        }

      

        public List<Utilisateur> Listl
        {
            get { return listL; }
            set { listL = value; }
        }

        private ListUtilisateur()
        {
            listL = new List<Utilisateur>();
        }

        public static ListUtilisateur Instance()
        {
            if (_instance == null)
            {
                _instance = new ListUtilisateur();
            }
            return _instance;
        }

        public void Add(Utilisateur util)
        {
            listL.Add(util);
        }

        public void Remove(Utilisateur util)
        {
            listL.Remove(util);
        }

        public Utilisateur trouverUtilisateur(Voyager voyage)
        {
            Utilisateur result = this.Listl.Find(
               delegate(Utilisateur bk)
               {
                   return bk._Id == voyage._UserId;
               }
            );
            return result;
        }
    }
}
