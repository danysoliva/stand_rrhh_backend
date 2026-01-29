using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class PlazaVacanteAdjuntoMap
    {
        public PlazaVacanteAdjuntoMap(EntityTypeBuilder<PlazaVacanteAdjunto> builder)
        {
            builder.ToTable("plazas_vacantes_adjuntos", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.PlazaVacantePostulanteId).HasColumnName("id_postulante").HasColumnType("int");
            builder.Property(x => x.Host).HasColumnName("host").HasColumnType("varchar").HasMaxLength(50);
            builder.Property(x => x.Path).HasColumnName("path").HasColumnType("varchar");
            builder.Property(x => x.ReferenceFileName).HasColumnName("reference_file_name").HasColumnType("varchar");
            builder.Property(x => x.FileName).HasColumnName("file_name").HasColumnType("varchar");
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");
            //builder.Property(x => x.ReasignacionCola).HasColumnName("reasignacion_cola").HasColumnType("int");

            builder.HasOne(x => x.PlazaVacantePostulante).WithMany(x => x.Adjuntos).HasForeignKey(p => p.PlazaVacantePostulanteId);


        }
    }
}
