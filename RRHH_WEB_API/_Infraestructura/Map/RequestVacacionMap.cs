using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class RequestVacacionMap
    {
        public RequestVacacionMap(EntityTypeBuilder<RequestVacacion> builder)
        {
            builder.ToTable("request_vacaciones", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.EmployeeId).HasColumnName("id_employee").HasColumnType("int");
            builder.Property(x => x.FechaInicio).HasColumnName("fecha_inicio").HasColumnType("datetime");
            builder.Property(x => x.FechaFin).HasColumnName("fecha_fin").HasColumnType("datetime");
            builder.Property(x => x.FechaReintegro).HasColumnName("fecha_reintegro").HasColumnType("datetime");
            builder.Property(x => x.CantidadDiasVacacion).HasColumnName("cant_dias_vacaciones").HasColumnType("numeric");
            builder.Property(x => x.CubreVacaciones).HasColumnName("cubre_vacaciones").HasColumnType("varchar");
            builder.Property(x => x.Observaciones).HasColumnName("observaciones").HasColumnType("varchar");
            builder.Property(x => x.JefeInmediatoId).HasColumnName("id_jefe_inmediato").HasColumnType("int");
            builder.Property(x => x.RequestStateId).HasColumnName("id_estado").HasColumnType("int");
            builder.Property(x => x.Comment).HasColumnName("comment").HasColumnType("varchar");
            builder.Property(x => x.SincronizadoEnOdoo).HasColumnName("sincronizado_en_odoo").HasColumnType("bit");
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");
            builder.Property(x => x.CreatedDate).HasColumnName("created_date").HasColumnType("datetime");
            builder.Property(x => x.TipoVacacionId).HasColumnName("id_tipo_vacaciones").HasColumnType("int");
            builder.Property(x => x.ActividadesPendientes).HasColumnName("actividades_pendientes").HasColumnType("varchar").HasMaxLength(300);

            builder.HasOne(x => x.RequestState).WithMany(y => y.SolicitudesVacaciones).HasForeignKey(z=> z.RequestStateId);
            
        }
    }
}
