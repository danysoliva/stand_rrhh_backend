using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Encuestas.Dto
{
    public class EncuestaReponseElementDto
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public List<string> Choices { get; set; }
        public List<EncuestaReponseOptionDto> ChoicesWithId { get; set; }
        //public string Html { get; set; }
        //public string Description { get; set; }
        public int ColCount { get; set; }
        //public int MaxLength { get; set; }
    }
}
