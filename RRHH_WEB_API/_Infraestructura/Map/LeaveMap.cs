using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class LeaveMap
    {
        public LeaveMap(EntityTypeBuilder<Leave> builder)
        {

            builder.ToTable("hr_leave", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.State).HasColumnName("state").HasColumnType("varchar");
            builder.Property(x => x.EmployeeId).HasColumnName("employee_id").HasColumnType("int");
            builder.Property(x => x.DepartmentId).HasColumnName("department_id").HasColumnType("int");
            builder.Property(x => x.DateFrom).HasColumnName("date_from").HasColumnType("datetime");
            builder.Property(x => x.DateTo).HasColumnName("date_to").HasColumnType("datetime");
            builder.Property(x => x.NumberOfDays).HasColumnName("number_of_days").HasColumnType("float");
            builder.Property(x => x.HolidayStatusId).HasColumnName("holiday_status_id").HasColumnType("int");
            builder.Property(x => x.CreateDate).HasColumnName("create_date").HasColumnType("datetime");

            builder.HasOne(x => x.Employee).WithMany(f => f.Leaves).HasForeignKey(t => t.EmployeeId);
        }
    }
}
