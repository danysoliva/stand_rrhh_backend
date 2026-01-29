using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class RequestItemMap
    {
        public RequestItemMap(EntityTypeBuilder<RequestItem> builder)
        {
            builder.ToTable("request_item", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar").HasMaxLength(150);
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");
            builder.Property(x => x.CreatedDate).HasColumnName("created_date").HasColumnType("datetime");
        }
    }
}
