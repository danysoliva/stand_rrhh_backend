using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class RequestStateMap
    {
        public RequestStateMap(EntityTypeBuilder<RequestState> builder)
        {
            builder.ToTable("request_state", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar").HasMaxLength(50);
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");
            //builder.Property(x => x.ReasignacionCola).HasColumnName("reasignacion_cola").HasColumnType("int");

            //builder.HasOne(x => x.UserDelegation).WithOne(x => x.Employee);
        }
    }
}
