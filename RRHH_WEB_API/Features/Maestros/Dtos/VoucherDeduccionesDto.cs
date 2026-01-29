using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Maestros.Dtos
{
    public class VoucherDeduccionesDto
    {
        public decimal AhorroRetiroCooperativa { get; set; }
        public decimal AhorroFijoCooperativa { get; set; }
        public decimal Bancos { get; set; }
        public decimal Cafeteria { get; set; }
        public decimal IHSS { get; set; }
        public decimal AFP { get; set; }
        public decimal Incapacidades { get; set; }
        public decimal ISR { get; set; }
    }
}
