using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class PayslipRunMap
    {
        public PayslipRunMap(EntityTypeBuilder<PayslipRun> builder)
        {

            builder.ToTable("hr_payslip_run", "Odoo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar");
            builder.Property(x => x.State).HasColumnName("state").HasColumnType("varchar");
            builder.Property(x => x.DateStart).HasColumnName("date_start").HasColumnType("datetime");
            builder.Property(x => x.DateEnd).HasColumnName("date_end").HasColumnType("datetime");
            builder.Property(x => x.CreditNote).HasColumnName("credit_note").HasColumnType("bit");
            builder.Property(x => x.CreateUID).HasColumnName("create_uid").HasColumnType("int");
            builder.Property(x => x.CreateDate).HasColumnName("create_date").HasColumnType("datetime");
            builder.Property(x => x.WriteUID).HasColumnName("write_uid").HasColumnType("int");
            builder.Property(x => x.WriteDate).HasColumnName("write_date").HasColumnType("datetime");
            builder.Property(x => x.NumerOfDays).HasColumnName("number_of_days").HasColumnType("int");
            builder.Property(x => x.PayRollTypeId).HasColumnName("payroll_type_id").HasColumnType("int");
            builder.Property(x => x.TotalInTransference).HasColumnName("total_in_transference").HasColumnType("decimal");
            builder.Property(x => x.Rate).HasColumnName("rate").HasColumnType("numeric");
            builder.Property(x => x.CurrencId).HasColumnName("currency_id").HasColumnType("int");
            builder.Property(x => x.PayslipNumber).HasColumnName("payslip_number").HasColumnType("varchar");
            builder.Property(x => x.Observation).HasColumnName("observation").HasColumnType("varchar");



        }
    }
}
