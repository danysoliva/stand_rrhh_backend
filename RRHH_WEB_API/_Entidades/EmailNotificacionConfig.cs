using System;

namespace RRHH_WEB_API._Entidades
{
    public class EmailNotificacionConfig
    {
        public int Id { get; set; }
        public string EventCode { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
