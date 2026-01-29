using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Maestros.Dtos
{
    public class VoucherBeneficiosDto
    {
        public Decimal DiasVacaciones { get; set; }
        public Decimal DiasFaltados { get; set; }
        public Decimal SalarioBase { get; set; }
        public Decimal Vacaciones { get; set; }
        public Decimal Bono { get; set; }
    }
}
