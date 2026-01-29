using System;
using System.Collections.Generic;

namespace RRHH_WEB_API._Entidades
{
    public class RequestConstanciaItem
    {
        public int RequestConstanciaId { get; set; }
        public RequestConstancia RequestConstancia { get; set; }
        public int RequestItemId { get; set; }
        public RequestItem RequestItem { get; set; }
        public int Moneda { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}
