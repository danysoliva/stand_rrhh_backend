using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades.QuejasSugerenciasDenuncias
{
    public class QuejaSugerenciaDenunciaState
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Enable { get; set; }


        public List<QuejaSugerenciaDenuncia> QuejasSugerenciasDenuncias { get; set; }
    }
}
