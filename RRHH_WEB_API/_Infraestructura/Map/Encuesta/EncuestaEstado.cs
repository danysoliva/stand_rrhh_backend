using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using RRHH_WEB_API._Entidades.Encuesta;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map.Encuesta
{
    public class EncuestaEstadoMap
    {
        public EncuestaEstadoMap(EntityTypeBuilder<RRHH_WEB_API._Entidades.Encuesta.EncuestaEstado> builder)
        {
            builder.ToTable("encuesta_estado", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasColumnType("titulo").HasMaxLength(50);
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");

        }
    }
}
