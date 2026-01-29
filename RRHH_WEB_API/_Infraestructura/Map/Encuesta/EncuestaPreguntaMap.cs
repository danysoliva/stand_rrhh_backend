using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using RRHH_WEB_API._Entidades.Encuesta;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map.Encuesta
{
    public class EncuestaPreguntaMap
    {
        public EncuestaPreguntaMap(EntityTypeBuilder<RRHH_WEB_API._Entidades.Encuesta.EncuestaPregunta> builder)
        {
            builder.ToTable("encuesta_preguntas", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.EncuestaId).HasColumnName("id_encuesta").HasColumnType("int");
            builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");

            builder.HasOne(t => t.Encuesta).WithMany(p => p.Preguntas).HasForeignKey(u => u.EncuestaId);

        }
    }
}
