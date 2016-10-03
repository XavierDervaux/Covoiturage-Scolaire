using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace covoiturage.main
{
    public class Carburant
    {
        private int Id { get; set; }
        private String Nom { get; set; }
        private double Prix { get; set; }

        public int _Id { get { return Id; } }
        public String _Nom { get { return Nom; } }
        public double _Prix { get { return Prix; } }

        public Carburant() { }

        public Carburant(int id, String nom, double prix)
        {
            this.Id = id;
            this.Nom = nom;
            this.Prix = prix;
        }
    }
}
