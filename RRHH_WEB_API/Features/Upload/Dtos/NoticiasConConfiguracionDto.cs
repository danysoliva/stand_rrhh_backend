using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Upload.Dtos
{
    public class NoticiasConConfiguracionDto
    {
        public int DuracionImagenes { get; set; }
        public List<RepositorioImagenesDto> RepositorioImagenes { get; set; }
    }
}
