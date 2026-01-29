using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    public class AutorizacionDeduccionPlanilla
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Empleado { get; set; }
        public Decimal Monto { get; set; }
        public DateTime FechaDeduccion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public int EstadoId { get; set; }
        public string Concepto { get; set; }
        public AutorizacionDeduccionPlanillaEstado EstadoDeduccionPorPlanilla { get; set; }
        public bool Enable { get; set; }
        public string Currency { get; set; }
        public decimal TasaCambio { get; set; }
    }
}
