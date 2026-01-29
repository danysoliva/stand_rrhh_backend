using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Encuestas.Dto
{
    public class EncuestaResponseDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartSurveyText { get; set; }
        public List<EncuestaResponsePageDto> Pages { get; set; }
    }


}
