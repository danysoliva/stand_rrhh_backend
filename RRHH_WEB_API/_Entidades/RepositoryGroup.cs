using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RRHH_WEB_API._Entidades
{
    public class RepositoryGroup
    {
        [Key]
        public int Id { get; set; }
        public string? Descripcion { get; set; } = string.Empty;
        public bool Enable { get; set; } = true;

        //public List<RepositoryDocument> RepositoryDocument  { get; set; }
        public virtual ICollection<RepositoryDocument> RepositoryDocument { get; set; } = new List<RepositoryDocument>();
    }

}
