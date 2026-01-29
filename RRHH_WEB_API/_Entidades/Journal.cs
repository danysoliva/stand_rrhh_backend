using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    public class Journal
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public Boolean  Enable { get; set; }

        public List<Employee> Empleados { get; set; }
    }
}
