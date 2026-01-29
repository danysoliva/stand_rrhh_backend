using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class UserDelegationMap
    {
        public UserDelegationMap(EntityTypeBuilder<UserDelegation> builder)
        {
            builder.ToTable("user_delegation", "rrhh_web");
            builder.HasKey(x => x.EmployeeId);
            builder.Property(x => x.EmployeeId).HasColumnName("id_employee").HasColumnType("int");
            builder.Property(x => x.UserLevelId).HasColumnName("id_level").HasColumnType("int").IsRequired();
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit").IsRequired();

            builder.HasOne(x => x.Employee).WithOne(x => x.UserDelegation);
            builder.HasOne(x => x.UserLevel).WithMany(x => x.UserDelegations).HasForeignKey(x => x.UserLevelId);
        }
    }
}
