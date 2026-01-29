using System.Collections.Generic;

namespace RRHH_WEB_API._Entidades
{
    public class RequestType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }

        public List<RequestConstancia> SolicitudesConstancias { get; set; }
    }

    public enum TipoSolicitudEnum
    {
        ConstanciaDeTrabajo = 1
    }
}
