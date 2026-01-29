using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class HoraEmpleadoTrabajadaMap
    {
        public HoraEmpleadoTrabajadaMap(EntityTypeBuilder<HoraEmpleadoTrabajada> builder)
        {
            builder.ToTable("horas_empleado_trabajadas","dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.EmpleadoId).HasColumnName("id_empleado").HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(x => x.EmployeeId).HasColumnName("id_employee").HasColumnType("int");
            builder.Property(x => x.HoraI).HasColumnName("horai").HasColumnType("time");
            builder.Property(x => x.HoraF).HasColumnName("horaf").HasColumnType("time");
            builder.Property(x => x.Cantidad).HasColumnName("cantidad").HasColumnType("decimal");
            builder.Property(x => x.Fecha).HasColumnName("fecha").HasColumnType("date");
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");
            builder.Property(x => x.CantidadDe).HasColumnName("cantidade").HasColumnType("decimal");
            builder.Property(x => x.FechaI).HasColumnName("fechai").HasColumnType("datetime").HasDefaultValue(Convert.ToDateTime("1999-01-01"));
            builder.Property(x => x.FechaF).HasColumnName("fechaf").HasColumnType("datetime");
            builder.Property(x => x.Week).HasColumnName("week").HasColumnType("int");
            builder.Property(x => x.Tipo).HasColumnName("tipo").HasColumnType("int");

            builder.HasOne(x => x.Employee).WithMany(x => x.horaEmpleadoTrabajadas).HasForeignKey(x => x.EmployeeId);
            //builder.HasOne(x => x.HoraEmpleadoNombre).WithOne(x => x.horaEmpleadoTrabajada).HasForeignKey<HoraEmpleadoNombre>(t=>Convert.ToInt32( t.EmpleadoId));


        }
    }
}
