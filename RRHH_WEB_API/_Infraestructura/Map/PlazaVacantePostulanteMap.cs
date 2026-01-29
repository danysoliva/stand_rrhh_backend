using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Infraestructura.Map
{
    public class PlazaVacantePostulanteMap
    {
        public PlazaVacantePostulanteMap(EntityTypeBuilder<PlazaVacantePostulante> builder)
        {
            builder.ToTable("plazas_vacantes_postulantes", "rrhh_web");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(x => x.PlazaVacanteId).HasColumnName("id_plaza_vacante").HasColumnType("int");
            builder.Property(x => x.EmpleadoId).HasColumnName("id_empleado").HasColumnType("int");
            builder.Property(x => x.NombrePostulante).HasColumnName("nombre_postulante").HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.Correo).HasColumnName("correo").HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.Telefono).HasColumnName("telefono").HasColumnType("varchar").HasMaxLength(10);
            builder.Property(x => x.EsRecomendado).HasColumnName("es_recomendao").HasColumnType("bit");
            builder.Property(x => x.Enable).HasColumnName("enable").HasColumnType("bit");

            builder.HasOne(x => x.PlazaVacante).WithMany(x => x.PlazaVacantePostulantes).HasForeignKey(p => p.PlazaVacanteId);



        }


    }
}
