using System;

namespace RRHH_WEB_API.Features.Solicitud.Dtos
{
    public class VacacionDto
    {
        public int EmployeeId { get; set; }
        public string Barcode { get; set; }
        public string Employee { get; set; }
        public string Job { get; set; }
        public string Department { get; set; }
        public string FechaIngreso { get; set; }
        public decimal CantidadDiasVacacion { get; set; }
        public string CantidadDiasVacacionEnLetras { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Observaciones { get; set; }
        public string CubreVacaciones { get; set; }
        public string FechaReintegro { get; set; }
        public int JefeInmediatoId { get; set; }
        public string JefeInmediato { get; set; } = string.Empty;
        public string ActividadesPendientes { get; set; } = string.Empty;
        public int TipoVacacionId { get; set; }
        public string TipoVacacionName { get; set; } = string.Empty;
    }
}
