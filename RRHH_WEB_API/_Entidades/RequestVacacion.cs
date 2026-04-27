using System;
using System.Collections.Generic;

namespace RRHH_WEB_API._Entidades
{
    public class RequestVacacion
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        //public Employee Employee { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaReintegro { get; set; }
        public decimal CantidadDiasVacacion { get; set; }
        public string CubreVacaciones { get; set; }
        public string Observaciones { get; set; }
        public int JefeInmediatoId { get; set; }
        public int RequestStateId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool SincronizadoEnOdoo { get; set; } = false;
        public RequestState RequestState { get; set; }
        public bool Enable { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? TipoVacacionId { get; set; }
        public string? ActividadesPendientes { get; set; } = string.Empty;
        public List<RequestVacacionTracking> RequestVacacionesTracking { get; set; }
    }
}
