using Microsoft.EntityFrameworkCore;
using RRHH_WEB_API._Entidades;
using RRHH_WEB_API._Entidades.Encuesta;
using RRHH_WEB_API._Entidades.Encuestas;
using RRHH_WEB_API._Entidades.QuejasSugerenciasDenuncias;
using RRHH_WEB_API._Infraestructura.Map;
using RRHH_WEB_API._Infraestructura.Map.Encuesta;
using RRHH_WEB_API._Infraestructura.Map.QuejasSugerenciasDenuncias;
using System.ComponentModel.DataAnnotations;

namespace RRHH_WEB_API._Infraestructura
{
    public class RRHH_Web_DBContext : DbContext
    {
        public RRHH_Web_DBContext(DbContextOptions<RRHH_Web_DBContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<UserDelegation> UserDelegation { get; set; }
        public DbSet<UserLevel> UserLevel { get; set; }
        public DbSet<RepositoryImage> RepositoryImage { get; set; }
        public DbSet<RepositoryDocument> RepositoryDocument { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<RequestConstancia> RequestConstancia { get; set; }
        public DbSet<RequestType> RequestType { get; set; }
        public DbSet<RequestState> RequestState { get; set; }
        public DbSet<RequestVacacion> RequestVacacion { get; set; }
        public DbSet<RequestVacacionTracking> RequestVacacionTracking { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<PayslipRun> PayslipRun { get; set; }
        public DbSet<PayslipLine> PayslipLine { get; set; }
        public DbSet<Payslip> Payslip { get; set; }
        public DbSet<Journal> Journal { get; set; }
        public DbSet<AutorizacionDeduccionPlanilla> AutorizacionDeduccionPlanilla { get; set; }
        public DbSet<AutorizacionDeduccionPlanillaEstado> AutorizacionDeduccionPlanillaEstado { get; set; }
        public DbSet<ParametrosGenerales> ParametrosGenerales { get; set; }
        public DbSet<HoraEmpleadosRolDepartamento> HorasEmpleadosRolesDepartamento { get; set; }
        public DbSet<HoraEmpleadoTrabajada> HoraEmpleadoTrabajada { get; set; }
        public DbSet<HoraEmpleadoNombre> HoraEmpleadoNombre { get; set; }
        public DbSet<BenefitDeduction>  BenefitDeduction { get; set; }
        public DbSet<PlazaVacante>  PlazaVacantes { get; set; }
        public DbSet<Concept> Concepts { get; set; }
        public DbSet<PlazaVacantePostulante>  PlazaVacantePostulantes { get; set; }
        public DbSet<PlazaVacanteAdjunto>  PlazaVacanteAdjuntos { get; set; }
        public DbSet<EncuestaH> Encuesta { get; set; }
        public DbSet<EncuestaEstado> EncuestaEstado { get; set; }
        public DbSet<EncuestaOpcion> EncuestaOpciones { get; set; }
        public DbSet<EncuestaPregunta> EncuestaPreguntas { get; set; }
        public DbSet<EncuestaRespuesta> EncuestaRespuestas { get; set; }
        public DbSet<QuejaSugerenciaDenuncia> QuejaSugerenciaDenuncia { get; set; }
        public DbSet<QuejaSugerenciaDenunciaState> QuejaSugerenciaDenunciaState { get; set; }
        public DbSet<QuejaSugerenciaDenunciaType> QuejaSugerenciaDenunciaType { get; set; }
        public DbSet<Feriado> Feriado { get; set; }
        public DbSet<RequestItem> RequestItem { get; set; }
        public DbSet<RequestConstanciaItem> RequestConstanciaItem { get; set; }
        public DbSet<ResourceResource> ResourceResource { get; set; }
        public DbSet<Leave> Leave { get; set; }
        public DbSet<PeriodoVacacion> PeriodoVacacion { get; set; }
        public DbSet<Deducciones> Deductions { get; set; }
        public DbSet<BenefitsVoucher> BeneficioVoucher { get; set; }
        public DbSet<DeductionVoucher> DeduccionesVoucher { get; set; }
        public DbSet<TipoVacacion> TipoVacaciones { get; set; }
        public DbSet<RepositoryGroup> RepositoryGroups { get; set; }

        public class Deducciones
        {
            [Key]
            public int Id { get; set; }
            public string Nombre { get; set; }
            public decimal Monto { get; set; }
        }


        public class BenefitsVoucher
        {
            //[Key]
            public long Id { get; set; }
            public string Name { get; set; }
            public decimal Monto { get; set; }
            public string Code { get; set; }
            public string CurrencyName { get; set; }
        }


        public class DeductionVoucher
        {
            [Key]
            public long Id { get; set; }
            public string Name { get; set; }
            public decimal Monto { get; set; }
            public string Code { get; set; }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new EmployeeMap(modelBuilder.Entity<Employee>());
            new UserDelegationMap(modelBuilder.Entity<UserDelegation>());
            new UserLevelMap(modelBuilder.Entity<UserLevel>());
            new RepositoryImageMap(modelBuilder.Entity<RepositoryImage>());
            new RepositoryDocumentMap(modelBuilder.Entity<RepositoryDocument>());
            new DepartmentMap(modelBuilder.Entity<Department>());
            new JobMap(modelBuilder.Entity<Job>());
            new RequestConstanciaMap(modelBuilder.Entity<RequestConstancia>());
            new RequestStateMap(modelBuilder.Entity<RequestState>());
            new RequestTypeMap(modelBuilder.Entity<RequestType>());
            new RequestVacacionMap(modelBuilder.Entity<RequestVacacion>());
            new RequestVacacionTrackingMap(modelBuilder.Entity<RequestVacacionTracking>());
            new ContractMap(modelBuilder.Entity<Contract>());
            new JournalMap(modelBuilder.Entity<Journal>());
            new PayslipRunMap(modelBuilder.Entity<PayslipRun>());
            new PayslipLineMap(modelBuilder.Entity<PayslipLine>());
            new PayslipMap(modelBuilder.Entity<Payslip>());
            new AutorizacionDeduccionPlanillaMap(modelBuilder.Entity<AutorizacionDeduccionPlanilla>());
            new AutorizacionDeduccionPlanillaEstadoMap(modelBuilder.Entity<AutorizacionDeduccionPlanillaEstado>());
            new ParametrosGeneralesMap(modelBuilder.Entity<ParametrosGenerales>());
            new HoraEmpleadoRolDepartamentoMap(modelBuilder.Entity<HoraEmpleadosRolDepartamento>());
            new HoraEmpleadoNombreMap(modelBuilder.Entity<HoraEmpleadoNombre>());
            new HoraEmpleadoTrabajadaMap(modelBuilder.Entity<HoraEmpleadoTrabajada>());
            new BenefitDeductionMap(modelBuilder.Entity<BenefitDeduction>());
            new ConceptMap(modelBuilder.Entity<Concept>());
            new PlazaVacanteMap (modelBuilder.Entity<PlazaVacante>());
            new PlazaVacanteAdjuntoMap (modelBuilder.Entity<PlazaVacanteAdjunto>());
            new PlazaVacantePostulanteMap (modelBuilder.Entity<PlazaVacantePostulante>());
            new EncuestaHMap (modelBuilder.Entity<EncuestaH>());
            new EncuestaEstadoMap (modelBuilder.Entity<EncuestaEstado>());
            new EncuestaOpcionesMap (modelBuilder.Entity<EncuestaOpcion>());
            new EncuestaPreguntaMap (modelBuilder.Entity<EncuestaPregunta>());
            new EncuestaRespuestaMap (modelBuilder.Entity<EncuestaRespuesta>());
            new QuejaSugerenciaDenunciaMap (modelBuilder.Entity<QuejaSugerenciaDenuncia>());
            new QuejaSugerenciaDenunciaStateMap (modelBuilder.Entity<QuejaSugerenciaDenunciaState>());
            new QuejaSugerenciaDenunciaTypeMap (modelBuilder.Entity<QuejaSugerenciaDenunciaType>());
            new FeriadoMap(modelBuilder.Entity<Feriado>());
            new RequestItemMap(modelBuilder.Entity<RequestItem>());
            new RequestConstanciaItemMap(modelBuilder.Entity<RequestConstanciaItem>());
            new ResourceResourceMap(modelBuilder.Entity<ResourceResource>());
            new LeaveMap(modelBuilder.Entity<Leave>());
            new PeriodoVacacionMap(modelBuilder.Entity<PeriodoVacacion>());
            new TipoVacacionMap(modelBuilder.Entity<TipoVacacion>());
            new RepositoryGroupMap(modelBuilder.Entity<RepositoryGroup>());
        }
    }
}
