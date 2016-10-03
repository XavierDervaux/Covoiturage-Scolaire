using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace covoiturage.main.singleton
{
    class ListRequest
    {
        private List<SqlCommand> listL;
        private static ListRequest _instance = null;

        public List<SqlCommand> Listl
        {
            get { return listL; }
            set { listL = value; }
        }

        private ListRequest()
        {
            listL = new List<SqlCommand>();
        }

        public static ListRequest Instance()
        {
            if (_instance == null)
            {
                _instance = new ListRequest();
            }
            return _instance;
        }

        public void Add(SqlCommand util)
        {
            listL.Add(util);
        }

        public void Remove(SqlCommand util)
        {
            listL.Remove(util);
        }
    }
}
