using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades.QuejasSugerenciasDenuncias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map.QuejasSugerenciasDenuncias
{
    public class QuejaSugerenciaDenunciaStateMap
    {
        public QuejaSugerenciaDenunciaStateMap(EntityTypeBuilder<QuejaSugerenciaDenunciaState> builder)
        {
            builder.ToTable("quejas_sugerencias_denuncias_state", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasColumnType("titulo").HasMaxLength(50);
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");

        }
    }
}
