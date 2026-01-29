using System.Collections.Generic;

namespace RRHH_WEB_API._Entidades
{
    public class RepositoryGroup
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public bool Enable { get; set; } = true;

        public List<RepositoryDocument> RepositoryDocument  { get; set; }

    }

}
