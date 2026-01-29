using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Encuestas.Dto
{
    public class EncuestaAnswerDto
    {
        
            public int PreguntaId { get; set; }
            public int OpcionId { get; set; }
        public int EncuestaId { get; set; }
        //public string Explique { get; set; }

    }
}
