using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using covoiturage.main.singleton;

namespace covoiturage.main
{
    class Notification
    {

        public int Id;
        public int Id_user;
        public String Nom;
        public String Text;

        public int _Id { get { return Id; } }
        public String _Nom { get { return Nom; } }
        public String _Text { get { return Text; } }

        public Notification(int id, int id_user, String nom, String text)
        {
            this.Id = id;
            this.Id_user = id_user;
            this.Nom = nom;
            this.Text = text;
        }
    }
}
