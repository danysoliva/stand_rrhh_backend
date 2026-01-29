using RRHH_WEB_API._Entidades.Encuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades.Encuesta
{
    public class EncuestaOpcion
    {
        public int Id { get; set; }
        public int EncuestaId { get; set; }
        public EncuestaH Encuesta { get; set; }
        public int PreguntaId { get; set; }
        public EncuestaPregunta Pregunta { get; set; }
        public string Opcion { get; set; }
        public bool Enable { get; set; }
    }
}
