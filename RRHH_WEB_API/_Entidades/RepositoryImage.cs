namespace RRHH_WEB_API._Entidades
{
    public class RepositoryImage
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Host { get; set; }
        public string ReferenceFileName { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
