using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class RepositoryGroupMap
    {
        public RepositoryGroupMap(EntityTypeBuilder<RepositoryGroup> builder)
        {
            builder.ToTable("repository_group", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasColumnType("varchar").HasMaxLength(50);
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");


    }
}
}
