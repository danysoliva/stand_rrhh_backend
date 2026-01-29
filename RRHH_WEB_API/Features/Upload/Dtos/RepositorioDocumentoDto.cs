using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Upload.Dtos
{
    public class RepositorioDocumentoDto
    {
        public int Id { get; set; }
        public string   Host { get; set; }
        public string   Path { get; set; }
        public string   FileName { get; set; }
        public string   FullPath { get; set; }
        public string   ReferenceFileName { get; set; }

        public int GrupoId { get; set; } = 0;
        public string GrupoDocumento { get; set; } = string.Empty;
    } 
}
