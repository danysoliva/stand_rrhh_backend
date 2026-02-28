using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class EmployeeMap
    {
        public EmployeeMap(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("hr_employee", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar");
            builder.Property(x => x.Gender).HasColumnName("gender").HasColumnType("varchar");
            builder.Property(x => x.WorkEmail).HasColumnName("work_email").HasColumnType("varchar");
            builder.Property(x => x.MobilePhone).HasColumnName("mobile_phone").HasColumnType("varchar");
            builder.Property(x => x.ShirtSize).HasColumnName("shirt_size").HasColumnType("varchar");
            builder.Property(x => x.PantSize).HasColumnName("pant_size").HasColumnType("varchar");
            builder.Property(x => x.ShoeSize).HasColumnName("shoe_size").HasColumnType("varchar");
            builder.Property(x => x.Weigth).HasColumnName("weight").HasColumnType("int");
            builder.Property(x => x.Height).HasColumnName("height").HasColumnType("int");
            builder.Property(x => x.JobId).HasColumnName("job_id").HasColumnType("int");
            builder.Property(x => x.JournalId).HasColumnName("x_id_journal").HasColumnType("int");
            builder.Property(x => x.BirthDay).HasColumnName("birthday").HasColumnType("date");
            builder.Property(x => x.IdentificationId).HasColumnName("identification_id").HasColumnType("varchar");
            builder.Property(x => x.BarCode).HasColumnName("barcode").HasColumnType("varchar");
            builder.Property(x => x.Pin).HasColumnName("pin").HasColumnType("varbinary");
            builder.Property(x => x.DepartmentId).HasColumnName("department_id").HasColumnType("int");
            builder.Property(x => x.ParentId).HasColumnName("parent_id").HasColumnType("int");
            //builder.Property(x => x.Image).HasColumnName("x_image").HasColumnType("varbinary");
            builder.Property(x => x.ResourceId).HasColumnName("resource_id").HasColumnType("int");
            builder.Property(x => x.Active).HasColumnName("active").HasColumnType("bit");

            builder.HasOne(x => x.UserDelegation).WithOne(x => x.Employee);
            builder.HasOne(x => x.Department).WithMany(x => x.Empleados).HasForeignKey(x => x.DepartmentId);
            builder.HasOne(x => x.Job).WithMany(x => x.Empleados).HasForeignKey(x => x.JobId);
            builder.HasOne(x => x.Journal).WithMany(x => x.Empleados).HasForeignKey(x => x.JournalId);
            builder.HasOne(x => x.Contract).WithOne(x => x.Employee);
            //builder.HasOne(x => x.SolicitudConstancia).WithMany(x => x.Empleados).HasForeignKey(x => x.JobId); ;
            builder.HasOne(x => x.Resource).WithOne(r => r.Empleado);
        }
    }
}
