using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    [Serializable]
    public class HoraEmpleadoTrabajada
    {
        public int Id { get; set; }
        public string? EmpleadoId { get; set; }
        public TimeSpan? HoraI { get; set; }
        public TimeSpan? HoraF { get; set; }
        public decimal Cantidad { get; set; }
        public int? EmployeeId { get; set; }
        public HoraEmpleadoNombre Employee { get; set; }
        public DateTime Fecha { get; set; }
        public bool Enable { get; set; }
        public decimal CantidadDe { get; set; }
        public DateTime? FechaI { get; set; } = Convert.ToDateTime("1999-01-01");
        public DateTime? FechaF { get; set; } = Convert.ToDateTime("1999-01-01");
        public int? Week { get; set; }
        public int? Tipo { get; set; }


        public HoraEmpleadoTrabajada()
        {
            FechaI= Convert.ToDateTime("1999-01-01");
            FechaF= Convert.ToDateTime("1999-01-01");
        }
    }
}
