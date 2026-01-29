using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.GestionesVarias.Dto
{
    public class PlazaVacantePostulanteAdjuntoDto
    {
        public int Id { get; set; }
        public int PostulanteId { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public string ReferenceFileName { get; set; }
        public bool Enable { get; set; }
    }
}
