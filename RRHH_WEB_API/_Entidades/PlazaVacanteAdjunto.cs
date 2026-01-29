using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    public class PlazaVacanteAdjunto
    {
        public int Id { get; set; }
        public int PlazaVacantePostulanteId { get; set; }
        public PlazaVacantePostulante PlazaVacantePostulante { get; set; }
        public string Host { get; set; }
        public string ReferenceFileName { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public bool Enable { get; set; }
    }
}
