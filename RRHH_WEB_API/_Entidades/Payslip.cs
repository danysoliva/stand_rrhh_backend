using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    public class Payslip
    {
        public int Id { get; set; }
        public int? StructId { get; set; }
        public string? Name { get; set; }
        public string? Number { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string? State { get; set; }
        public int? CompanyId { get; set; }
        public int? ContractId { get; set; }
        public bool? Paid { get; set; }
        public int? PayslipRunId { get; set; }
        public PayslipRun PayslipRun { get; set; }
        public int? CreateUID { get; set; }
        public DateTime?  CreateDate { get; set; }
        public int? WriteUID { get; set; }
        public DateTime? WriteDate { get; set; }
        public bool? CreditNote { get; set; }
        public bool? Enable { get; set; }


        public PayslipLine? PayslipLine { get; set; }

    }
}
