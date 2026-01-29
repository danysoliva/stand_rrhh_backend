using System.ComponentModel.DataAnnotations;

namespace RRHH_WEB_API._Entidades
{
    public class RepositoryDocument
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Host { get; set; }
        public string ReferenceFileName { get; set; }
        public bool Enabled { get; set; } = true;
        public int Tipo { get; set; }

        //[Key]
        public int? GrupoID { get; set; }
        public RepositoryGroup? RepositoryGroup { get; set; } = new RepositoryGroup();

    }

    public enum TipoDocumentoEnum
    {
        Formatos = 1,
        Politicas = 2
    }
}
