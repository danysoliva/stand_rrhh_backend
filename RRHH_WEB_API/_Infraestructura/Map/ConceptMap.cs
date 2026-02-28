using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class ConceptMap
    {
        public ConceptMap(EntityTypeBuilder<Concept> builder)
        {
            builder.ToTable("hr_concepts", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Code).HasColumnName("code").HasColumnType("varchar");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar");
            builder.Property(x => x.Note).HasColumnName("note").HasColumnType("varchar");
           


            //builder.HasOne(x => x.UserDelegation).WithOne(x => x.Employee);
        }
    }
}
