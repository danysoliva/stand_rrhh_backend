using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Email.Dto
{
    public class EnviarDetalleHorasParamsDto
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int EmployeeId { get; set; }
    }
}
