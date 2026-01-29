using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class JournalMap
    {
        public JournalMap(EntityTypeBuilder<Journal> builder)
        {

            builder.ToTable("hr_journal", "Odoo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasColumnType("varchar");
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");

            //builder.HasOne(x => x.UserDelegation).WithOne(x => x.Employee);
        }
    }
}
