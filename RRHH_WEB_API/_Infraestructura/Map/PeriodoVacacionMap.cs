using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class PeriodoVacacionMap
    {
        public PeriodoVacacionMap(EntityTypeBuilder<PeriodoVacacion> builder)
        {

            builder.ToTable("hr_saldo_vacaciones", "dbo");
            builder.HasKey(x => new { x.EmployeeId, x.Year});
            builder.Property(x => x.EmployeeId).HasColumnName("id_employee").HasColumnType("int");
            builder.Property(x => x.Year).HasColumnName("year").HasColumnType("int");
            builder.Property(x => x.Days).HasColumnName("days").HasColumnType("int");

            builder.HasOne(x => x.Employee).WithMany(f => f.PeriodosVacacion).HasForeignKey(t => t.EmployeeId);
        }
    }
}
