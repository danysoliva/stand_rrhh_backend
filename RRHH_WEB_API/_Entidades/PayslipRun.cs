using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    public class PayslipRun
    {
        public  int Id { get; set; }
        public string Name { get; set; }
        public string State  { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool? CreditNote { get; set; }
        public int CreateUID { get; set; }
        public DateTime CreateDate { get; set; }
        public int? WriteUID { get; set; }
        public DateTime? WriteDate { get; set; }
        public int NumerOfDays { get; set; }
        public int PayRollTypeId { get; set; }
        public decimal? TotalInTransference { get; set; }
        public decimal Rate { get; set; }
        public int CurrencId { get; set; }
        public string PayslipNumber { get; set; }
        public string Observation { get; set; }

        public Payslip Payslip { get; set; }
    }
}
