using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Maestros.Dtos
{
    public class VoucherResponseDto
    {
        public int PayRolTypeId { get; set; }

        public VoucherDto Voucher { get; set; }

        public VoucherHorasExtasDto VoucherHorasExtas { get; set; }
       
    }
}
