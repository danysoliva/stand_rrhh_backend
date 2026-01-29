using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    public class Feriado
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal CantidadDias { get; set; }
        public bool Enable { get; set; }
        public DateTime FechaCreado { get; set; }
    }
}
