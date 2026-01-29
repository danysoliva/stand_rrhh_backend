using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.GestionesVarias.Dto
{
    public class PostulantesAdminDto
    {
        public int Id { get; set; }
        public int plazaVacanteId { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string RecomendadoOInterno { get; set; }
        public List<AdjuntosPostulante> Adjuntos { get; set; }


        public class AdjuntosPostulante
        {
            public string  URL { get; set; }
            public string  FileNameReference { get; set; }
        }
    }
}
