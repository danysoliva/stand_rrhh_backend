using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class RequestConstanciaMap
    {
        public RequestConstanciaMap(EntityTypeBuilder<RequestConstancia> builder)
        {
            builder.ToTable("request_constancia", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.EmployeeId).HasColumnName("id_employee").HasColumnType("int");
            builder.Property(x => x.ResquestTypeId).HasColumnName("id_tipo_solicitud").HasColumnType("int");
            builder.Property(x => x.RequestStateId).HasColumnName("id_estado").HasColumnType("int");
            builder.Property(x => x.Comment).HasColumnName("comment").HasColumnType("varchar");
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");
            builder.Property(x => x.CreatedDate).HasColumnName("created_date").HasColumnType("datetime");

            builder.HasOne(x => x.RequestType).WithMany(x => x.SolicitudesConstancias).HasForeignKey(x => x.ResquestTypeId);
            builder.HasOne(x => x.RequestState).WithMany(x => x.SolicitudesConstancias).HasForeignKey(x => x.RequestStateId);
        }
    }
}
