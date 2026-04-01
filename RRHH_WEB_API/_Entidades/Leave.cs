using System;

namespace RRHH_WEB_API._Entidades
{
    public class Leave
    {
        public int Id { get; set; }
        public string State  { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int DepartmentId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal NumberOfDays { get; set; }
        public int HolidayStatusId  { get; set; }
        public DateTime CreateDate { get; set; }
        public int EstadoId { get; set; }
    }
}
