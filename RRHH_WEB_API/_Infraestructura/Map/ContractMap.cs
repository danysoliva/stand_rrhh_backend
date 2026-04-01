using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class ContractMap
    {
        public ContractMap(EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable("hr_contrato", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar");
            builder.Property(x => x.Active).HasColumnName("active").HasColumnType("bit");
            builder.Property(x => x.EmployeeId).HasColumnName("employee_id").HasColumnType("int");
            builder.Property(x => x.DepartmentId).HasColumnName("department_id").HasColumnType("int");
            builder.Property(x => x.JobId).HasColumnName("puesto_id").HasColumnType("int");
            builder.Property(x => x.TypeId).HasColumnName("id_tipo_contrato").HasColumnType("int");
            builder.Property(x => x.DateStart).HasColumnName("fecha_inicio").HasColumnType("date");
            builder.Property(x => x.DateEnd).HasColumnName("fecha_fin").HasColumnType("date");
            builder.Property(x => x.Trial_DateEnd).HasColumnName("temporal_fecha_fin").HasColumnType("date");
            builder.Property(x => x.ResourceCalendarId).HasColumnName("recursos_calendario_id").HasColumnType("int");
            builder.Property(x => x.Wage).HasColumnName("salario").HasColumnType("decimal");
            builder.Property(x => x.State).HasColumnName("state").HasColumnType("varchar");
        }
    }
}
