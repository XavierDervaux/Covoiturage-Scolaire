using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace covoiturage.main.singleton
{
    public static class ListCarburant
    {
        public static Carburant super95 = new Carburant(0, "Super 95", 0.51);
        public static Carburant super98 = new Carburant(1, "Super 98", 0.48);
        public static Carburant diesel = new Carburant(2, "Diesel", 0.35);

        public static Carburant trouverCarburant (int id){
            return ((super95._Id == id) ?  super95 : (super98._Id == id) ?  super98 :  diesel);
        }
    }
}
