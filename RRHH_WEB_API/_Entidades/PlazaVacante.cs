using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    public class PlazaVacante
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public Department Departamento { get; set; }
        public string Titulo { get; set; }
        public string Requisitos { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Enable { get; set; }

        public List<PlazaVacantePostulante> PlazaVacantePostulantes { get; set; }
    }
}
