using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.GestionesVarias.Dto
{
    public class PlazaVacantePostulanteDto
    {
        public int Id { get; set; }
        public int EmpleadoId { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public bool EsRecomendado { get; set; }
        public int plazaVacanteId { get; set; }

        //public List<PlazaVacantePostulanteAdjuntoDto> Adjuntos { get; set; }

    }
}
