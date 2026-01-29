using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.GestionesVarias.Dto
{
    public class ParamsDeduccionPlanillaDto
    {
        public int EmployeeId { get; set; }
        public Decimal Monto { get; set; }
        public string FechaDeduccion { get; set; }
        public string Concepto { get; set; }
        public string Currency { get; set; }
        public int UsuarioCreacionId { get; set; }
    }
}
