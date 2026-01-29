using RRHH_WEB_API._Entidades.Encuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades.Encuestas
{
    public class EncuestaH
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int  EstadoId { get; set; }
        public EncuestaEstado Estado { get; set; }
        public bool Enable { get; set; }

        public List<EncuestaPregunta> Preguntas { get; set; }
        //public List<EncuestaOpcion> Opciones { get; set; }
        //public List<EncuestaRespuesta> Respuestas { get; set; }

    }
}
