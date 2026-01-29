using System;

namespace RRHH_WEB_API._Entidades
{
    public class PeriodoVacacion
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int Year { get; set; }
        public int Days { get; set; }        
    }
}
