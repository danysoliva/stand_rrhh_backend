using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    public class PlazaVacantePostulante
    {
        public int Id { get; set; }
        public int PlazaVacanteId { get; set; }
        public PlazaVacante PlazaVacante { get; set; }
        public int EmpleadoId { get; set; }
        public Employee Empleado { get; set; }
        public string NombrePostulante { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public bool EsRecomendado { get; set; }
        public bool Enable { get; set; }

        public List<PlazaVacanteAdjunto> Adjuntos { get; set; }

    }
}
