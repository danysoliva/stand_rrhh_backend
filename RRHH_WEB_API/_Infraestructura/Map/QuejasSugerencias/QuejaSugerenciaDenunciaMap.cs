using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades.QuejasSugerenciasDenuncias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map.QuejasSugerenciasDenuncias
{
    public class QuejaSugerenciaDenunciaMap
    {
        public QuejaSugerenciaDenunciaMap(EntityTypeBuilder<QuejaSugerenciaDenuncia> builder)
        {
            builder.ToTable("quejas_sugerencias_denuncias", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasColumnType("titulo").HasMaxLength(50).IsRequired();
            builder.Property(x => x.StateId).HasColumnName("id_state").HasColumnType("int");
            builder.Property(x => x.TypeId).HasColumnName("id_type").HasColumnType("int");
            builder.Property(x => x.CreateDate).HasColumnName("CreateDate").HasColumnType("datetime");
            builder.Property(x => x.LastModification).HasColumnName("last_modification").HasColumnType("datetime");

            builder.HasOne(x => x.State).WithMany(t => t.QuejasSugerenciasDenuncias).HasForeignKey(r=> r.StateId);
            builder.HasOne(x => x.Type).WithMany(t => t.QuejasSugerenciasDenuncias).HasForeignKey(r=> r.TypeId);


        }
    }
}
