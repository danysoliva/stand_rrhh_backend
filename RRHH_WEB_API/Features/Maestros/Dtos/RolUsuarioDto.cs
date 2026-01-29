using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Maestros.Dtos
{
    public class RolUsuarioDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Code { get; set; }
        public string EmployeeName { get; set; }
        public int NivelUsuarioId { get; set; }
        public string NivelUsuario { get; set; }
    }
}
