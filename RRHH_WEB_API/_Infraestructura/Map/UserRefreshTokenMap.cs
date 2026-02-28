using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class UserRefreshTokenMap
    {
        public UserRefreshTokenMap(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.ToTable("user_refresh_token", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.EmployeeId).HasColumnName("id_employee").HasColumnType("int").IsRequired();
            builder.Property(x => x.Token).HasColumnName("token").HasColumnType("varchar(255)").IsRequired();
            builder.Property(x => x.Expires).HasColumnName("expires").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Created).HasColumnName("created").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Revoked).HasColumnName("revoked").HasColumnType("datetime");

            builder.HasOne(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId);
        }
    }
}
