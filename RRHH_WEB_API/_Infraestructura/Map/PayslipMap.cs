using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class PayslipMap
    {
        public PayslipMap(EntityTypeBuilder<Payslip> builder)
        {

            builder.ToTable("hr_payslip", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.StructId).HasColumnName("struct_id").HasColumnType("int");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar");
            builder.Property(x => x.Number).HasColumnName("number").HasColumnType("varchar");
            builder.Property(x => x.EmployeeId).HasColumnName("employee_id").HasColumnType("int");
            builder.Property(x => x.DateFrom).HasColumnName("date_from").HasColumnType("datetime");
            builder.Property(x => x.DateTo).HasColumnName("date_to").HasColumnType("datetime");
            builder.Property(x => x.State).HasColumnName("state").HasColumnType("varchar");
            builder.Property(x => x.CompanyId).HasColumnName("company_id").HasColumnType("int");
            builder.Property(x => x.Paid).HasColumnName("paid").HasColumnType("bit");
            builder.Property(x => x.ContractId).HasColumnName("contract_id").HasColumnType("int");
            builder.Property(x => x.CreditNote).HasColumnName("credit_note").HasColumnType("bit");
            builder.Property(x => x.PayslipRunId).HasColumnName("payslip_run_id").HasColumnType("int");
            builder.Property(x => x.CreateUID).HasColumnName("create_uid").HasColumnType("int");
            builder.Property(x => x.CreateDate).HasColumnName("create_date").HasColumnType("datetime");
            builder.Property(x => x.WriteUID).HasColumnName("write_uid").HasColumnType("int");
            builder.Property(x => x.WriteDate).HasColumnName("write_date").HasColumnType("datetime");

            builder.HasOne(x => x.Employee).WithMany(f => f.Paslips).HasForeignKey(t=>t.EmployeeId);
            builder.HasOne(x => x.PayslipRun).WithOne(f => f.Payslip);


        }
    }
}
