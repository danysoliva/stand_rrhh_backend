using System;
using System.Collections.Generic;

namespace RRHH_WEB_API.Features.Solicitud.Dtos
{
    public class SolicitudContanciaDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestType { get; set; }
        public int RequestStateId { get; set; }
        public string RequestState { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool EsVistaRRHHAdministrador { get; set; }
        public bool EsAdministrador { get; set; }
    }

    public class NuevaSolicitudConstanciaDto
    {
        public int TipoConstanciaId { get; set; }        
    }
}
