using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RRHH_WEB_API._Common;
using RRHH_WEB_API._Entidades;
using RRHH_WEB_API._Infraestructura;
using RRHH_WEB_API.Features.Email;
using RRHH_WEB_API.Features.Maestros.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace RRHH_WEB_API.Features.Maestros
{
    public class MaestroService
    {
        private readonly RRHH_DBContext rrhh_Web_DBContext;
        private readonly ACS_DBContext _acs_DBContext;
        private readonly EmailService _emailService;
        private readonly Seguridad _seguridad;

        private readonly DbSet<Employee> _employeeInstance;
        private readonly DbSet<UserDelegation> _userDelegationInstance;
        private readonly DbSet<UserLevel> _userLevelInstance;
        private readonly DbSet<RequestConstancia> _requestConstancialInstance;
        private readonly DbSet<RequestType> _requestTypelInstance;
        private readonly DbSet<RequestState> _requestStatelInstance;
        private readonly DbSet<RequestVacacion> _requestVacacionInstance;
        private readonly DbSet<RequestVacacionTracking> _requestVacacionTrackingInstance;
        private readonly DbSet<Contract> _requestContractInstance;
        private readonly DbSet<PayslipRun> _payslipRunInstance;
        private readonly DbSet<PayslipLine> _payslipLineInstance;
        private readonly DbSet<Payslip> _paysLipInstance;
        private readonly DbSet<Journal> _journalInstance;
        private readonly DbSet<RepositoryImage> _repositoryImageInstance;
        private readonly DbSet<HoraEmpleadosRolDepartamento> _horasEmpleadosRolesDepartamentosInstance;
        private readonly DbSet<HoraEmpleadoNombre> _horaEmpleadoNombresInstance;
        private readonly DbSet<HoraEmpleadoTrabajada> _horaEmpleadoTrabajadasInstance;
        private readonly DbSet<BenefitDeduction> _benefitDeductionInstance;
        private readonly DbSet<RepositoryGroup> _repositoryGroupInstance;

        List<OrdenVoucherHorasExtras> ordenDetalle = new List<OrdenVoucherHorasExtras>();
        List<OrdenVoucherDeduccion> ordenVoucerDeducciones = new List<OrdenVoucherDeduccion>();
        List<OrdenVoucherBeneficios> ordenVoucerBeneficios = new List<OrdenVoucherBeneficios>();
    
       public  List<string> optionList = new List<string>
            { "AdditionalCardPersonAdressType", /* rest of elements */ };

      
        public MaestroService(RRHH_DBContext rrhh_DBContext, ACS_DBContext acs_DBContext,EmailService mailService, Seguridad seguridad)
        {
             rrhh_Web_DBContext = rrhh_DBContext;
            _acs_DBContext = acs_DBContext;
            _emailService = mailService;
            _seguridad = seguridad;

            _employeeInstance = rrhh_DBContext.Employee;
            _userDelegationInstance = acs_DBContext.UserDelegation;
            _userLevelInstance = acs_DBContext.UserLevel;
            _requestConstancialInstance = acs_DBContext.RequestConstancia;
            _requestTypelInstance = acs_DBContext.RequestType;
            _requestStatelInstance = acs_DBContext.RequestState;
            _requestVacacionInstance = acs_DBContext.RequestVacacion;
            _requestVacacionTrackingInstance = acs_DBContext.RequestVacacionTracking;
            _requestContractInstance = rrhh_DBContext.Contract;
            _journalInstance = rrhh_DBContext.Journal;
            _payslipRunInstance = rrhh_DBContext.PayslipRun;
            _payslipLineInstance = rrhh_DBContext.PayslipLine;
            _paysLipInstance = rrhh_DBContext.Payslip;
            _repositoryImageInstance = acs_DBContext.RepositoryImage;
            _horasEmpleadosRolesDepartamentosInstance = acs_DBContext.HorasEmpleadosRolesDepartamento;
            _horaEmpleadoTrabajadasInstance = acs_DBContext.HoraEmpleadoTrabajada;
            _horaEmpleadoNombresInstance = acs_DBContext.HoraEmpleadoNombre;
            _benefitDeductionInstance = rrhh_Web_DBContext.BenefitDeduction;
            _repositoryGroupInstance = acs_DBContext.RepositoryGroups;


            ordenDetalle.Add(new OrdenVoucherHorasExtras {Orden=1,Code= "BASE_EX",CodeRelated= "BASE_EX" });
            ordenDetalle.Add(new OrdenVoucherHorasExtras {Orden=2,Code= "HE_25", CodeRelated = "PHE25" });
            ordenDetalle.Add(new OrdenVoucherHorasExtras {Orden=3,Code= "HE_50", CodeRelated = "PHE50" });
            ordenDetalle.Add(new OrdenVoucherHorasExtras {Orden=4,Code= "HE_75", CodeRelated = "PHE75" });
            ordenDetalle.Add(new OrdenVoucherHorasExtras {Orden=5,Code= "HE_100", CodeRelated = "PHE100" });
            ordenDetalle.Add(new OrdenVoucherHorasExtras {Orden=10,Code= "TPHE" });


            //Deducciones Orden
            ordenVoucerDeducciones.Add(new OrdenVoucherDeduccion { Orden = 1, Code = "AHRECOOP" });
            ordenVoucerDeducciones.Add(new OrdenVoucherDeduccion { Orden = 2, Code = "AHRCOO" });
            ordenVoucerDeducciones.Add(new OrdenVoucherDeduccion { Orden = 3, Code = "PRCOOP" });
            ordenVoucerDeducciones.Add(new OrdenVoucherDeduccion { Orden = 4, Code = "BANC" });
            ordenVoucerDeducciones.Add(new OrdenVoucherDeduccion { Orden = 5, Code = "CAF" });
            ordenVoucerDeducciones.Add(new OrdenVoucherDeduccion { Orden = 6, Code = "IHSS_T" });
            ordenVoucerDeducciones.Add(new OrdenVoucherDeduccion { Orden = 7, Code = "SMED" });
            ordenVoucerDeducciones.Add(new OrdenVoucherDeduccion { Orden = 8, Code = "AFP" });
            ordenVoucerDeducciones.Add(new OrdenVoucherDeduccion { Orden = 9, Code = "INCAPACIDAD" });
            ordenVoucerDeducciones.Add(new OrdenVoucherDeduccion { Orden = 10, Code = "ISR" });
            ordenVoucerDeducciones.Add(new OrdenVoucherDeduccion { Orden = 11, Code = "USULA" });


            //Beneficios Orden
            ordenVoucerBeneficios.Add(new OrdenVoucherBeneficios { Orden = 1, Code = "DVAC" });
            ordenVoucerBeneficios.Add(new OrdenVoucherBeneficios { Orden = 2, Code = "DFT" });
            ordenVoucerBeneficios.Add(new OrdenVoucherBeneficios { Orden = 3, Code = "DLB" });
            ordenVoucerBeneficios.Add(new OrdenVoucherBeneficios { Orden = 4, Code = "BASE" });
            ordenVoucerBeneficios.Add(new OrdenVoucherBeneficios { Orden = 5, Code = "PVAC" });
            ordenVoucerBeneficios.Add(new OrdenVoucherBeneficios { Orden = 6, Code = "BON" });



        }

        #region Perfil

        public Response<PerfilEmpleadoDto> GetEmployee(int id)
        {
            try
            {
                PerfilEmpleadoDto empleado = _employeeInstance.AsQueryable().AsNoTracking()
                .Where(y => y.Id == id)
                .Select(x => new PerfilEmpleadoDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Identificacion = x.IdentificationId ?? "No encontrado",
                    Department = x.Department.Name ?? "No encontrado",
                    Weigth = (x.Weigth == null) ? "No encontrado" : x.Weigth.ToString(),
                    Height = (x.Height == null) ? "No encontrado" : x.Height.ToString(),
                    ShoeSize = x.ShoeSize ?? "No encontrado",
                    PantSize = x.PantSize ?? "No encontrado",
                    ShirtSize = x.ShirtSize ?? "No encontrado",
                    MobilePhone = x.MobilePhone ?? "No encontrado",
                    WorkEmail = x.WorkEmail ?? "No encontrado",
                    JobName = x.Job.Name ?? "No encontrado",
                    //Gender = (x.Gender == null) ? "No encontrado" : (x.Gender == "male") ? "Masculino" : "Femenino",//Antiguo
                    Gender = x.Gender == null ? "No encontrado" : x.Gender,//Codigo Nuevo
                    Birthday = (x.BirthDay == null) ? "No encontrado" : x.BirthDay.Value.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES")),
                    //PictureProfile = ConvertToBase64(x.Image)
                }).FirstOrDefault();

                //var employeeProfilePicture = _employeeInstance.AsQueryable().AsNoTracking()
                //.FirstOrDefault(y => y.Id == id).Image;

                    //var base64= ConvertToBase64(employeeProfilePicture);

                //var firstCharacter = base64.Substring(0,1);

                //string metadata="";

                //if (firstCharacter== "/")
                //{
                //    metadata = $"data:image/jpg;base64,";
                //}

                //if (firstCharacter == "i")
                //{
                //    metadata = $"data:image/png;base64,";
                //}

                //if (firstCharacter == "R")
                //{
                //    metadata = $"data:image/gif;base64,";
                //}

                //empleado.PictureProfile = metadata + base64;


                return Response<PerfilEmpleadoDto>.Success(empleado);
            }
            catch (Exception ex)
            {
                return Response<PerfilEmpleadoDto>.Validation(ex.Message);
            }
        }

        public Response<VoucherResponseDto> GetVoucher(int employeeId, int paysliprunid)
        {
            try
            { 
                VoucherResponseDto voucherResponse = new VoucherResponseDto();
                VoucherDto voucher = new VoucherDto();
                VoucherHorasExtasDto voucherPorHorasExtras = new VoucherHorasExtasDto();

                List<int> categoriesId = new List<int>{ 8,9,10};

                if (paysliprunid==0)
                {
                  return  Response<VoucherResponseDto>.Validation("DEBE SELECCIONAR UNA PLANILLA");
                }

                var payRollType = _payslipRunInstance.AsQueryable().AsNoTracking().FirstOrDefault(p => p.Id == paysliprunid).PayRollTypeId;

                var empleado = _employeeInstance.FirstOrDefault(e => e.Id == employeeId);


                if (payRollType==3) //Horas Extras
                {
                    voucherPorHorasExtras = _payslipRunInstance.AsQueryable().AsNoTracking().Where(u => u.Id == paysliprunid && u.Payslip.EmployeeId==employeeId)
                            .Select(p => new VoucherHorasExtasDto
                            {
                                PayslipName = p.Name,
                                State = p.State,
                                //EmployeeId = p.Payslip.EmployeeId,//Codigo Anterior
                                EmployeeId = employeeId,
                                EmployeeName = p.Payslip.Employee.Name,
                                Identificacion = p.Payslip.Employee.IdentificationId,
                                EmployeeDepartment = p.Payslip.Employee.Department.Name,
                                EmployeeJobName = p.Payslip.Employee.Job.Name,
                                EmployeeJournal = p.Payslip.Employee.Journal.Descripcion,
                                BarCode = p.Payslip.Employee.BarCode,
                                FechaPago = p.DateStart.ToString("dd/MM/yyyy") + " al " + p.DateEnd.ToString("dd/MM/yyyy"),
                                Moneda = p.CurrencId == 2 ? "USD" : "L",
                                DateStart = p.DateStart,
                                DateEnd = p.DateEnd,
                                Detalles = ObtenerDetalleHoras(employeeId, paysliprunid),
                            }).FirstOrDefault();

                    voucherPorHorasExtras.TotalHorasExtras = voucherPorHorasExtras.Detalles.Where(p => p.Code == "TPHE").Sum(t=> t.TotalLinea);

                    voucherPorHorasExtras.TotalCantidadHoras = voucherPorHorasExtras.Detalles.Where(x=> x.Code != "TPHE"). Sum(p => p.CantidadHoras);

                    voucherPorHorasExtras.Detalles = voucherPorHorasExtras.Detalles.Where(p => p.Code != "TPHE").ToList();
                }
                else
                {


                    List<VoucherDto> vouchersporempleado = _paysLipInstance.AsQueryable().AsNoTracking()
                        //.Where(f => f.PayslipRunId == paysliprunid && f.EmployeeId == employeeId  && !categoriesId.Contains(f.PayslipLine.CategoryId))
                        .Where(f =>
                            f.PayslipRunId == paysliprunid &&
                            f.EmployeeId == employeeId &&
                            (!f.PayslipLine.CategoryId.HasValue ||
                             !categoriesId.Contains(f.PayslipLine.CategoryId.Value))
                        )
                        .Select(x => new VoucherDto
                        {
                            Id = x.Id,
                            PayslipName = x.Name,
                            PayslipRunName = x.PayslipRun.Name,
                            State = x.State,
                            EmployeeId = x.EmployeeId,
                            EmployeeName = x.Employee.Name,
                            EmployeeDepartment = x.Employee.Department.Name,
                            EmployeeJobName = x.Employee.Job.Name,
                            EmployeeJournal = x.Employee.Journal.Descripcion,
                            BarCode = x.Employee.BarCode,
                            DateStart = x.PayslipRun.DateStart,
                            DateEnd = x.PayslipRun.DateEnd,
                            Moneda = x.PayslipRun.CurrencId == 2 ? "USD" : "L"
                        }).ToList();


                    var detalleSalario = _payslipLineInstance.AsQueryable().AsNoTracking()
                        .Where(f =>
                            f.Payslip.PayslipRunId == paysliprunid &&
                            f.Payslip.EmployeeId == employeeId &&
                            (!f.Payslip.PayslipLine.CategoryId.HasValue ||
                             !categoriesId.Contains(f.Payslip.PayslipLine.CategoryId.Value))
                        )
                    //.Where(f => f.Payslip.PayslipRunId == paysliprunid && f.EmployeId == employeeId  && !categoriesId.Contains(f.CategoryId))
                    .ToList();


               voucher = new VoucherDto
                {
                    Id = vouchersporempleado.FirstOrDefault().Id,
                    PayslipName = vouchersporempleado.FirstOrDefault().PayslipName,
                    PayslipRunName = vouchersporempleado.FirstOrDefault().PayslipRunName,
                    State = vouchersporempleado.FirstOrDefault().State,
                    EmployeeId = vouchersporempleado.FirstOrDefault().EmployeeId,
                    EmployeeName = vouchersporempleado.FirstOrDefault().EmployeeName,
                    EmployeeJobName = vouchersporempleado.FirstOrDefault().EmployeeJobName,
                    EmployeeDepartment = vouchersporempleado.FirstOrDefault().EmployeeDepartment,
                    EmployeeJournal = vouchersporempleado.FirstOrDefault().EmployeeJournal,
                    DateStart = vouchersporempleado.FirstOrDefault().DateStart,
                    DateEnd = vouchersporempleado.FirstOrDefault().DateEnd,
                    FechaPago = vouchersporempleado.FirstOrDefault().DateStart.ToString("dd/MM/yyyy") + " al " + vouchersporempleado.FirstOrDefault().DateEnd.ToString("dd/MM/yyyy"),
                    BarCode = vouchersporempleado.FirstOrDefault().BarCode,
                    Moneda = vouchersporempleado.FirstOrDefault().Moneda,

                    //Beneficios
                    DiasVacaciones = detalleSalario.Where(y => y.Code == "DVAC").FirstOrDefault()?.Amount ?? 0,
                    DiasFaltados = detalleSalario.Where(y => y.Code == "DFT").FirstOrDefault()?.Amount ?? 0,
                    DiasLaborados = detalleSalario.Where(y => y.Code == "DLB").FirstOrDefault()?.Amount ?? 0,
                    SalarioBase = detalleSalario.Where(y => y.Code == "BASE").FirstOrDefault()?.Amount ?? 0,
                    Vacaciones = detalleSalario.FirstOrDefault(y => y.Code == "PVAC")?.Amount ?? 0,
                    Bono = detalleSalario.FirstOrDefault(y => y.Code == "BON")?.Amount ?? 0,


                    ////Deducciones 
                    AhorroRetiroCooperativa = detalleSalario.Where(y => y.Code == "AHRECOOP").FirstOrDefault()?.Amount ?? 0,
                    AhorroFijoCooperativa = detalleSalario.Where(y => y.Code == "AHRCOO").FirstOrDefault()?.Amount ?? 0,
                    Bancos = detalleSalario.Where(y => y.Code == "BANC").FirstOrDefault()?.Amount ?? 0,
                    Cafeteria = detalleSalario.Where(y => y.Code == "CAF").FirstOrDefault()?.Amount ?? 0,
                    IHSS = detalleSalario.Where(y => y.Code == "IHSS_T").FirstOrDefault()?.Amount ?? 0,
                    AFP = detalleSalario.Where(y => y.Code == "AFP").FirstOrDefault()?.Amount ?? 0,
                    Incapacidades = detalleSalario.Where(y => y.Code == "INCAPACIDAD").FirstOrDefault()?.Amount ?? 0,
                    ISR = detalleSalario.Where(y => y.Code == "ISR").FirstOrDefault()?.Amount ?? 0,
                    USULA = detalleSalario.Where(y => y.Code == "USULA").FirstOrDefault()?.Amount ?? 0,

                    //Resumen
                    TotalEgresos = detalleSalario.Where(y => y.Code == "TEGRESO").FirstOrDefault().Amount.GetValueOrDefault(),
                    SalarioNeto = detalleSalario.Where(y => y.Code == "NET").FirstOrDefault().Amount.GetValueOrDefault(),
                    TotalIngresos = detalleSalario.Where(y => y.Code == "GROSS").FirstOrDefault().Amount.GetValueOrDefault()
                };


                    List<string> ExclusionescodeDeducciones = new List<string> { "DVAC", "DFT", "DLB", "BASE", "PVAC", "BON", "TEGRESO", "NET" };

                    List<string> codeBeneficios = new List<string> { "DVAC", "DFT", "DLB","PVAC", "BON", "BASIC" };

                  
                    string sql = $"EXEC dbo.uspGetBenefitsFromVoucherV3  { employeeId}, { paysliprunid}";

                    var beneficios = rrhh_Web_DBContext.BeneficioVoucher
                    .FromSqlRaw(sql).ToList().Select(x => new VoucherBeneficioPlanillaDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Monto = x.Monto,
                        Code = x.Code,
                        CurrencyName=x.CurrencyName
                    }).ToList();



                    string sql2 = $"EXEC dbo.uspGetDeductionFromVoucherV3  { employeeId}, { paysliprunid}";

                    var deducciones = rrhh_Web_DBContext.DeduccionesVoucher
                    .FromSqlRaw(sql2).ToList().Select(x => new VoucherDeduccionPlanillaDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Monto = x.Monto,
                        Code = x.Code
                    }).ToList();



                    voucher.Deducciones = deducciones;
                    voucher.Beneficios = beneficios;

                    voucher.TotalEgresos= deducciones.Where(d=> d.Code!= "GROSS").Sum(d => d.Monto);

                    //voucher.TotalIngresos=voucher.SalarioBase+voucher.Bono+voucher.Vacaciones;
                }

                voucherResponse.PayRolTypeId = payRollType;
                voucherResponse.Voucher = voucher;
                voucherResponse.VoucherHorasExtas = voucherPorHorasExtras;

                return Response<VoucherResponseDto>.Success(voucherResponse);


            }
            catch (Exception ex)
            {
                return Response<VoucherResponseDto>.Validation(ex.Message);
            }
        }

        public List<VoucherHorasExtasDto.DetalleHoras> ObtenerDetalleHoras(int employeeid, int paysliprunid)
        {

            try
            {

           
            List<int> cantidadHoras = new List<int> { 81, 64, 65, 63};
            List<int> TotalLineas = new List<int> { 82, 68, 69, 70, 86, 71 };
            List<string> Codes = new List<string> { "HE_25", "HE_50", "HE_75", "HE_100", "BASE_EX", "TPHE" };
            List<string> CodesPagos = new List<string> { "PHE25", "PHE50", "PHE75", "PHE100", "BASE_EX", "TPHE" };

            var detalleHoras = _payslipRunInstance.AsQueryable().AsNoTracking()
                                .Where(p => p.Id == paysliprunid && p.Payslip.EmployeeId == employeeid && Codes.Contains(p.Payslip.PayslipLine.Code)
                                        )
                            .Select(p => new VoucherHorasExtasDto.DetalleHoras
                            {
                                Code=p.Payslip.PayslipLine.Code,
                                CodeRelated= p.Payslip.PayslipLine.Code== "BASE_EX" || p.Payslip.PayslipLine.Code == "TPHE" ? p.Payslip.PayslipLine.Code: "P" + p.Payslip.PayslipLine.Code.Replace("_","") ,
                                Detalle = p.Payslip.PayslipLine.Name,
                                CantidadHoras = cantidadHoras.Contains(p.Payslip.PayslipLine.SalaryRuleId)==true?(decimal)p.Payslip.PayslipLine.Amount:0,
                            }).ToList();




                var Pago_detalleHoras = _payslipRunInstance.AsQueryable().AsNoTracking()
                    .Where(p => p.Id == paysliprunid && p.Payslip.EmployeeId == employeeid && CodesPagos.Contains(p.Payslip.PayslipLine.Code)
                            )
                .Select(p => new VoucherHorasExtasDto.DetalleHoras
                {
                    Code = p.Payslip.PayslipLine.Code,
                    //CodeRelated = "P" + p.Payslip.PayslipLine.Code,
                    Detalle = p.Payslip.PayslipLine.Name,
                    //CantidadHoras = cantidad_detalleHoras.AsQueryable().AsNoTracking().FirstOrDefault(r => r.CodeRelated == p.Payslip.PayslipLine.Code).CantidadHoras,
                    TotalLinea = TotalLineas.Contains(p.Payslip.PayslipLine.SalaryRuleId)==true?(decimal)p.Payslip.PayslipLine.Amount:0
                            }).ToList();


                foreach (var item in detalleHoras)
                {
                    item.TotalLinea = Pago_detalleHoras.AsQueryable().AsNoTracking().FirstOrDefault(e => e.Code == item.CodeRelated).TotalLinea;
                }

                var detalleHorasSinDuplicados = detalleHoras.GroupBy(x => x.Code).Select(d => d.First()).ToList(); 

                foreach (var item in detalleHorasSinDuplicados)
                {
                    foreach (var elemento in ordenDetalle)
                    {
                        if (item.Code == elemento.Code)
                        {
                            item.Orden = elemento.Orden;
                        }
                    }

                }

                return detalleHorasSinDuplicados.OrderBy(p => p.Orden).ToList();
                //return new List<VoucherHorasExtasDto.DetalleHoras>();
            }
            catch (Exception ex)
            {
                return new List<VoucherHorasExtasDto.DetalleHoras>();

            }

        }

        public Response<List<NominaEncabezadoDto>> GetNominaEncabezado(int employeeId)
        {
            try
            {


                var idAExcluir = new List<int>{ 8, 9 };
                var stateAExcluir = new List<string>{ "anulated", "draft", "Cancelada" , "Borrador" };
                var stateAExcluirPayslip = new List<string>{ "anulated", "draft", "Cancelada" };

                var lastDate = _payslipRunInstance.AsQueryable().AsNoTracking().OrderByDescending(t => t.CreateDate).First().CreateDate.AddMonths(-6).Date;


                List<NominaEncabezadoDto> NominasEncabezado = _payslipRunInstance.AsQueryable().AsNoTracking()
                    .Where(f => f.Payslip.EmployeeId == employeeId 
                            && !idAExcluir.Contains(f.PayRollTypeId) 
                            && !stateAExcluir.Contains(f.State) && f.CreateDate.Date >= lastDate
                            && !stateAExcluirPayslip.Contains(f.Payslip.State)
                            && f.Payslip.Enable==true
                            )
                    .Select(x => new NominaEncabezadoDto
                    {
                        ID = x.Id,
                        Name = x.Name,
                        CreateDate = x.CreateDate
                    }).Distinct().OrderByDescending(x => x.CreateDate).ToList();

                return Response<List<NominaEncabezadoDto>>.Success(NominasEncabezado);
            }
            catch (Exception ex)
            {
                return Response<List<NominaEncabezadoDto>>.Validation(ex.Message);
            }
        }


        public Response<List<HoraEmpleadoDto>> DetalleHorasEmpleado(RangoFechaHorasEmpleadoParamsDto horasEmpleado)
        {
            try
            {

                DiasSemana diaSemana = new DiasSemana();

                var culture = new System.Globalization.CultureInfo("es-ES");

                //var detalleHoras = _horaEmpleadoTrabajadasInstance.AsQueryable().Include(f=>f.Employee)
                //                    .AsNoTracking().Where(h => h.EmployeeId == horasEmpleado.EmployeeId
                //                    && h.FechaI >= Convert.ToDateTime(horasEmpleado.FechaInicio)
                //                    && h.FechaI <= Convert.ToDateTime(horasEmpleado.FechaFin)
                //                    && h.Enable == true
                //                    )
                //     .Select(r => new HoraEmpleadoDto
                //     {
                //         Serial = r.Id.ToString(),
                //         Code = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r=> r.Id==horasEmpleado.EmployeeId).BarCode,
                //         EmpleadoId=(int)r.EmployeeId,
                //         EmployeeName = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == horasEmpleado.EmployeeId).Name,
                //         NormalHour=r.Cantidad,
                //         ExtraHours = r.CantidadDe,
                //         FechaI = (DateTime)r.FechaI,
                //         FechaF = (DateTime)r.FechaF.GetValueOrDefault(),
                //         Fecha = culture.DateTimeFormat.GetDayName(r.Fecha.DayOfWeek) + ", " + r.Fecha.ToString("dd/MM/yyyy"),
                //         Departamento = r.Employee.HoraEmpleadoDepartamento.Name,
                //         Semana = (int)r.Week
                //     }).ToList();


               var activoParam = new SqlParameter("@employee_id", horasEmpleado.EmployeeId);
               var fechaIParam = new SqlParameter("@fechaI", horasEmpleado.FechaInicio);
               var fechaFParam = new SqlParameter("@fechaF", horasEmpleado.FechaFin);

                var horas_trabajadas = _acs_DBContext.HorasTrabajadasEmpleados
                    .FromSqlRaw(
                        "EXEC usp_horas_empleado_trabajadas_Listar @employee_id,@fechaI,@fechaF",
                        activoParam,fechaIParam,fechaFParam)
                    .ToList();

                var detalleHoras = horas_trabajadas.AsQueryable()
                         .Select(r => new HoraEmpleadoDto
                         {
                             Serial = r.Serial.ToString(),
                             Code = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == horasEmpleado.EmployeeId).BarCode,
                             EmpleadoId = (int)r.EmpleadoId,
                             EmployeeName = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == horasEmpleado.EmployeeId).Name,
                             NormalHour = r.NormalHour,
                             ExtraHours = r.ExtraHours,
                             FechaI =  r.FechaI,
                             FechaF =  r.FechaF,
                             Fecha = culture.DateTimeFormat.GetDayName(r.Fecha.DayOfWeek) + ", " + r.Fecha.ToString("dd/MM/yyyy"),
                             Departamento = r.Departamento,
                             Semana =  r.Semana
                         }).ToList() ;

                return Response<List<HoraEmpleadoDto>>.Success(detalleHoras);
            }
            catch (Exception ex)
            {
                return Response<List<HoraEmpleadoDto>>. Excepcion(ex.Message);
            }
        }

        public Response<bool> SendEmailVoucher(int employeeId, int paysliprunid)
        {
            try
            {
                VoucherResponseDto voucherResponse = new VoucherResponseDto();
                VoucherDto voucher = new VoucherDto();
                VoucherHorasExtasDto voucherPorHorasExtras = new VoucherHorasExtasDto();

                List<int> categoriesId = new List<int> { 8, 9, 10 };
                EmailSendParams _emailSendParams = new EmailSendParams();

                if (paysliprunid == 0)
                {
                    return Response<bool>.Validation("DEBE SELECCIONAR UNA PLANILLA");
                }

                var payRollType = _payslipRunInstance.AsQueryable().AsNoTracking().FirstOrDefault(p => p.Id == paysliprunid).PayRollTypeId;
                var mailEmpleado = _employeeInstance.AsQueryable().AsNoTracking().First(r => r.Id == employeeId).WorkEmail;

                if (string.IsNullOrEmpty(mailEmpleado))
                {
                    return Response<bool>.Validation("No se pudo enviar el correo, no tiene un correo electrónico configurado");
                }

                if (payRollType == 3) //Horas Extras
                {
                    voucherPorHorasExtras = _payslipRunInstance.AsQueryable().AsNoTracking().Where(u => u.Id == paysliprunid && u.Payslip.EmployeeId == employeeId)
                            .Select(p => new VoucherHorasExtasDto
                            {
                                PayslipName = p.Name,
                                State = p.State,
                                EmployeeId = p.Payslip.EmployeeId,
                                EmployeeName = p.Payslip.Employee.Name,
                                Identificacion = p.Payslip.Employee.IdentificationId,
                                EmployeeDepartment = p.Payslip.Employee.Department.Name,
                                EmployeeJobName = p.Payslip.Employee.Job.Name,
                                EmployeeJournal = p.Payslip.Employee.Journal.Descripcion,
                                BarCode = p.Payslip.Employee.BarCode,
                                FechaPago = p.DateStart.ToShortDateString() + " al " + p.DateEnd.ToShortDateString(),
                                Moneda = p.CurrencId == 2 ? "USD" : "L",
                                DateStart = p.DateStart,
                                DateEnd = p.DateEnd,
                                Detalles = ObtenerDetalleHoras(employeeId, paysliprunid),
                            }).FirstOrDefault();

                    voucherPorHorasExtras.TotalHorasExtras = voucherPorHorasExtras.Detalles.Where(p => p.Code == "TPHE").Sum(t => t.TotalLinea);

                    voucherPorHorasExtras.TotalCantidadHoras = voucherPorHorasExtras.Detalles.Where(x => x.Code != "TPHE").Sum(p => p.CantidadHoras);

                    voucherPorHorasExtras.Detalles = voucherPorHorasExtras.Detalles.Where(p => p.Code != "TPHE").ToList();

                    string filas = "";

                    string titulo = "<h2>"+voucherPorHorasExtras.PayslipName+"</h2>";
                    
                    string encabezado = @"<table>
                                                <thead>
                                                      <tr>
                                                        <td><span style='width:100%'><strong>Colaborador:</strong> " + voucherPorHorasExtras.EmployeeName + @"</span></td>
                                                      </tr>
                                                      <tr>
                                                        <td><span><strong>Cargo:</strong> " + voucherPorHorasExtras.EmployeeJobName + @"</span></td>
                                                      </tr>
                                                      <tr>
                                                        <td><span><strong>Fecha Pago:</strong> " + voucherPorHorasExtras.DateStart + " al " + voucherPorHorasExtras.DateEnd + @"</span></td>
                                                      </tr>
                                                      <tr>
                                                        <td><span><strong>Código:</strong> " + voucherPorHorasExtras.BarCode + @"</span></td>
                                                      </tr>
                                                      <tr>
                                                        <td><span><strong>Departamento:</strong>" + voucherPorHorasExtras.EmployeeDepartment + @"</span></td>
                                                      </tr>
                                                      <tr>
                                                        <td><span><strong>Turno:</strong>" + voucherPorHorasExtras.EmployeeJournal + @" </span></td>
                                                      </tr>
                                                </thead>
                                            </table>";

                    string filaTotal = "";

                    foreach (var item in voucherPorHorasExtras.Detalles)
                    {
                        filas = filas + " <tr><td>" + item.Detalle+"</td>"+ "<td>" + item.CantidadHoras + "</td>" + "<td>" + item.TotalLinea.ToString("N2") + "</td></tr>";
                    }


                     filaTotal = filaTotal + "<tr style='background-color: bisque;font-weight: bold;'><td>Total Horas Extras</td>" + "<td>" + voucherPorHorasExtras.TotalCantidadHoras + "</td>" + "<td>" + voucherPorHorasExtras.TotalHorasExtras.ToString("N2") + "</td></tr>";



                    filas = filas + filaTotal;

                    _emailSendParams = new EmailSendParams();

                    _emailSendParams.Destinatarios = new List<string>();

                    _emailSendParams.Subject = "Voucher de pago por Horas Extras";
                    _emailSendParams.Body = @"<html>
                                                  <head>
                                                    <title></title>
	                                                <style>
                                                #customers {
                                                  font-family: Arial, Helvetica, sans-serif;
                                                  border-collapse: collapse;
                                                  width: 100%;
                                                  margin:10px
                                                }

                                                #customers td, #customers th {
                                                  border: 1px solid #ddd;
                                                  padding: 8px;
                                                }

                                                #customers tr:nth-child(even){background-color: #f2f2f2;}

                                                #customers th {
                                                  padding-top: 5px;
                                                  padding-bottom: 5px;
                                                  text-align: left;
                                                  background-color: #e7e7e7;
                                                  color: black;
                                                }

                                                </style>
                                                  </head>
                                                  <body>"+titulo+encabezado+@"<br>
                                                  <div style='display:flex;width: 50%;'>
                                                    <table id='customers'>
                                                      <thead>
                                                        <tr>
                                                          <th>Detalle</th>
                                                          <th>Cantidad</th>
                                                          <th>Total</th>
                                                        </tr>
                                                      </thead>
                                                      <tbody>
                                                          "+filas+" </tbody>  </table> </div>      </body> </html>"
                                                ;

                    _emailSendParams.Destinatarios.Add(mailEmpleado);


                    _emailService.EnviarCorreo_General(_emailSendParams);
                }
                else
                {


                    List<VoucherDto> vouchersporempleado = _paysLipInstance.AsQueryable().AsNoTracking()
                        //.Where(f => f.PayslipRunId == paysliprunid && f.EmployeeId == employeeId && f.PayslipLine.SalaryRuleId != 2 && !categoriesId.Contains(f.PayslipLine.CategoryId))
                        .Where(f =>
                        f.PayslipRunId == paysliprunid &&
                        f.EmployeeId == employeeId &&
                        (!f.PayslipLine.CategoryId.HasValue ||
                         !categoriesId.Contains(f.PayslipLine.CategoryId.Value))
)
                        .Select(x => new VoucherDto
                        {
                            Id = x.Id,
                            PayslipName = x.Name,
                            PayslipRunName = x.PayslipRun.Name,
                            State = x.State,
                            EmployeeId = x.EmployeeId,
                            EmployeeName = x.Employee.Name,
                            EmployeeDepartment = x.Employee.Department.Name,
                            EmployeeJobName = x.Employee.Job.Name,
                            EmployeeJournal = x.Employee.Journal.Descripcion,
                            BarCode = x.Employee.BarCode,
                        DateStart = x.PayslipRun.DateStart,
                            DateEnd = x.PayslipRun.DateEnd,
                            Moneda = x.PayslipRun.CurrencId == 2 ? "USD" : "L"
                        }).ToList();





                    var detalleSalario = _payslipLineInstance.AsQueryable().AsNoTracking()
                        //.Where(f => f.Payslip.PayslipRunId == paysliprunid && f.EmployeId == employeeId && f.SalaryRuleId != 2 && !categoriesId.Contains(f.CategoryId))
                        .Where(f =>
                        f.Payslip.PayslipRunId == paysliprunid &&
                        f.Payslip.EmployeeId == employeeId &&
                        (!f.Payslip.PayslipLine.CategoryId.HasValue ||
                         !categoriesId.Contains(f.Payslip.PayslipLine.CategoryId.Value))
)
                        .ToList();


                    voucher = new VoucherDto
                    {
                        Id = vouchersporempleado.FirstOrDefault().Id,
                        PayslipName = vouchersporempleado.FirstOrDefault().PayslipName,
                        PayslipRunName = vouchersporempleado.FirstOrDefault().PayslipRunName,
                        State = vouchersporempleado.FirstOrDefault().State,
                        EmployeeId = vouchersporempleado.FirstOrDefault().EmployeeId,
                        EmployeeName = vouchersporempleado.FirstOrDefault().EmployeeName,
                        EmployeeJobName = vouchersporempleado.FirstOrDefault().EmployeeJobName,
                        EmployeeDepartment = vouchersporempleado.FirstOrDefault().EmployeeDepartment,
                        EmployeeJournal = vouchersporempleado.FirstOrDefault().EmployeeJournal,
                        //Code = vouchersporempleado.FirstOrDefault().Code,
                        DateStart = vouchersporempleado.FirstOrDefault().DateStart,
                        DateEnd = vouchersporempleado.FirstOrDefault().DateEnd,
                        FechaPago = vouchersporempleado.FirstOrDefault().DateStart.ToShortDateString() + " al " + vouchersporempleado.FirstOrDefault().DateEnd.ToShortDateString(),
                        BarCode = vouchersporempleado.FirstOrDefault().BarCode,
                        Moneda = vouchersporempleado.FirstOrDefault().Moneda,

                        //Beneficios
                        DiasVacaciones = detalleSalario.Where(y => y.Code == "DVAC").FirstOrDefault()?.Amount ?? 0,
                        DiasFaltados = detalleSalario.Where(y => y.Code == "DFT").FirstOrDefault()?.Amount ?? 0,
                        DiasLaborados = detalleSalario.Where(y => y.Code == "DLB").FirstOrDefault()?.Amount ?? 0,
                        SalarioBase = detalleSalario.Where(y => y.Code == "BASE").FirstOrDefault()?.Amount ?? 0,
                        Vacaciones = detalleSalario.FirstOrDefault(y => y.Code == "PVAC")?.Amount ?? 0,
                        Bono = detalleSalario.FirstOrDefault(y => y.Code == "BON")?.Amount ?? 0,


                        ////Deducciones 
                        AhorroRetiroCooperativa = detalleSalario.Where(y => y.Code == "AHRECOOP").FirstOrDefault()?.Amount ?? 0,
                        AhorroFijoCooperativa = detalleSalario.Where(y => y.Code == "AHRCOO").FirstOrDefault()?.Amount ?? 0,
                        Bancos = detalleSalario.Where(y => y.Code == "BANC").FirstOrDefault()?.Amount ?? 0,
                        Cafeteria = detalleSalario.Where(y => y.Code == "CAF").FirstOrDefault()?.Amount ?? 0,
                        IHSS = detalleSalario.Where(y => y.Code == "IHSS_T").FirstOrDefault()?.Amount ?? 0,
                        AFP = detalleSalario.Where(y => y.Code == "AFP").FirstOrDefault()?.Amount ?? 0,
                        Incapacidades = detalleSalario.Where(y => y.Code == "INCAPACIDAD").FirstOrDefault()?.Amount ?? 0,
                        ISR = detalleSalario.Where(y => y.Code == "ISR").FirstOrDefault()?.Amount ?? 0,
                        USULA = detalleSalario.Where(y => y.Code == "USULA").FirstOrDefault()?.Amount ?? 0,

                        //Resumen
                        TotalEgresos = detalleSalario.Where(y => y.Code == "TEGRESO").FirstOrDefault().Amount.GetValueOrDefault(),
                        SalarioNeto = detalleSalario.Where(y => y.Code == "NET").FirstOrDefault().Amount.GetValueOrDefault()
                    };


                    List<string> ExclusionescodeDeducciones = new List<string> { "DVAC", "DFT", "DLB", "BASE", "PVAC", "BON", "TEGRESO", "NET" };

                    List<string> codeBeneficios = new List<string> { "DVAC", "DFT", "DLB", "BASE", "PVAC", "BON" };

                    var deducciones = detalleSalario.Where(d => !ExclusionescodeDeducciones.Contains(d.Code)
                                    ).Select(y => new VoucherDeduccionPlanillaDto
                                    {
                                        Id = y.Id,
                                        Code = y.Code,
                                        Name = y.Name,
                                        Monto = y.Amount ?? 0,
                                        Orden = ordenVoucerDeducciones.FirstOrDefault(p => p.Code == y.Code)?.Orden ?? 0

                                    }).OrderBy(o => o.Orden).ToList();

                    var beneficios = detalleSalario.Where(d => codeBeneficios.Contains(d.Code)
                                   ).Select(y => new VoucherBeneficioPlanillaDto
                                   {
                                       Id = y.Id,
                                       Code = y.Code,
                                       Name = y.Name,
                                       Monto = y.Amount ?? 0,
                                       Orden = ordenVoucerBeneficios.FirstOrDefault(p => p.Code == y.Code)?.Orden ?? 0

                                   }).OrderBy(o => o.Orden).ToList();

                    voucher.Deducciones = deducciones;
                    voucher.Beneficios = beneficios;

                    voucher.TotalIngresos = voucher.SalarioBase + voucher.Bono + voucher.Vacaciones;

                    _emailSendParams = new EmailSendParams();

                    string titulo = "<h2>" + voucher.PayslipName + "</h2>";

                    string encabezado = @"<table>
                                                <thead>
                                                      <tr>
                                                        <td><span style='width:100%'><strong>Colaborador:</strong> " + voucher.EmployeeName + @"</span></td>
                                                      </tr>
                                                      <tr>
                                                        <td><span><strong>Cargo:</strong> " + voucher.EmployeeJobName + @"</span></td>
                                                      </tr>
                                                      <tr>
                                                        <td><span><strong>Fecha Pago:</strong> " + voucher.DateStart.ToShortDateString() + " al " + voucher.DateEnd.ToShortDateString() + @"</span></td>
                                                      </tr>
                                                      <tr>
                                                        <td><span><strong>Código:</strong> " + voucher.BarCode + @"</span></td>
                                                      </tr>
                                                      <tr>
                                                        <td><span><strong>Departamento:</strong>" + voucher.EmployeeDepartment + @"</span></td>
                                                      </tr>
                                                      <tr>
                                                        <td><span><strong>Turno:</strong>" + voucher.EmployeeJournal + @" </span></td>
                                                      </tr>
                                                </thead>
                                            </table>";


                    string filasBeneficios = "";
                    string filasDeducciones = "";

                    foreach (var item in voucher.Beneficios)
                    {
                        filasBeneficios = filasBeneficios + "<tr><td> "+item.Name+" </td><td>"+item.Monto.ToString("N2") + "</td></tr>";
                    }


                    foreach (var item in voucher.Deducciones)
                    {
                        filasDeducciones = filasDeducciones + "<tr><td> " + item.Name + " </td><td>" + item.Monto.ToString("N2") + "</td></tr>";
                    }

                    _emailSendParams.Destinatarios = new List<string>();

                    _emailSendParams.Subject = "Voucher de pago Quicenal";
                    _emailSendParams.Body = @"<html>
                                                  <head>
                                                    <title></title>
	                                                <style>
                                                #customers {
                                                  font-family: Arial, Helvetica, sans-serif;
                                                  border-collapse: collapse;
                                                  width: 100%;
                                                  margin-right:10px
                                                }

                                                #customers td, #customers th {
                                                  border: 1px solid #ddd;
                                                  padding: 8px;
                                                }

                                                #customers tr:nth-child(even){background-color: #f2f2f2;}

                                                #customers th {
                                                  padding-top: 5px;
                                                  padding-bottom: 5px;
                                                  text-align: left;
                                                  background-color: #e7e7e7;
                                                  color: black;
                                                }

                                                </style>
                                                  </head>
                                                  <body>"+titulo+encabezado+@"
                                                  <div style='display:flex;width: 50%;'>
                                                    <table id='customers'>
                                                      <thead>
                                                        <tr>
                                                          <th>Detalle</th>
                                                          <th>Cantidad</th>
                                                        </tr>
                                                      </thead>
                                                      <tbody>
                                                        "+filasBeneficios+ @"<tr  style='background-color: bisque;font-weight: bold;'>
                                                          <td><strong>Ingresos</strong></td>
                                                          <td><strong>" + voucher.TotalIngresos.ToString("N2") + @"</strong></td>
                                                        </tr>
                                                      </tbody>
                                                    </table><br>
	                                                <table id='customers'>
                                                      <thead>
                                                        <tr>
                                                          <th>Detalle</th>
                                                          <th>Cantidad</th>
                                                        </tr>
                                                      </thead>
                                                      <tbody>d
                                                        " + filasDeducciones+ @"
                                                        <tr style='background-color: bisque;font-weight: bold;'>
                                                         <td><strong>Total Egresos</strong></td>
                                                          <td><strong>" + voucher.TotalEgresos.ToString("N2") + @"</strong></td>
                                                        </tr>
                                                      </tbody>
                                                    </table>
	                                                </div>
                                                  </body>
                                                </html>";
                                                ;

                    _emailSendParams.Destinatarios.Add(mailEmpleado);


                    _emailService.EnviarCorreo_General(_emailSendParams);
                }

                return Response<bool>.Success(true);


            }
            catch (Exception ex)
            {
                return Response<bool>.Validation(ex.Message);
            }
        }

        #endregion

        #region RolesUsuarios

        public Response<List<RolUsuarioDto>> ObtenerRolesUsuarios()
        {
            try
            {

                var userDelegations = _userDelegationInstance.AsQueryable().AsNoTracking()
                                    .Select(p => new { 
                                            p.EmployeeId,
                                            p.UserLevelId,
                                            p.UserLevel.Name
                                    }).AsEnumerable();

               var idsFilter = userDelegations.AsQueryable().AsNoTracking().Where(y=>y.UserLevelId==1).Select(f => f.EmployeeId).ToList();

                //Codigo anterior
                //var roles = _employeeInstance.AsQueryable().AsNoTracking()
                //            .Where(r => r.Resource.Active == true).ToList();

                //Codigo Nuevo
                var employees = _employeeInstance.AsQueryable().AsNoTracking()
                            //.Where(t=> idsFilter.Contains(t.Id)).ToList();
                            .Where(r => r.Active == true).ToList()
                            .Select(f => new RolUsuarioDto
                            {
                                Id = f.Id,
                                EmployeeName = f.Name,
                                Code = f.BarCode,
                                EmployeeId = f.Id,
                                NivelUsuarioId=2,
                                NivelUsuario= "Usuario"
                            }).ToList();


                foreach (var empleado in employees)
                {
                    if (idsFilter.Contains( empleado.Id))
                    {
                        empleado.NivelUsuarioId = 1;
                        empleado.NivelUsuario = "Administrador";
                    }

                }

                //var rolesFinal = employees.Select(p => new RolUsuarioDto
                //            {
                //                EmployeeId = p.Id,
                //                //EmployeeName = p.Name,
                //                //NivelUsuarioId = p.UserDelegation.UserLevel.Name == null ? (int)UserLevelEnum.Usuario : p.UserDelegation.UserLevelId, //Codigo Annterior
                //                //NivelUsuarioId = userDelegations.FirstOrDefault(d=> d.EmployeeId==p.Id).Name == null ? (int)UserLevelEnum.Usuario : userDelegations.FirstOrDefault(d => d.EmployeeId == p.Id).UserLevelId,//Codigo Nuevo
                //                //NivelUsuario = p.UserDelegation.UserLevel.Name == null ? "Usuario" : p.UserDelegation.UserLevel.Name, //Codigo Annterior
                //                //NivelUsuario = userDelegations.FirstOrDefault(d => d.EmployeeId == p.Id).Name == null ? "Usuario" : userDelegations.FirstOrDefault(d => d.EmployeeId == p.Id).Name,// Codigo Nuevo
                //                //Code = p.BarCode

                //            }).OrderBy(t=>t.EmployeeName).ToList();


                return Response<List<RolUsuarioDto>>.Success(employees);
            }
            catch (Exception ex)
            {
                return Response<List<RolUsuarioDto>>.Validation(ex.Message);
            }
        }


        public Response<bool> CambiarRolUsuario(RolUsuarioParamsDto rolParam)
        {
            try
            {
                UserDelegation empleado = new UserDelegation();

                int countRecord= _userDelegationInstance.AsQueryable().AsNoTracking().Where(t => t.EmployeeId == rolParam.EmployeeId).Count();

                if (countRecord==0)
                {

                    empleado.UserLevelId = rolParam.UserLevelId;
                    empleado.EmployeeId = rolParam.EmployeeId;
                    empleado.Enable = true;

                    _userDelegationInstance.Add(empleado);
                }
                else
                {
                    empleado = _userDelegationInstance.AsQueryable().AsNoTracking().Where(t => t.EmployeeId == rolParam.EmployeeId).FirstOrDefault();
                   
                    empleado.UserLevelId = rolParam.UserLevelId;
                    _userDelegationInstance.Update(empleado);
                }

                _acs_DBContext.SaveChanges();

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Validation(ex.Message);
            }
        }

        public Response<bool> CambiarPIN(int employeeId, string nuevoPin)
        {
            try
            {
                Employee employee = _employeeInstance.AsQueryable().FirstOrDefault(x => x.Id == employeeId);
                if (employee == null) return Response<bool>.Validation("Empleado no encontrado");

                employee.Pin = _seguridad.GenerarHash(nuevoPin);

                _employeeInstance.Update(employee);
                rrhh_Web_DBContext.SaveChanges();

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Validation(ex.Message);
            }
        }


        #endregion

        public string ConvertToBase64(byte[] data)
        {
            if (data != null)
            {
                string base64String = Convert.ToBase64String(data, 0, data.Length);
                return base64String;

            }
            else
            {
                return " ";
            }
    


        }

     public class OrdenVoucherHorasExtras
        {
            public int Orden { get; set; }
            public string Code { get; set; }
            public string CodeRelated { get; set; }
        }


        public class OrdenVoucherDeduccion
        {
            public int Orden { get; set; }
            public string Code { get; set; }
        }


        public class OrdenVoucherBeneficios
        {
            public int Orden { get; set; }
            public string Code { get; set; }
        }

    }
}
