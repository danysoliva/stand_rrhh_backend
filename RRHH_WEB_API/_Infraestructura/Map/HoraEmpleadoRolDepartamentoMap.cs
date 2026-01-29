using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class HoraEmpleadoRolDepartamentoMap
    {
        public HoraEmpleadoRolDepartamentoMap(EntityTypeBuilder<HoraEmpleadosRolDepartamento> builder)
        {
            builder.ToTable("horas_empleados_roles_departamentos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar");
            builder.Property(x => x.Activo).HasColumnName("activo").HasColumnType("bit");

            

        }
    }
}
