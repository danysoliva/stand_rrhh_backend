using System.Collections.Generic;

namespace RRHH_WEB_API._Entidades
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompleteName { get; set; }
        public int CompanyId { get; set; }
        public int? ParentId { get; set; }
        public int ManagerId { get; set; }
        public int? PaymentAccountId { get; set; }
        public bool Active { get; set; }
        public List<Employee> Empleados { get; set; }
        public List<PlazaVacante>  PlazaVacantes { get; set; }
    }

    public enum Departamento
    {
        RecursosHumanos = 31,
        GerenteRecursosHumanos = 44
    }
}