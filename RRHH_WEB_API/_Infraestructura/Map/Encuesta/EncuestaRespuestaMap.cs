using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using RRHH_WEB_API._Entidades.Encuesta;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map.Encuesta
{
    public class EncuestaRespuestaMap
    {
        public EncuestaRespuestaMap(EntityTypeBuilder<RRHH_WEB_API._Entidades.Encuesta.EncuestaRespuesta> builder)
        {
            builder.ToTable("encuesta_respuestas", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.EncuestaId).HasColumnName("id_encuesta").HasColumnType("int");
            builder.Property(x => x.PreguntaId).HasColumnName("id_pregunta").HasColumnType("int");
            builder.Property(x => x.OpcionId).HasColumnName("id_opcion").HasColumnType("int");
            builder.Property(x => x.EmployeeId).HasColumnName("employee_id").HasColumnType("int");
            //builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");

            //builder.HasOne(t => t.Encuesta).WithMany(p => p.Respuestas).HasForeignKey(u => u.EncuestaId);



        }
    }
}
