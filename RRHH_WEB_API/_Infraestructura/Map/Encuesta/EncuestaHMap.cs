using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using RRHH_WEB_API._Entidades.Encuesta;
using System.Threading.Tasks;
using RRHH_WEB_API._Entidades.Encuestas;

namespace RRHH_WEB_API._Infraestructura.Map.Encuesta
{
    public class EncuestaHMap
    {
        public EncuestaHMap(EntityTypeBuilder<EncuestaH> builder)
        {
            builder.ToTable("encuesta_h", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Titulo).HasColumnName("titulo").HasColumnType("titulo");
            builder.Property(x => x.FechaCreacion).HasColumnName("fechaCreacion").HasColumnType("datetime");
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");
            builder.Property(x => x.EstadoId).HasColumnName("idEstado").HasColumnType("int");

            builder.HasOne(t => t.Estado).WithMany(r => r.Encuestas).HasForeignKey(u => u.EstadoId);

        }
    }
}
