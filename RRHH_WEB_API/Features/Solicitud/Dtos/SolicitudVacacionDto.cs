using System;

namespace RRHH_WEB_API.Features.Solicitud.Dtos
{
    public class SolicitudVacacionDto
    {
        public int Id { get; set; }
        public decimal CantidadDiasVacacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaReintegro { get; set; }
        public string CubreVacaciones { get; set; }
        public string Observaciones { get; set; }
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public int JefeInmediatoId { get; set; }
        public string JefeInmediato { get; set; }
        public string MailJefeInmediato { get; set; }
        public int RequestStateId { get; set; }
        public string RequestState { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool EsVistaRRHHAdministrador { get; set; }
        public bool EsVistaAdministrador { get; set; }
        public bool EsVistaJefatura { get; set; }
    }


    public class NuevaSolicitudVacacionDto
    {
        public decimal CantidadDiasVacacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaReintegro { get; set; }
        public string CubreVacaciones { get; set; }
        public string Observaciones { get; set; }
        public string ActividadesPendientes { get; set; }
        public TipoVacacionDto TipoVacacion { get; set; }
    }
}
