using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class FeriadoMap
    {
        public FeriadoMap(EntityTypeBuilder<Feriado> builder)
        {
            builder.ToTable("hr_feriados", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasColumnType("varchar");
            builder.Property(x => x.FechaInicio).HasColumnName("fechai").HasColumnType("datetime");
            builder.Property(x => x.FechaFin).HasColumnName("fechaf").HasColumnType("datetime");
            builder.Property(x => x.CantidadDias).HasColumnName("cantidad_dias").HasColumnType("numeric");
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");
            builder.Property(x => x.FechaCreado).HasColumnName("fecha_creado").HasColumnType("datetime");
        }
    }
}
