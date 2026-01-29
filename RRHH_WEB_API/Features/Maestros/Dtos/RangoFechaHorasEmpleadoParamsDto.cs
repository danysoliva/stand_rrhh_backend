using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Maestros.Dtos
{
    public class RangoFechaHorasEmpleadoParamsDto
    {
        public int EmployeeId { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
    }
}
