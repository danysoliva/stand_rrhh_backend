using Microsoft.EntityFrameworkCore;
using RRHH_WEB_API._Entidades;
using RRHH_WEB_API._Entidades.Encuesta;
using RRHH_WEB_API._Entidades.Encuestas;
using RRHH_WEB_API._Entidades.QuejasSugerenciasDenuncias;
using RRHH_WEB_API._Infraestructura.Map;
using RRHH_WEB_API._Infraestructura.Map.Encuesta;
using RRHH_WEB_API._Infraestructura.Map.QuejasSugerenciasDenuncias;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RRHH_WEB_API._Infraestructura.RRHH_DBContext;

namespace RRHH_WEB_API._Infraestructura
{
    public class ACS_DBContext : DbContext
    {
        public ACS_DBContext(DbContextOptions<ACS_DBContext> options) : base(options)
        {
        }

        public DbSet<AutorizacionDeduccionPlanilla> AutorizacionDeduccionPlanilla { get; set; }
        public DbSet<AutorizacionDeduccionPlanillaEstado> AutorizacionDeduccionPlanillaEstado { get; set; }
        public DbSet<HoraEmpleadoTrabajada> HoraEmpleadoTrabajada { get; set; }
        public DbSet<HoraEmpleadosRolDepartamento> HorasEmpleadosRolesDepartamento { get; set; }
        public DbSet<HoraEmpleadoNombre> HoraEmpleadoNombre { get; set; }
        public DbSet<HorasTrabajadasEmpleado> HorasTrabajadasEmpleados { get; set; }
        public DbSet<UserDelegation> UserDelegation { get; set; }
        public DbSet<UserLevel> UserLevel { get; set; }
        public DbSet<RepositoryImage> RepositoryImage { get; set; }
        public DbSet<RepositoryDocument> RepositoryDocument { get; set; }
        public DbSet<RequestConstancia> RequestConstancia { get; set; }
        public DbSet<RequestType> RequestType { get; set; }
        public DbSet<RequestState> RequestState { get; set; }
        public DbSet<RequestVacacion> RequestVacacion { get; set; }
        public DbSet<RequestVacacionTracking> RequestVacacionTracking { get; set; }
        public DbSet<PlazaVacantePostulante> PlazaVacantePostulantes { get; set; }
        public DbSet<ParametrosGenerales> ParametrosGenerales { get; set; }
        public DbSet<PlazaVacante> PlazaVacantes { get; set; }
        public DbSet<PeriodoVacacion> PeriodoVacacion { get; set; }
        public DbSet<Feriado> Feriado { get; set; }
        public DbSet<PlazaVacanteAdjunto> PlazaVacanteAdjuntos { get; set; }
        public DbSet<EncuestaH> Encuesta { get; set; }
        public DbSet<EncuestaEstado> EncuestaEstado { get; set; }
        public DbSet<EncuestaOpcion> EncuestaOpciones { get; set; }
        public DbSet<EncuestaPregunta> EncuestaPreguntas { get; set; }
        public DbSet<EncuestaRespuesta> EncuestaRespuestas { get; set; }
        public DbSet<QuejaSugerenciaDenuncia> QuejaSugerenciaDenuncia { get; set; }
        public DbSet<QuejaSugerenciaDenunciaState> QuejaSugerenciaDenunciaState { get; set; }
        public DbSet<QuejaSugerenciaDenunciaType> QuejaSugerenciaDenunciaType { get; set; }
        public DbSet<RequestItem> RequestItem { get; set; }
        public DbSet<RequestConstanciaItem> RequestConstanciaItem { get; set; }
        public DbSet<TipoVacacion> TipoVacaciones { get; set; }
        public DbSet<RepositoryGroup> RepositoryGroups { get; set; }
        public DbSet<SolicitudVacacionEmpleadoDto> SolicitudVacacionEmpleado { get; set; }


        public class HorasTrabajadasEmpleado
        {
            [Key]
            public int Serial { get; set; }
            public string? Code { get; set; }
            public int? EmpleadoId { get; set; }
            public string? EmployeeName { get; set; }
            public decimal? NormalHour { get; set; }
            public decimal? ExtraHours { get; set; }
            public DateTime? FechaI { get; set; }
            public DateTime? FechaF { get; set; }
            public DateTime Fecha { get; set; }
            public string? Departamento { get; set; }
            public int? Semana { get; set; }
        }

