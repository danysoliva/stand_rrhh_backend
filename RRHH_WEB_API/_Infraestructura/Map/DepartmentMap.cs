using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class DepartmentMap
    {
        public DepartmentMap(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("hr_department", "Odoo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar");
            builder.Property(x => x.CompleteName).HasColumnName("complete_name").HasColumnType("varchar");
            builder.Property(x => x.CompanyId).HasColumnName("company_id").HasColumnType("int");
            builder.Property(x => x.ParentId).HasColumnName("parent_id").HasColumnType("int");
            builder.Property(x => x.ManagerId).HasColumnName("manager_id").HasColumnType("int");
            builder.Property(x => x.PaymentAccountId).HasColumnName("payment_account_id").HasColumnType("int");
            builder.Property(x => x.Active).HasColumnName("active").HasColumnType("bit");
            //builder.Property(x => x.ReasignacionCola).HasColumnName("reasignacion_cola").HasColumnType("int");

            //builder.HasOne(x => x.UserDelegation).WithOne(x => x.Employee);
        }
    }
}
