using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Common
{
    public class ValidarNulls
    {

        public ValidarNulls()
        {

        }

        public int ValidarEnteros(int ob)
        {
            if (ob == null)
            {
                return 0;
            }
            else
                if (object.ReferenceEquals(null, ob))
            {
                return 0;
            }
            else
            {
                return ob;
            }
        }

        public string ValidarString(string ob)
        {
            if (ob == null)
            {
                return "";
            }
            else
                if (object.ReferenceEquals(null, ob))
            {
                return "";
            }
            else
            {
                return ob;
            }
        }
    }
}
