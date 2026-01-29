using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using RRHH_WEB_API._Entidades.Encuesta;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map.Encuesta
{
    public class EncuestaOpcionesMap
    {
        public EncuestaOpcionesMap(EntityTypeBuilder<RRHH_WEB_API._Entidades.Encuesta.EncuestaOpcion> builder)
        {
            builder.ToTable("encuesta_opciones", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.EncuestaId).HasColumnName("id_encuesta").HasColumnType("int");
            builder.Property(x => x.PreguntaId).HasColumnName("id_pregunta").HasColumnType("int");
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");

            //builder.HasOne(t => t.Encuesta).WithMany(p => p.Opciones).HasForeignKey(u => u.EncuestaId);
            //builder.HasOne(t => t.Pregunta).WithMany(p => p.Opciones).HasForeignKey(u => u.PreguntaId);


        }
    }
}
