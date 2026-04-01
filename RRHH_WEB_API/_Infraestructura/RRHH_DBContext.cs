using Microsoft.EntityFrameworkCore;
using RRHH_WEB_API._Entidades;
using RRHH_WEB_API._Entidades.Encuesta;
using RRHH_WEB_API._Entidades.Encuestas;
using RRHH_WEB_API._Entidades.QuejasSugerenciasDenuncias;
using RRHH_WEB_API._Infraestructura.Map;
using RRHH_WEB_API._Infraestructura.Map.Encuesta;
using RRHH_WEB_API._Infraestructura.Map.QuejasSugerenciasDenuncias;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RRHH_WEB_API._Infraestructura
{
    public class RRHH_DBContext : DbContext
    {
        public RRHH_DBContext(DbContextOptions<RRHH_DBContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Department> Department { get; set; }       
        public DbSet<Contract> Contract { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<PayslipRun> PayslipRun { get; set; }
        public DbSet<PayslipLine> PayslipLine { get; set; }
        public DbSet<Payslip> Payslip { get; set; }
        public DbSet<Journal> Journal { get; set; }
        public DbSet<BenefitDeduction>  BenefitDeduction { get; set; }
        public DbSet<Concept> Concepts { get; set; }        
        public DbSet<ResourceResource> ResourceResource { get; set; }
        public DbSet<EmailNotificacionConfig> EmailNotificacionConfig { get; set; }
        public DbSet<Leave> Leave { get; set; }
        public DbSet<Deducciones> Deductions { get; set; }
        public DbSet<BenefitsVoucher> BeneficioVoucher { get; set; }
        public DbSet<DeductionVoucher> DeduccionesVoucher { get; set; }
        public DbSet<EmployeePicture> EmployeePicture { get; set; }
       
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

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
            [NotMapped]
            public long Id { get; set; }
            public string Name { get; set; }
            public decimal Monto { get; set; }
            public string Code { get; set; }
            public string CurrencyName { get; set; }
        }


        public class DeductionVoucher
        {
            [NotMapped]
            public long Id { get; set; }
            public string Name { get; set; }
            public decimal Monto { get; set; }
            public string Code { get; set; }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new EmployeeMap(modelBuilder.Entity<Employee>());
            new DepartmentMap(modelBuilder.Entity<Department>());
            new JobMap(modelBuilder.Entity<Job>());           
            new ContractMap(modelBuilder.Entity<Contract>());
            new JournalMap(modelBuilder.Entity<Journal>());
            new PayslipRunMap(modelBuilder.Entity<PayslipRun>());
            new PayslipLineMap(modelBuilder.Entity<PayslipLine>());
            new PayslipMap(modelBuilder.Entity<Payslip>());            
            new BenefitDeductionMap(modelBuilder.Entity<BenefitDeduction>());
            new ConceptMap(modelBuilder.Entity<Concept>());            
            new ResourceResourceMap(modelBuilder.Entity<ResourceResource>());
            new LeaveMap(modelBuilder.Entity<Leave>());            
            new UserRefreshTokenMap(modelBuilder.Entity<UserRefreshToken>());
            new EmployeePictureMap(modelBuilder.Entity<EmployeePicture>());
            new JobMap(modelBuilder.Entity<Job>());

            modelBuilder.Entity<EmailNotificacionConfig>().ToTable("email_notifications_config", "dbo");

            modelBuilder.Entity<EmailNotificacionConfig>()
           .HasNoKey();

            modelBuilder.Entity<BenefitsVoucher>()
            .HasNoKey();

            modelBuilder.Entity<DeductionVoucher>()
            .HasNoKey();
        }
    }
}
