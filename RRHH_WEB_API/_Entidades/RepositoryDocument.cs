using Nest;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RRHH_WEB_API._Entidades
{
    public class RepositoryDocument
    {
        [Key]
        public int Id { get; set; }
        public string Path { get; set; }

        [Column("file_name")]
        public string FileName { get; set; }
        public string Host { get; set; }

        [Column("reference_name")]
        public string ReferenceFileName { get; set; }
        public bool Enabled { get; set; } = true;
        public int Tipo { get; set; }

        [Column("id_grupo")]
        public int? GrupoID { get; set; }

        [ForeignKey("GrupoID")]
        public virtual RepositoryGroup RepositoryGroup { get; set; }

    }

    public enum TipoDocumentoEnum
    {
        Formatos = 1,
        Politicas = 2
    }
}
