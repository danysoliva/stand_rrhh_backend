using RRHH_WEB_API._Entidades.Encuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades.Encuesta
{
    public class EncuestaEstado
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Enable { get; set; }

        public List<EncuestaH> Encuestas { get; set; }
    }
}
