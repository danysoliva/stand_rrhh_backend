using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Encuestas.Dto
{
    public class EncuestaResponsePageDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<EncuestaReponseElementDto > Elements { get; set; }
    }
}
