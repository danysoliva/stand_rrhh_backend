using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class HoraEmpleadoNombreMap
    {
        public HoraEmpleadoNombreMap(EntityTypeBuilder<HoraEmpleadoNombre> builder)
        {
            builder.ToTable("horas_empleados_nombres","dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Codigo).HasColumnName("codigo").HasColumnType("varchar").HasMaxLength(6);
            builder.Property(x => x.Nombre).HasColumnName("nombre").HasColumnType("varchar").HasMaxLength(200);
            builder.Property(x => x.DepartamentoId).HasColumnName("id_departamento").HasColumnType("int");
            builder.Property(x => x.GrupoId).HasColumnName("id_grupo").HasColumnType("int");
            builder.Property(x => x.EmpleadoId).HasColumnName("id_empleado").HasColumnType("nvarchar").HasMaxLength(6);
            builder.Property(x => x.WorkEmail).HasColumnName("work_email").HasColumnType("varchar").HasMaxLength(80);
            builder.Property(x => x.Marking).HasColumnName("marking").HasColumnType("bit");
            builder.Property(x => x.FechaC).HasColumnName("fechac").HasColumnType("datetime");
            builder.Property(x => x.Active).HasColumnName("active").HasColumnType("bit");
            builder.Property(x => x.XHourIn).HasColumnName("x_hour_in").HasColumnType("datetime");
            builder.Property(x => x.XHourOut).HasColumnName("x_hour_out").HasColumnType("datetime");
            builder.Property(x => x.RollId).HasColumnName("roll_id").HasColumnType("int");


            builder.HasOne(x => x.HoraEmpleadoDepartamento).WithMany(x => x.horaEmpleadoNombres).HasForeignKey(t=>t.RollId);

        }
    }
}
