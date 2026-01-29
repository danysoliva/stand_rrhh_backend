using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class PayslipLineMap
    {
        public PayslipLineMap(EntityTypeBuilder<PayslipLine> builder)
        {
            
            builder.ToTable("hr_payslip_line", "Odoo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint");
            builder.Property(x => x.PayslipId).HasColumnName("slip_id").HasColumnType("int");
            builder.Property(x => x.SalaryRuleId).HasColumnName("salary_rule_id").HasColumnType("int");
            builder.Property(x => x.EmployeId).HasColumnName("employee_id").HasColumnType("int");
            builder.Property(x => x.ContractId).HasColumnName("contract_id").HasColumnType("int");
            builder.Property(x => x.Rate).HasColumnName("rate").HasColumnType("numeric");
            builder.Property(x => x.Amount).HasColumnName("amount").HasColumnType("numeric");
            builder.Property(x => x.Quantity).HasColumnName("quantity").HasColumnType("numeric");
            builder.Property(x => x.Total).HasColumnName("total").HasColumnType("numeric");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar");
            builder.Property(x => x.Code).HasColumnName("code").HasColumnType("varchar");
            builder.Property(x => x.CategoryId).HasColumnName("category_id").HasColumnType("int");
            builder.Property(x => x.Active).HasColumnName("active").HasColumnType("bit");
            builder.Property(x => x.AppearsOnPayslip).HasColumnName("appears_on_payslip").HasColumnType("bit");
            builder.Property(x => x.Note).HasColumnName("note").HasColumnType("varchar");
            builder.Property(x => x.CreateUID).HasColumnName("create_uid").HasColumnType("int");
            builder.Property(x => x.CreateDate).HasColumnName("create_date").HasColumnType("datetime");            
            builder.Property(x => x.GroupById).HasColumnName("group_by_id").HasColumnType("int");
            builder.Property(x => x.AmountIsrDeductible).HasColumnName("amount_isr_deductible").HasColumnType("int");         
            builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int");
            builder.Property(x => x.AnalyticAccountId).HasColumnName("analytic_account_id").HasColumnType("int");



            builder.HasOne(x => x.Payslip).WithOne(x => x.PayslipLine);


        }
    }
}
