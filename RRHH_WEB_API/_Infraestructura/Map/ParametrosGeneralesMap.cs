using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class ParametrosGeneralesMap
    {
        public ParametrosGeneralesMap(EntityTypeBuilder<ParametrosGenerales> builder)
        {
            builder.ToTable("parametros_generales", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasColumnType("varchar");
            builder.Property(x => x.Valor).HasColumnName("valor").HasColumnType("varchar");
        }
    }
}
