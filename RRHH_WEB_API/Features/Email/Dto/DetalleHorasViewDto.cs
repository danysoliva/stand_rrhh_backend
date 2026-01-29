using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Email.Dto
{
    public class DetalleHorasViewDto
    {
        public string Serial { get; set; }
        public string Code { get; set; }
        public int EmpleadoId { get; set; }
        public string EmployeeName { get; set; }
        public decimal NormalHour { get; set; }
        public decimal ExtraHours { get; set; }
        public DateTime FechaI { get; set; }
        public DateTime FechaF { get; set; }
        public DateTime Fecha { get; set; }
        public string Departamento { get; set; }
        public int Semana { get; set; }
    }
}
