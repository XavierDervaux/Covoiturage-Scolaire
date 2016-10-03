using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using covoiturage.main.user;

namespace covoiturage.main.singleton
{
    class ListNotification
    {
       private List<Notification> listL;
       private static ListNotification _instance = null;

       public List<Notification> Listl
        {
            get { return listL; }
            set { listL = value; }
        }

        private ListNotification()
        {
            listL = new List<Notification>();
        }

        public static ListNotification Instance()
        {
            if (_instance == null)
            {
                _instance = new ListNotification();
            }
            return _instance;
        }

        public void Add(Notification util)
        {
            listL.Add(util);
        }

        public void Remove(Notification util)
        {
            listL.Remove(util);
        }     
    }
}