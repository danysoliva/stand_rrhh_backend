using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class PlazaVacanteMap
    {
        public PlazaVacanteMap(EntityTypeBuilder<PlazaVacante> builder)
        {
            builder.ToTable("plazas_vacantes", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Titulo).HasColumnName("titulo").HasColumnType("varchar");
            builder.Property(x => x.DepartmentId).HasColumnName("id_department").HasColumnType("int");
            builder.Property(x => x.Requisitos).HasColumnName("requisitos").HasColumnType("varchar");
            builder.Property(x => x.FechaCreacion).HasColumnName("fecha_creacion").HasColumnType("datetime");
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");
            //builder.Property(x => x.ReasignacionCola).HasColumnName("reasignacion_cola").HasColumnType("int");

            builder.HasOne(x => x.Departamento).WithMany(x => x.PlazaVacantes).HasForeignKey(d=>d.DepartmentId);

        }
    }
}
