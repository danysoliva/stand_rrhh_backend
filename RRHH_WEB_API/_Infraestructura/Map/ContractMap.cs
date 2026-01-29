using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class ContractMap
    {
        public ContractMap(EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable("hr_contract", "Odoo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar");
            builder.Property(x => x.Active).HasColumnName("active").HasColumnType("bit");
            builder.Property(x => x.EmployeeId).HasColumnName("employee_id").HasColumnType("int");
            builder.Property(x => x.DepartmentId).HasColumnName("department_id").HasColumnType("int");
            builder.Property(x => x.JobId).HasColumnName("job_id").HasColumnType("int");
            builder.Property(x => x.TypeId).HasColumnName("type_id").HasColumnType("int");
            builder.Property(x => x.DateStart).HasColumnName("date_start").HasColumnType("date");
            builder.Property(x => x.DateEnd).HasColumnName("date_end").HasColumnType("date");
            builder.Property(x => x.Trial_DateEnd).HasColumnName("trial_date_end").HasColumnType("date");
            builder.Property(x => x.ResourceCalendarId).HasColumnName("resource_calendar_id").HasColumnType("int");
            builder.Property(x => x.Wage).HasColumnName("wage").HasColumnType("decimal");
            builder.Property(x => x.State).HasColumnName("state").HasColumnType("varchar");
        }
    }
}
