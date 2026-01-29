using RRHH_WEB_API._Entidades.Encuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades.Encuesta
{
    public class EncuestaPregunta
    {
        public int Id { get; set; }
        public int EncuestaId { get; set; }
        public EncuestaH Encuesta { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public bool Enable { get; set; }

        public List<EncuestaOpcion> Opciones { get; set; }

    }
}
