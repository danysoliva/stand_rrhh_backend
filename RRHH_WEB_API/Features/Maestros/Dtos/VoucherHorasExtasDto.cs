using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Maestros.Dtos
{
    public class VoucherHorasExtasDto
    {
        public string PayslipName { get; set; }
        public string State { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Identificacion { get; set; }
        public string EmployeeDepartment { get; set; }
        public string EmployeeJobName { get; set; }
        public string EmployeeJournal { get; set; }
        public string BarCode { get; set; }
        public string FechaPago { get; set; }
        public string Moneda { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public decimal TotalHorasExtras { get; set; }
        public decimal TotalCantidadHoras { get; set; }

        public List<DetalleHoras> Detalles { get; set; }


        public class DetalleHoras{
            public string Code { get; set; }
            public string CodeRelated { get; set; }
            public string Detalle { get; set; }
            public decimal CantidadHoras { get; set; }
            public decimal TotalLinea { get; set; }
            public int Orden { get; set; }
        }

    }
}
