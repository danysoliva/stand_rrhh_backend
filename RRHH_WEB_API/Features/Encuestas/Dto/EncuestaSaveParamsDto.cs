using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Encuestas.Dto
{
    public class EncuestaSaveParamsDto
    {
        public string   Titulo { get; set; }
        public List<PreguntasDto> Preguntas { get; set; }
    }
}
