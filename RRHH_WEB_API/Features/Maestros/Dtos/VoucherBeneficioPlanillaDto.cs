using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Maestros.Dtos
{
    public class VoucherBeneficioPlanillaDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CurrencyName { get; set; }
        public decimal Monto { get; set; }
        public int Orden { get; set; }
    }
}
