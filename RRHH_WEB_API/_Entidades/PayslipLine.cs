using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    public class PayslipLine
    {
        public long Id { get; set; }
        public int PayslipId { get; set; }
        public Payslip Payslip { get; set; }
        public int SalaryRuleId { get; set; }
        public int? EmployeId { get; set; }
        public int? ContractId { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; } = 0;
        public decimal? Quantity { get; set; }
        public decimal? Total { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? CategoryId { get; set; }
        public bool Active { get; set; }
        public bool? AppearsOnPayslip { get; set; }
        public string?    Note { get; set; }
        public int? CreateUID { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? GroupById { get; set; }
        public int? AmountIsrDeductible { get; set; }
        public int? AccountId { get; set; }
        public int? AnalyticAccountId { get; set; }

        //public List<PayslipRun> PayslipRuns { get; set; }
    }
}
