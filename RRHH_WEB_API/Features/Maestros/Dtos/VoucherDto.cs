using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Maestros.Dtos
{
    public class VoucherDto
    {
        public long Id { get; set; }
        public string PayslipName { get; set; }
        public string PayslipRunName { get; set; }
        public string State { get; set; }
       
        public int EmployeeId { get; set; }
        public string BarCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeDepartment { get; set; }
        public string EmployeeJobName { get; set; }
        public string EmployeeJournal { get; set; }
        
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string FechaPago { get; set; }
        public string Moneda { get; set; }


        //Deducciones
        public decimal AhorroRetiroCooperativa { get; set; }
        public decimal AhorroFijoCooperativa { get; set; }
        public decimal Bancos { get; set; }
        public decimal Cafeteria { get; set; }
        public decimal IHSS { get; set; }
        public decimal AFP { get; set; }
        public decimal Incapacidades { get; set; }
        public decimal ISR { get; set; }
        public decimal USULA { get; set; }

        //Beneficios
        public Decimal DiasVacaciones { get; set; }
        public Decimal DiasLaborados { get; set; }
        public Decimal DiasFaltados { get; set; }
        public Decimal SalarioBase { get; set; }
        public Decimal Vacaciones { get; set; }
        public Decimal Bono { get; set; }


        //Resumen
        public decimal TotalEgresos { get; set; }
        public decimal SalarioNeto { get; set; }
        public decimal TotalIngresos { get; set; }


        public List<VoucherDeduccionPlanillaDto> Deducciones { get; set; }
        public List<VoucherBeneficioPlanillaDto> Beneficios { get; set; }

        //public VoucherDeduccionesDto Deducciones { get; set; }
        //public VoucherBeneficiosDto Beneficios { get; set; }


        
    }
}
