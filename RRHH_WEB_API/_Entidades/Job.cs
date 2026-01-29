using System.Collections.Generic;

namespace RRHH_WEB_API._Entidades
{
    public class Job
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId  { get; set; }
        public int CompanyId { get; set; }

        public List<Employee> Empleados { get; set; }
    }
}
