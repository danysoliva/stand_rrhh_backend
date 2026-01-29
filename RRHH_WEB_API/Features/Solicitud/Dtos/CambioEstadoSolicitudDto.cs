using System.Collections.Generic;

namespace RRHH_WEB_API.Features.Solicitud.Dtos
{
    public class CambioEstadoSolicitudDto
    {
        public int SolicitudId { get; set; }
        public int EstadoId { get; set; }
        public string Comentario { get; set; }
        public List<ConceptoDto> Conceptos { get; set; }
    }
}
