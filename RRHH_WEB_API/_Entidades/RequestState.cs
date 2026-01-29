using System.Collections.Generic;

namespace RRHH_WEB_API._Entidades
{
    public class RequestState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }

        public List<RequestConstancia> SolicitudesConstancias { get; set; }
        public List<RequestVacacion> SolicitudesVacaciones { get; set; }
    }

    public enum EstadoSolicitudEnum
    {
        EnProceso = 1,
        Aprobado = 2,
        Denegado = 3,
        AprobadoPorJefeInmediato = 4,
        RechazadoPorJefeInmediato = 5,
        AprobadoPorRRHH = 6,
        RechazadoPorRRHH = 7
    }
}
