
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class TipoVacacionMap
    {
        public TipoVacacionMap(EntityTypeBuilder<TipoVacacion> builder)
        {
            builder.ToTable("TipoVacaciones", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasColumnType("varchar").HasMaxLength(50);
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");
        }
    }
}
