using System;
using System.Collections.Generic;

namespace RRHH_WEB_API._Entidades
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public string IdentificationId { get; set; }
        public string Pin { get; set; }
        public string BarCode { get; set; }
        public string MobilePhone { get; set; }
        public string? WorkEmail { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        public int? JournalId { get; set; }
        public Journal Journal { get; set; }
        public int ResourceId { get; set; }
        public ResourceResource Resource { get; set; }
        public string ShirtSize { get; set; }
        public string PantSize { get; set; }
        public string ShoeSize { get; set; }
        public int? Height { get; set; }
        public int? Weigth { get; set; }
        public int? JobId { get; set; }
        public Job Job { get; set; }
        public int? ParentId { get; set; }
        public virtual Employee Parent { get; set; }
        public byte[] Image { get; set; }

        public bool EsEmpleadoAdministrador()
        {
            return UserDelegation.UserLevelId == (int)UserLevelEnum.Administrador;
        }

        public bool EsEmpleadoRRHHAdministrador()
        {
            return UserDelegation.UserLevelId == (int)UserLevelEnum.Administrador && (DepartmentId == (int)Departamento.RecursosHumanos || DepartmentId == (int)Departamento.GerenteRecursosHumanos);
        }

        public bool EsEmpleadoNormal()
        {
            return UserDelegation.UserLevelId == 0 || UserDelegation.UserLevelId == (int)UserLevelEnum.Usuario;
        }

        public UserDelegation UserDelegation { get; set; } = new UserDelegation();
        public Contract Contract { get; set; }
        public RequestConstancia SolicitudConstancia { get; set; }

        
        public List<PeriodoVacacion> PeriodosVacacion { get; set; }
        public List<Leave> Leaves { get; set; }
        public List<Payslip> Paslips { get; set; }
        public List<AutorizacionDeduccionPlanilla> Deducciones { get; set; }
        public List<PlazaVacantePostulante> Postulantes { get; set; }
    }
}
