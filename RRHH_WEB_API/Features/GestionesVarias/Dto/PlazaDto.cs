using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.GestionesVarias.Dto
{
    public class PlazaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Requisitos { get; set; }
        public int DepartmentId { get; set; }
        public string Departamento { get; set; }
        public string FechaCreacion { get; set; }
    }
}
