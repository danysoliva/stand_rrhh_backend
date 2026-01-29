using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    public class ResourceResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }
        public string ResourceType { get; set; }
        public int CalendarId { get; set; }
        public string TZ { get; set; }
        public int CrateUID { get; set; }
        public DateTime CrateDate { get; set; }
        public int WriteUID { get; set; }
        public DateTime WriteDate { get; set; }

        public  Employee Empleado { get; set; }
    }
}
