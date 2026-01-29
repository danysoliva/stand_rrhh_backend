using RRHH_WEB_API._Entidades.QuejasSugerenciasDenuncias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades.QuejasSugerenciasDenuncias
{
    public class QuejaSugerenciaDenuncia
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string Descripcion { get; set; }
        public QuejaSugerenciaDenunciaState State { get; set; }
        public int TypeId { get; set; }
        public QuejaSugerenciaDenunciaType  Type { get; set; }
        public DateTime CreateDate { get; set; }

        public DateTime? LastModification { get; set ; }

        // Constructor de la clase
        public QuejaSugerenciaDenuncia()
        {
            // Establecer el valor predeterminado para MiFecha
            LastModification = new DateTime(1900, 1, 1);
        }
    }
}
