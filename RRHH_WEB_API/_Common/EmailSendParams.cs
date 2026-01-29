using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Common
{
    public class EmailSendParams
    {
        public List<string> Destinatarios { get; set; }
        public List<string> Copias { get; set; }
        public string Subject { get; set; }
        public string Nombre { get; set; }
        public string Body { get; set; }
    }
}
