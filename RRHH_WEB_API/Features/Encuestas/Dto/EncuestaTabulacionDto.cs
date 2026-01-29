using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Encuestas.Dto
{
    public class EncuestaTabulacionDto
    {
        public int PreguntaId { get; set; }
        public string Pregunta { get; set; }
        public int OpcionId { get; set; }
        public string Opcion { get; set; }
        public int Conteo { get; set; }
    }
}
