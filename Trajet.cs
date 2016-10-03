using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using covoiturage.main.user;

namespace covoiturage.main
{
    public class Trajet
    {
        private int Id { get; set; }
        private Adresse Depart { get; set; }
        private Adresse Arrivee { get; set; }

        private int NbrKm { get; set; }
        private int NbrInscript { get; set; }
        private int NbrPlaceVoiture { get; set; }
        private float NbrLitreVoiture {get; set;}
        private int HDepart { get; set; }
        private int HArrivee { get; set; }
        private Carburant Carbu { get; set; }
        private Utilisateur Util { get; set; }

        //GETTEUR PUBLIQUE
        public int _Id {get { return Id; }}
        public Adresse _Depart { get { return Depart; } }
        public Adresse _Arrivee { get { return Arrivee; } }

        public int _NbrKm { get { return NbrKm; } }
        public int _NbrInscript { get { return NbrInscript; } }
        public int _NbrPlaceVoiture { get { return NbrPlaceVoiture; } }
        public float _NbrLitreVoiture { get { return NbrLitreVoiture; } }
        public int _HDepart { get { return HDepart; } }
        public int _HArrivee { get { return HArrivee; } }
        public Carburant _Carburant { get { return Carbu; } }

        public Trajet(int id, Adresse depart, Adresse arrivee, int nbrKm,  int nbrPlaceVoiture, float nbrLitreVoiture, 
            Carburant carburant, int Hdepart, int Harrivee)
        {
            this.Id = id;
            this.Depart = depart;
            this.Arrivee = arrivee;
            this.NbrKm = nbrKm;
            this.NbrPlaceVoiture = nbrPlaceVoiture;
            this.NbrLitreVoiture = nbrLitreVoiture;
            this.Carbu = carburant;
            this.HDepart = Hdepart;
            this.HArrivee = Harrivee;
        }

        public int placeDispo()
        {
            return _NbrPlaceVoiture - SqlRequest.nbrInscrits(this);
        }

        public double calculerPrix()
        {
            double conso = _NbrLitreVoiture / 100;
            conso = conso * _Carburant._Prix;
            return conso * _NbrKm / SqlRequest.nbrInscrits(this);
        }

        public double calculerPrixMin()
        {
            double conso = _NbrLitreVoiture / 100;
            conso = conso * _Carburant._Prix;
            return conso * _NbrKm / _NbrPlaceVoiture;
        }
        public DateTime transformerTimestamp (int seconde)
        {
            DateTime vraiDate = new DateTime(1970, 1, 1, 0, 0, 0);
            return vraiDate.AddSeconds(seconde);
        }
    }
}