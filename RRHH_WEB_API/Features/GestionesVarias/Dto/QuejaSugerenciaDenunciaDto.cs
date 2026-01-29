using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.GestionesVarias.Dto
{
    public class QuejaSugerenciaDenunciaDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int StateId { get; set; }
        public string Estado { get; set; }
        public string CreateDate { get; set; }
        public int TypeId { get; set; }
        public string Tipo { get; set; }
        public DateTime LastModification { get; set; }
    }
}
