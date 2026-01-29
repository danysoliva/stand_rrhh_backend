using System;
using System.Collections.Generic;

namespace RRHH_WEB_API._Entidades
{
    public class RequestVacacionTracking
    {
        public int Id { get; set; }
        public int RequestVacacionId { get; set; }
        public RequestVacacion RequestVacacion { get; set; }
        public string Descripcion { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