        public class SolicitudVacacionEmpleadoDto
        {
            //[Key]
            [NotMapped]
            public int? SolicitudId { get; set; }
            public int? EmployeeId { get; set; }
            public string EmployeeName { get; set; }
            public string EmployeeBarCode { get; set; }
            public string EmployeeJobName { get; set; }
            public string EmployeeDepartmentName { get; set; }
 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HoraEmpleadoTrabajada>().ToTable("horas_empleado_trabajadas", "dbo");

            new RequestStateMap(modelBuilder.Entity<RequestState>());
            new RequestTypeMap(modelBuilder.Entity<RequestType>());
            new RequestVacacionMap(modelBuilder.Entity<RequestVacacion>());
            new RequestVacacionTrackingMap(modelBuilder.Entity<RequestVacacionTracking>());

            new AutorizacionDeduccionPlanillaMap(modelBuilder.Entity<AutorizacionDeduccionPlanilla>());
            //modelBuilder.Entity<AutorizacionDeduccionPlanilla>().ToTable("AutorizacionDeduccionPlanilla", "rrhh_web");

            new AutorizacionDeduccionPlanillaEstadoMap(modelBuilder.Entity<AutorizacionDeduccionPlanillaEstado>());
            //modelBuilder.Entity<AutorizacionDeduccionPlanillaEstado>().ToTable("AutorizacionDeduccionPlanilla_Estado", "rrhh_web");

            new ParametrosGeneralesMap(modelBuilder.Entity<ParametrosGenerales>());
            new HoraEmpleadoRolDepartamentoMap(modelBuilder.Entity<HoraEmpleadosRolDepartamento>());
            new HoraEmpleadoNombreMap(modelBuilder.Entity<HoraEmpleadoNombre>());
            new HoraEmpleadoTrabajadaMap(modelBuilder.Entity<HoraEmpleadoTrabajada>());
            new PlazaVacanteMap(modelBuilder.Entity<PlazaVacante>());
            new PlazaVacanteAdjuntoMap(modelBuilder.Entity<PlazaVacanteAdjunto>());
            new PlazaVacantePostulanteMap(modelBuilder.Entity<PlazaVacantePostulante>());
            new EncuestaHMap(modelBuilder.Entity<EncuestaH>());
            new EncuestaEstadoMap(modelBuilder.Entity<EncuestaEstado>());
            new EncuestaOpcionesMap(modelBuilder.Entity<EncuestaOpcion>());
            new EncuestaPreguntaMap(modelBuilder.Entity<EncuestaPregunta>());
            new EncuestaRespuestaMap(modelBuilder.Entity<EncuestaRespuesta>());
            new QuejaSugerenciaDenunciaMap(modelBuilder.Entity<QuejaSugerenciaDenuncia>());
            new QuejaSugerenciaDenunciaStateMap(modelBuilder.Entity<QuejaSugerenciaDenunciaState>());
            new QuejaSugerenciaDenunciaTypeMap(modelBuilder.Entity<QuejaSugerenciaDenunciaType>());
            new RequestItemMap(modelBuilder.Entity<RequestItem>());
            new RequestConstanciaItemMap(modelBuilder.Entity<RequestConstanciaItem>());
            new PeriodoVacacionMap(modelBuilder.Entity<PeriodoVacacion>());
            new TipoVacacionMap(modelBuilder.Entity<TipoVacacion>());

            //new RepositoryGroupMap(modelBuilder.Entity<RepositoryGroup>());
            modelBuilder.Entity<RepositoryGroup>().ToTable("repository_group", "rrhh_web");

            //new RepositoryDocumentMap(modelBuilder.Entity<RepositoryDocument>());
            modelBuilder.Entity<RepositoryDocument>().ToTable("repository_document", "rrhh_web");


            new RepositoryImageMap(modelBuilder.Entity<RepositoryImage>());
            new RequestConstanciaMap(modelBuilder.Entity<RequestConstancia>());
            new UserDelegationMap(modelBuilder.Entity<UserDelegation>());
            new FeriadoMap(modelBuilder.Entity<Feriado>());
            new UserLevelMap(modelBuilder.Entity<UserLevel>());

            modelBuilder.Entity<SolicitudVacacionEmpleadoDto>().HasNoKey();
            //modelBuilder.Entity<RequestConstancia>().ToTable("request_constancia", "rrhh_web");

        }

    }
}
