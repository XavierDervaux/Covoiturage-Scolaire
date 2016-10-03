using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace covoiturage.main
{
    public class Adresse
    {
        private String Adress { get; set; }
        private String Numero { get; set; }
        private String Ville { get; set; }
        private String Cp { get; set; }
         
        public String _Adress { get { return Adress; } }
        public String _Numero { get { return Numero; } }
        public String _Ville { get { return Ville; } }
        public String _Cp { get { return Cp; } }

        public Adresse(String adresse, String numero, String ville, String cp)
        {
            this.Adress = adresse;
            this.Numero = numero;
            this.Ville = ville;
            this.Cp = cp;
        }
    }
}
