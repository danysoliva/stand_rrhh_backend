using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class AutorizacionDeduccionPlanillaMap
    {
        public AutorizacionDeduccionPlanillaMap(EntityTypeBuilder<AutorizacionDeduccionPlanilla> builder)
        {

            builder.ToTable("AutorizacionDeduccionPlanilla", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.EmployeeId).HasColumnName("employee_id").HasColumnType("int");
            builder.Property(x => x.UsuarioCreacionId).HasColumnName("usuario_creacion_id").HasColumnType("int");
            builder.Property(x => x.EstadoId).HasColumnName("estado_id").HasColumnType("int");
            builder.Property(x => x.Monto).HasColumnName("monto").HasColumnType("decimal");
            builder.Property(x => x.FechaDeduccion).HasColumnName("fecha_deduccion").HasColumnType("date");
            builder.Property(x => x.FechaCreacion).HasColumnName("fecha_creacion").HasColumnType("date");
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");
            builder.Property(x => x.Concepto).HasColumnName("concepto").HasColumnType("varchar").HasMaxLength(200);
            builder.Property(x => x.Currency).HasColumnName("currency").HasColumnType("varchar").HasMaxLength(5);
            builder.Property(x => x.TasaCambio).HasColumnName("tasa_cambio").HasColumnType("decimal").IsRequired();

            builder.HasOne(x => x.Empleado).WithMany(x => x.Deducciones).HasForeignKey(g=> g.EmployeeId);
            builder.HasOne(x => x.EstadoDeduccionPorPlanilla).WithMany(x => x.DeduccionesPorPlanilla).HasForeignKey(g=> g.EstadoId);
        }
    }
}
