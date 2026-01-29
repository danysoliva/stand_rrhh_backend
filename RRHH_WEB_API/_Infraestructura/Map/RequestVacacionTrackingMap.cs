using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class RequestVacacionTrackingMap
    {
        public RequestVacacionTrackingMap(EntityTypeBuilder<RequestVacacionTracking> builder)
        {
            builder.ToTable("request_vacaciones_tracking", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.RequestVacacionId).HasColumnName("id_request_vacacion").HasColumnType("int");
            builder.Property(x => x.Descripcion).HasColumnName("description").HasColumnType("varchar");
            builder.Property(x => x.CreatedDate).HasColumnName("created_date").HasColumnType("datetime");

            builder.HasOne(x => x.RequestVacacion).WithMany(x => x.RequestVacacionesTracking).HasForeignKey(x => x.RequestVacacionId);
        }
    }
}
