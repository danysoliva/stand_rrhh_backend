using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Encuestas.Dto
{
    public class EncuestaFiltroDto
    {
        public List<EncuestaEstadoFiltroDto> Estados { get; set; }
        public List<EncuestaNameFiltroDto> Encuestas { get; set; }
    }

  
}
