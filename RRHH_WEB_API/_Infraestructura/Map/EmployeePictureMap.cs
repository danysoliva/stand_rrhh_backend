using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class EmployeePictureMap
    {
        public EmployeePictureMap(EntityTypeBuilder<EmployeePicture> builder)
        {
            builder.ToTable("hr_employee_picture", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.IdEmployee).HasColumnName("id_employee").HasColumnType("int");
            builder.Property(x => x.EmployeeCode).HasColumnName("employee_code").HasColumnType("varchar");
            builder.Property(x => x.Path).HasColumnName("path_").HasColumnType("varchar");
            builder.Property(x => x.FileName).HasColumnName("file_name_").HasColumnType("varchar");
            builder.Property(x => x.CreateDate).HasColumnName("create_date").HasColumnType("datetime");
            builder.Property(x => x.CreateUid).HasColumnName("create_uid").HasColumnType("int");
            builder.Property(x => x.Active).HasColumnName("active").HasColumnType("bit");
        }
    }
}
