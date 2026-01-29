using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class RequestConstanciaItemMap
    {
        public RequestConstanciaItemMap(EntityTypeBuilder<RequestConstanciaItem> builder)
        {
            builder.ToTable("request_constancia_item", "rrhh_web");
            builder.HasKey(x => new { x.RequestConstanciaId, x.RequestItemId });
            builder.Property(x => x.RequestConstanciaId).HasColumnName("id_request_constancia").HasColumnType("int");
            builder.Property(x => x.RequestItemId).HasColumnName("id_request_item").HasColumnType("int");
            builder.Property(x => x.Name).HasColumnName("name_request_item").HasColumnType("varchar");
            builder.Property(x => x.Value).HasColumnName("value_request_item").HasColumnType("numeric");
            builder.Property(x => x.Moneda).HasColumnName("moneda_request_item").HasColumnType("int");

            builder.HasOne(x => x.RequestConstancia).WithMany(x => x.RequestConstanciaItems).HasForeignKey(x => x.RequestConstanciaId);
            builder.HasOne(x => x.RequestItem).WithMany(x => x.RequestConstanciaItems).HasForeignKey(x => x.RequestItemId);
        }
    }
}
