using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class RepositoryDocumentMap
    {
        public RepositoryDocumentMap(EntityTypeBuilder<RepositoryDocument> builder)
        {
            builder.ToTable("repository_document", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.FileName).HasColumnName("file_name").HasColumnType("varchar");
            builder.Property(x => x.Path).HasColumnName("path").HasColumnType("varchar");
            builder.Property(x => x.Host).HasColumnName("host").HasColumnType("varchar");
            builder.Property(x => x.ReferenceFileName).HasColumnName("reference_name").HasColumnType("varchar");
            builder.Property(x => x.Enabled).HasColumnName("enabled").HasColumnType("bit");
            builder.Property(x => x.GrupoID).HasColumnName("id_grupo").HasColumnType("int");

            builder.HasOne(x => x.RepositoryGroup).WithMany(f => f.RepositoryDocument).HasForeignKey(t => t.GrupoID);


        }
    }
}
