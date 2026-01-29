using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    public class HoraEmpleadoNombre
    {
        public int Id { get; set; }
        public string   Codigo { get; set; }
        public string   Nombre { get; set; }
        public int   DepartamentoId { get; set; }
        public int  GrupoId { get; set; }
        public string  EmpleadoId { get; set; }

        public string  WorkEmail { get; set; }
        public bool Marking { get; set; }
        public DateTime FechaC { get; set; }
        public bool Active { get; set; }
        public DateTime XHourIn { get; set; }
        public DateTime XHourOut { get; set; }
        public int  RollId { get; set; }

        public HoraEmpleadosRolDepartamento HoraEmpleadoDepartamento { get; set; }

        public List<HoraEmpleadoTrabajada> horaEmpleadoTrabajadas { get; set; }
        //public HoraEmpleadoTrabajada  horaEmpleadoTrabajada { get; set; }
    }
}
