using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RRHH_WEB_API._Entidades
{
    public class RequestConstancia
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int RequestStateId { get; set; } = (int)EstadoSolicitudEnum.EnProceso;
        public RequestType RequestType { get; set; }
        public string Comment { get; set; } = string.Empty;
        public RequestState RequestState { get; set; }
        public int ResquestTypeId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool Enable { get; set; } = true;

        public List<Employee> Empleados { get; set; }
        public List<RequestConstanciaItem> RequestConstanciaItems { get; set; }
    }
}
