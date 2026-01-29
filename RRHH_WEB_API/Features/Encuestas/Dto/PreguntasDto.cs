using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Encuestas.Dto
{
    public class PreguntasDto
    {
        public string Pregunta { get; set; }
        public string[] Opciones  { get; set; }
    }
}
