using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.GestionesVarias.Dto
{
    public class DeduccionDto
    {
        public int Id { get; set; }
        public string? NombreEmpleado { get; set; }
        public string Barcode { get; set; }
        public string Identidad { get; set; }
        public string Concepto { get; set; }
        public string Currency { get; set; }
        public Decimal Monto { get; set; }
        public DateTime FechaDeduccion { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
