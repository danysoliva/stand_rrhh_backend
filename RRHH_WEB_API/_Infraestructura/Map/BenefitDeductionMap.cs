using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class BenefitDeductionMap
    {
        public BenefitDeductionMap(EntityTypeBuilder<BenefitDeduction> builder)
        {
            builder.ToTable("hr_benefits_deductions", "Odoo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.ContractId).HasColumnName("contract_id").HasColumnType("int");
            builder.Property(x => x.ConceptId).HasColumnName("concept_id").HasColumnType("int");
            builder.Property(x => x.Active).HasColumnName("active").HasColumnType("bit");
            builder.Property(x => x.Value).HasColumnName("value").HasColumnType("decimal");
            builder.Property(x => x.Type).HasColumnName("type").HasColumnType("varchar");


            builder.HasOne(x => x.Contract).WithMany(x => x.BenefitDeductions).HasForeignKey(r=>r.ContractId);
            builder.HasOne(x => x.Concept).WithMany(x => x.BenefitDeductions).HasForeignKey(r=>r.ConceptId);
        }
    }
}
