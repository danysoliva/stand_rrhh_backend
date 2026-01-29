using System;
using System.Collections.Generic;

namespace RRHH_WEB_API.Features.Solicitud.Dtos
{
    public class ValidarVacacionDto
    {
        public TipoVerificacionEnum TipoVerificacion { get; set; }
        public decimal CantidadDiasVacacion { get; set; }
        public Jornada Jornada { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaReintegro { get; set; }
        
        
        public virtual List<DateTime> ListadoFechas { get; set; }
        public virtual List<FechaVacacion> ListadoFechasVacacion { get; set; }
    }

    public class FechaVacacion
    {
        public DateTime Fecha { get; set; }
        public decimal CantidadDiaFeriado { get; set; }
        public decimal CantidadDiaVacacion { get; set; }
        public bool EsDiaCompleto { get; set; }
        public bool EsFeriadoCompleto { get; set; }
        public bool EsMedioFeriado { get; set; }
        public bool EsSabado { get; set; }
        public bool EsDomingo { get; set; }
    }

    public enum TipoVerificacionEnum
    {
        PorFecha = 1,
        PorDias = 2,
        PorJornada = 3
    }

    public enum Jornada
    {
        Ocultar = 0,
        Mañana = 1,
        Tarde = 2
    }
}
