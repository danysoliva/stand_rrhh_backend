using System;
using System.Collections.Generic;

namespace RRHH_WEB_API._Entidades
{
    public class Contract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int? DepartmentId { get; set; }
        public int? TypeId { get; set; }
        public int? JobId { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? Trial_DateEnd { get; set; }
        public int ResourceCalendarId { get; set; }
        public decimal? Wage { get; set; }
        public string State { get; set; }

        //public List<Employee> Empleados { get; set; }
        public List<BenefitDeduction> BenefitDeductions { get; set; }

    }
}
