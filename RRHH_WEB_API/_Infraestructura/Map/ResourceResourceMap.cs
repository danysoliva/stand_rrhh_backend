using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class ResourceResourceMap
    {
        public ResourceResourceMap(EntityTypeBuilder<ResourceResource> builder)
        {
            builder.ToTable("resource_resource", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar");
            builder.Property(x => x.Active).HasColumnName("active").HasColumnType("bit");
            builder.Property(x => x.CompanyId).HasColumnName("company_id").HasColumnType("int");
            builder.Property(x => x.ResourceType).HasColumnName("resource_type").HasColumnType("varchar");
            builder.Property(x => x.CalendarId).HasColumnName("calendar_id").HasColumnType("int");
            builder.Property(x => x.TZ).HasColumnName("tz").HasColumnType("varchar");
            builder.Property(x => x.CrateUID).HasColumnName("create_uid").HasColumnType("int");
            builder.Property(x => x.CrateDate).HasColumnName("create_date").HasColumnType("datetime");
            builder.Property(x => x.WriteUID).HasColumnName("write_uid").HasColumnType("int");
            builder.Property(x => x.WriteDate).HasColumnName("write_date").HasColumnType("datetime");
        }
    }
}
