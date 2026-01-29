using System;
using System.Collections.Generic;

namespace RRHH_WEB_API._Entidades
{
    public class RequestItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<RequestConstanciaItem> RequestConstanciaItems { get; set; }
    }
}
