using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    public class HoraEmpleadosRolDepartamento
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Activo { get; set; }

        public List<HoraEmpleadoNombre> horaEmpleadoNombres { get; set; }
    }
}
