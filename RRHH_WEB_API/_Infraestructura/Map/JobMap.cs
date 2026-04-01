using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class JobMap
    {
        public JobMap(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("hr_job", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar");
            builder.Property(x => x.CompanyId).HasColumnName("id_company").HasColumnType("int");
            builder.Property(x => x.DepartmentId).HasColumnName("id_departamento").HasColumnType("int");

            //builder.HasOne(x => x.UserDelegation).WithOne(x => x.Employee);
        }
    }
}
