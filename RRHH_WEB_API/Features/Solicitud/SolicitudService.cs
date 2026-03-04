using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RRHH_WEB_API._Common;
using RRHH_WEB_API._Entidades;

using RRHH_WEB_API._Infraestructura;
using RRHH_WEB_API.Features.Solicitud.Clases;
using RRHH_WEB_API.Features.Solicitud.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace RRHH_WEB_API.Features.Solicitud
{
    public class SolicitudService
    {
        private readonly RRHH_DBContext rrhh_Web_DBContext;
        private readonly ACS_DBContext _acs_DBContext;


        private readonly DbSet<Employee> _employeeInstance;
        private readonly DbSet<UserDelegation> _userDelegationInstance;
        private readonly DbSet<UserLevel> _userLevelInstance;
        private readonly DbSet<RepositoryImage> _repositoryImageInstance;
        private readonly DbSet<Department> _departmentInstance;
        private readonly DbSet<RequestConstancia> _requestConstanciaInstance;
        private readonly DbSet<PayslipRun> _payslipRunInstance;
        private readonly DbSet<PayslipLine> _payslipLineInstance;
        private readonly DbSet<Payslip> _payslipInstance;
        private readonly DbSet<Journal> _journalInstance;
        private readonly DbSet<RequestType> _requestTypeInstance;
        private readonly DbSet<RequestState> _requestStateInstance;
        private readonly DbSet<Contract> _contractInstance;
        private readonly DbSet<RequestVacacion> _requestVacacionInstance;
        private readonly DbSet<RequestVacacionTracking> _requestVacacionTrackingInstance;
        private readonly DbSet<AutorizacionDeduccionPlanilla> _autorizacionDeduccionPlanillaInstance;
        private readonly DbSet<AutorizacionDeduccionPlanillaEstado> _autorizacionDeduccionPlanillaEstadoInstance;
        private readonly DbSet<BenefitDeduction> _benefitDeductionInstance;
        private readonly DbSet<Feriado> _feriadoInstance;
        private readonly DbSet<RequestItem> _requestItemInstance;
        private readonly DbSet<RequestConstanciaItem> _requestConstanciaItemInstance;
        private readonly DbSet<PeriodoVacacion> _peridoVacacionInstance;
       

        private readonly DbSet<TipoVacacion> _tipoVacacionesInstance;

        private IConfiguration _configuration;

        private EmailConfiguration _emailConfiguration = new EmailConfiguration();
        private EmailSendParams _emailSendParams = new EmailSendParams();
        const int depreciacionConceptoId = 9876;

        public SolicitudService(RRHH_DBContext rrhh_DBContext, IConfiguration configuration, ACS_DBContext acs_DBContext)
        {
            rrhh_Web_DBContext = rrhh_DBContext;
            _acs_DBContext = acs_DBContext;


            _employeeInstance = rrhh_DBContext.Employee;
            _userDelegationInstance = _acs_DBContext.UserDelegation;
            _userLevelInstance = _acs_DBContext.UserLevel;
            _repositoryImageInstance = _acs_DBContext.RepositoryImage;
            _departmentInstance = rrhh_DBContext.Department;
            _requestConstanciaInstance = _acs_DBContext.RequestConstancia;
            _payslipRunInstance = rrhh_DBContext.PayslipRun;
            _payslipLineInstance = rrhh_DBContext.PayslipLine;
            _payslipInstance = rrhh_DBContext.Payslip;
            _journalInstance = rrhh_DBContext.Journal;
            _requestTypeInstance = _acs_DBContext.RequestType;
            _requestStateInstance = _acs_DBContext.RequestState;
            _requestVacacionInstance = _acs_DBContext.RequestVacacion;
            _requestVacacionTrackingInstance = _acs_DBContext.RequestVacacionTracking;
            _contractInstance = rrhh_DBContext.Contract;
            _autorizacionDeduccionPlanillaInstance = acs_DBContext.AutorizacionDeduccionPlanilla;
            _autorizacionDeduccionPlanillaEstadoInstance = acs_DBContext.AutorizacionDeduccionPlanillaEstado;
            _benefitDeductionInstance = rrhh_DBContext.BenefitDeduction;
            _feriadoInstance = acs_DBContext.Feriado;
            _requestItemInstance = _acs_DBContext.RequestItem;
            _requestConstanciaItemInstance = _acs_DBContext.RequestConstanciaItem;
            _tipoVacacionesInstance = _acs_DBContext.TipoVacaciones;
            _peridoVacacionInstance = _acs_DBContext.PeriodoVacacion;

            _configuration = configuration;


            _emailConfiguration.Port = this._configuration.GetValue<int>("Smtp:Port");
            _emailConfiguration.SmtpServer = this._configuration.GetValue<string>("Smtp:Server");
            _emailConfiguration.UserName = this._configuration.GetValue<string>("Smtp:UserName");
            _emailConfiguration.Password = this._configuration.GetValue<string>("Smtp:Password");
            _emailConfiguration.From = this._configuration.GetValue<string>("Smtp:FromAddress");
            _emailConfiguration.DisplayName = this._configuration.GetValue<string>("Smtp:DisplayName");
            _emailConfiguration.RRHHEmail = this._configuration.GetValue<string>("Smtp:RRHH_Mail");

            _acs_DBContext = acs_DBContext;
        }


        public List<int> ObtenerEmpleadosACargo(int empleadoId)
        {
            List<int> empleadosACargo = _employeeInstance.AsQueryable().AsNoTracking()
                .Where(x => x.ParentId == empleadoId).Select(x => x.Id).ToList();
            return empleadosACargo;
        }

        private ValidarVacacionDto InicializarDiasVacacion(ValidarVacacionDto validarVacacion, List<Feriado> feriadosDelAnioActual)
        {
            if (validarVacacion.FechaInicio > validarVacacion.FechaFin)
            {
                validarVacacion.FechaFin = validarVacacion.FechaInicio;
            }
            else if (validarVacacion.FechaFin < validarVacacion.FechaInicio)
            {
                validarVacacion.FechaInicio = validarVacacion.FechaFin;
            }

            const decimal medioDia = (decimal)0.5;
            bool cantidadEsMenorAUno = validarVacacion.CantidadDiasVacacion > 0 && validarVacacion.CantidadDiasVacacion < 1;
            bool cantidadEsUno = validarVacacion.CantidadDiasVacacion == 1;
            bool cantidadEsMayorADos = validarVacacion.CantidadDiasVacacion > 2;
            List<Feriado> feriadosEncontrados = feriadosDelAnioActual.Where(x => validarVacacion.FechaInicio.Date <= x.FechaFin.Date && validarVacacion.FechaFin.Date >= x.FechaInicio.Date).ToList();
            decimal sumaDiasFeriado = feriadosEncontrados.Sum(x => x.CantidadDias);
            bool cantidadEsDecimal = ((validarVacacion.CantidadDiasVacacion + sumaDiasFeriado) % 1) != 0;

            if (validarVacacion.TipoVerificacion == TipoVerificacionEnum.PorDias)
            {

                if (cantidadEsMenorAUno)
                {
                    validarVacacion.CantidadDiasVacacion = medioDia;
                    validarVacacion.FechaFin = validarVacacion.FechaInicio.Date;
                }
                else
                {
                    validarVacacion.FechaFin = validarVacacion.FechaInicio.Date.AddDays((int)Math.Round(validarVacacion.CantidadDiasVacacion + sumaDiasFeriado, MidpointRounding.AwayFromZero) - 1);
                    validarVacacion.FechaFin = VerificarFecha(validarVacacion.FechaFin.Date, feriadosDelAnioActual, true);
                }

                if (cantidadEsDecimal)
                    validarVacacion.FechaReintegro = (validarVacacion.Jornada == Jornada.Mañana) ? VerificarFecha(validarVacacion.FechaFin.Date, feriadosDelAnioActual) : VerificarFecha(validarVacacion.FechaFin.Date.AddDays(1), feriadosDelAnioActual);
                else
                    validarVacacion.FechaReintegro = VerificarFecha(validarVacacion.FechaFin.Date.AddDays(1), feriadosDelAnioActual);

            }
            else if (validarVacacion.TipoVerificacion == TipoVerificacionEnum.PorFecha)
            {

                validarVacacion.CantidadDiasVacacion = (validarVacacion.FechaFin.Date - validarVacacion.FechaInicio.Date).Days + 1;
                validarVacacion.FechaReintegro = VerificarFecha(validarVacacion.FechaFin.Date.AddDays(1), feriadosDelAnioActual);
            }

            return validarVacacion;
        }

        //private DateTime VerificarFecha(DateTime fecha, List<Feriado> feriadosDelAnioActual)
        //{
        //    bool esDiaFeriado = feriadosDelAnioActual.Any(x => x.FechaInicio.Date == fecha.Date);
        //    DateTime fechaReingreso = fecha;

        //    if (esDiaFeriado)
        //    {
        //        fecha = fecha.Date.AddDays(1);
        //        fechaReingreso = VerificarFecha(fecha.Date, feriadosDelAnioActual);
        //    }

        //    return fechaReingreso;
        //}

        private DateTime VerificarFecha(DateTime fecha, List<Feriado> feriadosDelAnioActual, bool feriadoCompleto = false)
        {
            decimal valor;
            valor = (feriadoCompleto) ? 1 : (decimal)0.5;

            bool esDiaFeriado = feriadosDelAnioActual.Any(x => x.FechaInicio.Date == fecha.Date && x.CantidadDias == 1);
            //if (feriadoCompleto)
            //    esDiaFeriado = feriadosDelAnioActual.Any(x => x.FechaInicio.Date == fecha.Date && x.CantidadDias == 1);
            //else
            //    esDiaFeriado = feriadosDelAnioActual.Any(x => x.FechaInicio.Date == fecha.Date);

            DateTime fechaReingreso = fecha;
            if (esDiaFeriado || fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                fecha = fecha.Date.AddDays(1);
                fechaReingreso = VerificarFecha(fecha.Date, feriadosDelAnioActual);
            }

            return fechaReingreso;
        }

        private ValidarVacacionDto DescontarDiasFeriados(ValidarVacacionDto validarVacacion, List<Feriado> feriadosDelAnioActual)
        {
            List<Feriado> feriadosEncontrados = feriadosDelAnioActual.Where(x => validarVacacion.FechaInicio.Date <= x.FechaFin.Date && validarVacacion.FechaFin.Date >= x.FechaInicio.Date).ToList();
            bool cantidadEsDecimal = (validarVacacion.CantidadDiasVacacion % 1) != 0;
            bool sumaDiasFeriadosEsDecimal = (feriadosEncontrados.Sum(x => x.CantidadDias) % 1) != 0;
            bool cantidadDiasVacacionYSumaDiasFeriadosSonDecimal = cantidadEsDecimal && sumaDiasFeriadosEsDecimal;


            if (cantidadDiasVacacionYSumaDiasFeriadosSonDecimal == false)
                validarVacacion.CantidadDiasVacacion -= feriadosEncontrados.Sum(x => x.CantidadDias);


            //if (cantidadEsDecimal && sumaDiasFeriadosEsDecimal)
            //    validarVacacion.CantidadDiasVacacion = validarVacacion.CantidadDiasVacacion;
            //else
            //    validarVacacion.CantidadDiasVacacion -= feriadosEncontrados.Sum(x => x.CantidadDias);

            validarVacacion.CantidadDiasVacacion = (validarVacacion.CantidadDiasVacacion == 0) ? -1 : validarVacacion.CantidadDiasVacacion;
            return validarVacacion;
        }

        private ValidarVacacionDto DescontarDomingos(ValidarVacacionDto validarVacacion, List<Feriado> feriadosDelAnioActual)
        {
            List<DateTime> listadoFechas = Enumerable.Range(0, 1 + validarVacacion.FechaFin
                                            .Subtract(validarVacacion.FechaInicio).Days)
                                            .Select(incremento => validarVacacion.FechaInicio.AddDays(incremento))
                                            .ToList();

            List<DateTime> listadoFeriadosDomingos = feriadosDelAnioActual
                                                            .Where(x => listadoFechas.Contains(x.FechaInicio) && x.FechaInicio.DayOfWeek == DayOfWeek.Sunday)
                                                            .Select(x => x.FechaInicio)
                                                            .ToList();

            List<DateTime> listadoFechasDomingos = listadoFechas.Where(fecha => fecha.DayOfWeek == DayOfWeek.Sunday).ToList();
            int domingosARestar = listadoFechasDomingos.Count() - listadoFeriadosDomingos.Count();


            validarVacacion.CantidadDiasVacacion = ((validarVacacion.CantidadDiasVacacion == 0) ? -1 : validarVacacion.CantidadDiasVacacion) - domingosARestar;
            return validarVacacion;
        }

        private decimal ObtenerTasaDeCambio(int employeeId)
        {
            PayslipRun payslipRun = _payslipRunInstance.AsQueryable().AsNoTracking()
                                        .Where(x => x.Payslip.EmployeeId == employeeId)
                                        .OrderByDescending(x => x.Id)
                                        .FirstOrDefault();

            return (payslipRun.IsNotNull() && payslipRun.CurrencId == 2) ? payslipRun.Rate : 1;
        }

        private ConstanciaTrabajoIngresoDeduccionDto ObtenerSalarioOrdinario(int employeeId, decimal tasa)
        {
            try
            {
                var salarioOrdinario = _contractInstance.AsQueryable().AsNoTracking()
                                        .FirstOrDefault(d => d.EmployeeId == employeeId && d.State != "close" && d.Active).Wage;

                ConstanciaTrabajoIngresoDeduccionDto ingreso = new ConstanciaTrabajoIngresoDeduccionDto
                {
                    Monto = (salarioOrdinario ?? 0),
                    Nombre = "Salario Ordinario"
                };

                return ingreso;
            }
            catch (Exception ex)
            {
                return new ConstanciaTrabajoIngresoDeduccionDto();
            }
        }

        private ConstanciaTrabajoIngresoDeduccionDto ObtenerOtrosIngresos(int employeeId, decimal tasa)
        {
            try
            {
                List<int> conceptosAIncluir = new List<int> { 1, 2 };

                decimal? otrosIngresos = _benefitDeductionInstance.AsQueryable().AsNoTracking()
                                        .Where(d => conceptosAIncluir.Contains(d.ConceptId ?? 0) && d.Contract.EmployeeId == employeeId && d.Type == "pi" && d.Contract.Active && d.Contract.State != "close" && d.Active == true)
                                        .Sum(f => f.Value);

                List<int> cantidadHoras = new List<int> { 81, 64, 65, 63 };
                List<int> TotalLineas = new List<int> { 82, 68, 69, 70, 86, 71 };


                var detalleHoras = _payslipRunInstance.AsQueryable().AsNoTracking()
                                   .Where(p => p.Payslip.EmployeeId == employeeId && p.Payslip.PayslipLine.Code == "TPHE").ToList();


                List<decimal> ultimosRegistrosDetalleHoras = new List<decimal>();
                decimal promedioHoras = 0;

                if (detalleHoras.Count>0)
                {
                     ultimosRegistrosDetalleHoras = (from dh in detalleHoras orderby dh.DateStart descending
                                join ps in _payslipInstance.AsQueryable().AsNoTracking().Where(b=> b.EmployeeId== employeeId)
                                on dh.Id equals ps.PayslipRunId
                                join psl in _payslipLineInstance.AsQueryable().AsNoTracking().Where(r=> TotalLineas.Contains(r.SalaryRuleId) && r.Code== "TPHE")
                                on ps.Id equals psl.PayslipId into tablas_join
                                from result in tablas_join.DefaultIfEmpty()
                                select result.Amount == null ? 0 : result.Amount ?? 0
                               ).Take(2).ToList();

                    promedioHoras = ultimosRegistrosDetalleHoras.Sum() / 2;
                }

                ConstanciaTrabajoIngresoDeduccionDto ingreso = new ConstanciaTrabajoIngresoDeduccionDto
                {
                    Monto = ((otrosIngresos ?? 0) + promedioHoras) / tasa,
                    Nombre = "Otros Ingresos (Horas Extras, Transporte, etc)"
                };

                return ingreso;
            }
            catch (Exception ex)
            {
                return new ConstanciaTrabajoIngresoDeduccionDto();

            }
        }

        private List<ConstanciaTrabajoIngresoDeduccionDto> ObtenerDeducciones(int employeeId, int solicitudConstanciaId, decimal tasa)
        {
            try
            {
                List<int> conceptosAExcluir = new List<int> { 1, 2, 3, 14, 17, 18, 20, 22, 383, 9 };

                var deducciones = from rci in _requestConstanciaItemInstance.AsQueryable().AsNoTracking()
                                    .Where(r => r.RequestConstanciaId == solicitudConstanciaId && r.RequestItemId != depreciacionConceptoId)
                                  join d in _benefitDeductionInstance.AsQueryable().AsNoTracking()
                                    .Where(d => d.Contract.EmployeeId == employeeId && d.Type == "pi" && d.Contract.Active && d.Contract.State != "close" && d.Active == true && !conceptosAExcluir.Contains(d.Id))
                                  on rci.RequestItemId equals d.Concept.Id into gd
                                  from d_rci in gd.DefaultIfEmpty()
                                  select new ConstanciaTrabajoIngresoDeduccionDto
                                  {
                                      Id = rci.RequestItemId,
                                      Nombre = rci.Name,
                                      Monto = ((rci.Value == 0) ? d_rci.Value ?? 0 : rci.Value) / tasa
                                  };

                foreach (var deduccion in deducciones)
                {
                    int deduccionCafeteria = 4;
                    if (deduccion.Id == deduccionCafeteria)
                    {
                        decimal? deduccionPorCafeteria = _payslipLineInstance.Where(x => x.EmployeId == employeeId && x.Active && x.Code == "CAF" && x.SalaryRuleId == 50).OrderByDescending(x => x.Id).Take(2).Sum(x => x.Amount);
                        deduccion.Monto = deduccionPorCafeteria ?? 0;
                    }
                }

                string sql = "EXEC dbo.usp_GetDeductions @employee_id ="+employeeId;

                var deductions = rrhh_Web_DBContext.Deductions
                .FromSqlRaw(sql) .ToList().Select(x=> new ConstanciaTrabajoIngresoDeduccionDto { 
                    Id=x.Id,
                    Nombre=x.Nombre,
                    Monto=x.Monto
                });

                return deductions.ToList();
            }
            catch (Exception ex)
            {
                return new List<ConstanciaTrabajoIngresoDeduccionDto>();
            }
        }

        #region Constancia

        public Response<List<ConceptoDto>> ObtenerConceptosConfigurables()
        {
            try
            {
                var conceptos = _requestItemInstance.AsQueryable().AsNoTracking()
                            .Select(x => new ConceptoDto
                            {
                                Id = x.Id,
                                Text = x.Name,
                                Valor = 0,
                                Moneda = 0
                            }).ToList();

                return Response<List<ConceptoDto>>.Success(conceptos);
            }
            catch (Exception ex)
            {
                return Response<List<ConceptoDto>>.Excepcion(ex.Message);
            };
        }
        public Response<List<SolicitudContanciaDto>> ObtenerSolicitudesDeConstanciasPorEmpleadoId(int empleadoId)
        {
            try
            {

                //==============Nueva Logica con la Migracion
                var empleado = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(x => x.Id == empleadoId);
                var constancias = _requestConstanciaInstance.AsQueryable().AsNoTracking()
                    .Where(x => x.Enable && x.EmployeeId == empleadoId);

                List<SolicitudContanciaDto> solicitudesDeConstancias = constancias
                    .Select( constancias => new SolicitudContanciaDto
                    {
                        Id = constancias.Id,
                        EmployeeName = empleado.Name,
                        EmployeeId = constancias.EmployeeId,
                        RequestTypeId = constancias.ResquestTypeId,
                        RequestType = constancias.RequestType.Name,
                        RequestStateId = constancias.RequestStateId,
                        RequestState = constancias.RequestState.Name,
                        Comment = constancias.Comment,
                        CreatedDate = constancias.CreatedDate
                    }).OrderByDescending(y => y.CreatedDate).ToList();
                //=================Fin Nueva Logica con la Migracion    


                //==============Lógica Anterior a la Migracion
                //List<SolicitudContanciaDto> solicitudesDeConstancias = _requestConstanciaInstance.AsQueryable().AsNoTracking()
                //    .Where(x => x.Enable && x.EmployeeId == empleadoId)
                //    .Select(x => new SolicitudContanciaDto
                //    {
                //        Id = x.Id,
                //        EmployeeName = x.Employee.Name,
                //        EmployeeId = x.EmployeeId,
                //        RequestTypeId = x.ResquestTypeId,
                //        RequestType = x.RequestType.Name,
                //        RequestStateId = x.RequestStateId,
                //        RequestState = x.RequestState.Name,
                //        Comment = x.Comment,
                //        CreatedDate = x.CreatedDate
                //    }).OrderByDescending(y => y.CreatedDate).ToList();
                //=================Fin Lógica Anterior a la Migracion

                return Response<List<SolicitudContanciaDto>>.Success(solicitudesDeConstancias);
            }
            catch (Exception ex)
            {
                return Response<List<SolicitudContanciaDto>>.Excepcion(ex.Message);
            }
        }

        public Response<List<SolicitudContanciaDto>> GuardarSolicitudDeConstancia(int empleadoId, int tipoConstanciaId)
        {
            try
            {
                List<SolicitudContanciaDto> solicitudesDeConstancias = ObtenerSolicitudesDeConstanciasPorEmpleadoId(empleadoId).Data;

                bool haySolicitudesConstanciaPendientes = solicitudesDeConstancias
                    .Where(x => x.RequestTypeId == tipoConstanciaId
                    && x.RequestStateId == (int)EstadoSolicitudEnum.EnProceso).Any();

                if (haySolicitudesConstanciaPendientes)
                {
                    return Response<List<SolicitudContanciaDto>>.Validation("Aún tiene una solicitud 'En Proceso'.");
                }

                RequestConstancia solicitudConstancia = new RequestConstancia
                {
                    EmployeeId = empleadoId,
                    ResquestTypeId = tipoConstanciaId
                };

                _requestConstanciaInstance.Add(solicitudConstancia);
                _acs_DBContext.SaveChanges();

                ///Correo a el solicitante
                var mail = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == solicitudConstancia.EmployeeId).WorkEmail;
                var name = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == solicitudConstancia.EmployeeId).Name;

                if (!string.IsNullOrEmpty(mail))
                {

                    _emailSendParams.Destinatarios = new List<string>();

                    _emailSendParams.Subject = "Se ha creado una solicitud de Constancia";
                    _emailSendParams.Body = "<p>Estimado(a) " + name + ", reciba un cordial saludo,</p>" +
                                            "<p>Se ha creado una solicitud de constancia, se le estará notificando el avance de la misma<p>" +
                                            "<p>Saludos,<p>";

                    //_emailSendParams.Destinatarios.Add(mail ?? "");
                    _emailSendParams.Destinatarios.Add("reuceda05@hotmail.com");

                    EnviarCorreo(_emailSendParams);
                }
                else
                {
                    _emailSendParams.Destinatarios = new List<string>();

                    _emailSendParams.Subject = "Se ha creado una solicitud de Constancia";
                    _emailSendParams.Body = "<p>Srs(as) de RRHH, <strong>" + name + "</strong> ha creado una solicitud de constancia.</p>" +
                                            "<p>Sin embargo no tiene configurada una dirección de correo, por tanto se ha redirigido el correo a este dirección</p>" +
                                             "<p>Saludos,</p>";

                    _emailSendParams.Destinatarios.Add(_emailConfiguration.RRHHEmail);
                    //_emailSendParams.Destinatarios.Add("reuceda05@hotmail.com");


                    EnviarCorreo(_emailSendParams);
                }

                //Correo a RRHH
                _emailSendParams.Subject = "Se ha creado una solicitud de Constancia";
                _emailSendParams.Body = "<p>Srs(as) de RRHH, <strong>" + name + "</strong> ha creado una solicitud de constancia,</p>" +
                                        "<p>Usted puede acceder al portal de <a href='http://10.50.11.32:82/#/auth/login'>Stand RRHH<a> para poder gestionar su solicituda</p>" +
                                        "<p>Saludos,</p>";

                _emailSendParams.Destinatarios = new List<string> { _emailConfiguration.RRHHEmail };
                //_emailSendParams.Destinatarios.Add("reuceda05@hotmail.com");

                EnviarCorreo(_emailSendParams);


                return ObtenerSolicitudesDeConstanciasPorEmpleadoId(empleadoId);
            }
            catch (Exception e)
            {
                rrhh_Web_DBContext.Database.RollbackTransaction();
                return Response<List<SolicitudContanciaDto>>.Excepcion(e.Message);
            }
        }

        public Response<List<SolicitudContanciaDto>> EliminarSolicitudDeConstancia(int empleadoId, int solicitudId)
        {
            try
            {
                RequestConstancia solicitud = _requestConstanciaInstance.AsQueryable().Where(x => x.Id == solicitudId).FirstOrDefault();
                solicitud.Enable = false;

                _acs_DBContext.RequestConstancia.Update(solicitud);
                _acs_DBContext.SaveChanges();

                return ObtenerSolicitudesDeConstanciasPorEmpleadoId(empleadoId);
            }
            catch (Exception e)
            {
                return Response<List<SolicitudContanciaDto>>.Excepcion(e.Message);
            }
        }

        public Response<List<SolicitudContanciaDto>> ObtenerSolicitudesDeConstanciasPorEstadoId(int empleadoId, int estadoId = 0)
        {
            try
            {
                Employee empleado = _employeeInstance.AsQueryable().AsNoTracking()
                                        //.Include(x => x.UserDelegation)
                                        .FirstOrDefault(x => x.Id == empleadoId);
                empleado.UserDelegation = _userDelegationInstance.FirstOrDefault(f=> f.EmployeeId == empleadoId);

                bool esVistaRRHHAdministrador = empleado.EsEmpleadoRRHHAdministrador();
                bool esAdministrador = empleado.EsEmpleadoAdministrador();

                if (!esAdministrador)
                {
                    return Response<List<SolicitudContanciaDto>>.Excepcion("No tiene permisos para visualizar ésta información.");
                }

                List<int> empleadosACargo = ObtenerEmpleadosACargo(empleadoId);
                IEnumerable<SolicitudContanciaDto> solicitudes = _requestConstanciaInstance.AsQueryable().AsNoTracking()
                                                                    .Where(x => x.Enable == true
                                                                            && x.RequestStateId != (int)EstadoSolicitudEnum.Denegado                                                                            )
                                                                    .Select(x => new SolicitudContanciaDto
                                                                    {
                                                                        Id = x.Id,
                                                                        //EmployeeName = x.Employee.Name,//Codigo viejo
                                                                        EmployeeName = empleado.Name,//Codigo Nuevo
                                                                        EmployeeId = x.EmployeeId,
                                                                        RequestTypeId = x.ResquestTypeId,
                                                                        RequestType = x.RequestType.Name,
                                                                        RequestStateId = x.RequestStateId,
                                                                        RequestState = x.RequestState.Name,
                                                                        Comment = x.Comment,
                                                                        CreatedDate = x.CreatedDate,
                                                                        EsVistaRRHHAdministrador = esVistaRRHHAdministrador
                                                                    }).OrderBy(x => x.CreatedDate).ToList();

                bool consultarSolicitudesFiltradasPorEstado = estadoId != 0;
                if (consultarSolicitudesFiltradasPorEstado)
                {
                    solicitudes = solicitudes.Where(x => x.RequestStateId == estadoId);
                }

                return Response<List<SolicitudContanciaDto>>.Success(solicitudes.ToList());
            }
            catch (Exception ex)
            {
                return Response<List<SolicitudContanciaDto>>.Excepcion(ex.Message);
            }
        }

        public Response<List<SolicitudContanciaDto>> CambiarEstadoSolicitudDeConstancia(int empleadoId, CambioEstadoSolicitudDto cambioEstadoSolicitudDto)
        {
            try
            {
                rrhh_Web_DBContext.Database.BeginTransaction();


                RequestConstancia solicitud = _requestConstanciaInstance.AsQueryable().Where(x => x.Id == cambioEstadoSolicitudDto.SolicitudId).FirstOrDefault();
                solicitud.Comment = cambioEstadoSolicitudDto.Comentario ?? "";
                solicitud.RequestStateId = cambioEstadoSolicitudDto.EstadoId;
                _acs_DBContext.RequestConstancia.Update(solicitud);
                rrhh_Web_DBContext.SaveChanges();

                if (cambioEstadoSolicitudDto.EstadoId == (int)EstadoSolicitudEnum.Aprobado)
                {
                    foreach (var c in cambioEstadoSolicitudDto.Conceptos)
                    {
                        RequestConstanciaItem requestConstanciaItem = new RequestConstanciaItem
                        {
                            RequestConstanciaId = cambioEstadoSolicitudDto.SolicitudId,
                            RequestItemId = c.Id,
                            Name = c.Text,
                            Value = c.Valor,
                            Moneda = c.Moneda

                        };
                        _requestConstanciaItemInstance.Add(requestConstanciaItem);
                    }


                    rrhh_Web_DBContext.SaveChanges();
                }

                rrhh_Web_DBContext.Database.CommitTransaction();

                ///Correo a el solicitante
                var mail = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == solicitud.EmployeeId).WorkEmail;
                var name = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == solicitud.EmployeeId).Name;
                var estado = _requestStateInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == solicitud.RequestStateId).Name;

                if (!string.IsNullOrEmpty(mail))
                {

                    _emailSendParams.Destinatarios = new List<string>();

                    _emailSendParams.Subject = "Su solicitud de constancia ha cambiado de estado";
                    _emailSendParams.Body = "<p>Estimado(a) " + name + ", reciba un cordial saludo,</p>" +
                                            "<p>Le notificamos que su solicitud de constancia ha cambiado a <strong>" + estado + "</strong><p>" +
                                            "<p>Saludos,<p>";
                    _emailSendParams.Destinatarios.Add(mail ?? "");

                    EnviarCorreo(_emailSendParams);
                }
                else
                {
                    _emailSendParams.Destinatarios = new List<string>();

                    _emailSendParams.Subject = "Su solicitud de constancia ha cambiado de estado";
                    _emailSendParams.Body = @"<p>Srs(as) de RRHH, <strong>" + name + "</strong> ha creado una solicitud de constancia. La cual ha cambiado de estado a <strong>" + estado + "</strong></p>" +
                                            "<p>Sin embargo no tiene configurada una dirección de correo, por tanto se ha redirigido el correo a este dirección</p>" +
                                             "<p>Saludos,</p>";
                    //+"<p>Saludos,</p>";
                    _emailSendParams.Destinatarios.Add(_emailConfiguration.RRHHEmail);

                    EnviarCorreo(_emailSendParams);
                }

                return ObtenerSolicitudesDeConstanciasPorEstadoId(empleadoId);
            }
            catch (Exception e)
            {
                return Response<List<SolicitudContanciaDto>>.Excepcion(e.Message);
            }
        }

        public Response<ConstanciaTrabajoDto> ObtenerConstanciaParaImpresion(int empleadoId, int solicitudConstanciaId)
        {
            try
            {
                RequestConstancia constancia = _requestConstanciaInstance.AsQueryable().Where(p => p.Id == solicitudConstanciaId).FirstOrDefault();
                int empleadoIdConstancia = constancia.EmployeeId;

                Employee empleado = _employeeInstance.AsQueryable().AsNoTracking().Include(x => x.UserDelegation).FirstOrDefault(x => x.Id == empleadoId);
                if (!empleado.EsEmpleadoRRHHAdministrador())
                {
                    return Response<ConstanciaTrabajoDto>.Validation("No tiene permiso para realizar esta acción");
                }

                ConstanciaTrabajoDto constanciaTrabajo = _employeeInstance.AsQueryable().AsNoTracking()
                    .Where(y => y.Id == empleadoIdConstancia)
                    .Select(x => new ConstanciaTrabajoDto
                    {
                        TipoConstanciaId = constancia.ResquestTypeId,
                        Moneda = x.Contract.TypeId == 2 ? "USD" : "L",
                        Employee = x.Name,
                        IdentificationId = x.IdentificationId,
                        Department = x.Department.Name,
                        Job = x.Job.Name,
                        FechaIngreso = x.Contract.DateStart,
                        Ingresos = new List<ConstanciaTrabajoIngresoDeduccionDto>(),
                        Deducciones = new List<ConstanciaTrabajoIngresoDeduccionDto>(),
                    }).FirstOrDefault();

                DateTimeFormatInfo spanishformat = new CultureInfo("es-ES", false).DateTimeFormat;
                constanciaTrabajo.DiaIngreso = constanciaTrabajo.FechaIngreso.Day;
                constanciaTrabajo.MesIngreso = spanishformat.GetMonthName(constanciaTrabajo.FechaIngreso.Month);
                constanciaTrabajo.AnioIngreso = constanciaTrabajo.FechaIngreso.Year;
                constanciaTrabajo.DiaActual = constanciaTrabajo.CreatedDate.Day;
                constanciaTrabajo.MesActual = spanishformat.GetMonthName(constanciaTrabajo.CreatedDate.Month);
                constanciaTrabajo.AnioActual = constanciaTrabajo.CreatedDate.Year;

                decimal tasa = ObtenerTasaDeCambio(empleadoIdConstancia);
                constanciaTrabajo.Ingresos.Add(ObtenerSalarioOrdinario(empleadoIdConstancia, tasa));
                constanciaTrabajo.Ingresos.Add(ObtenerOtrosIngresos(empleadoIdConstancia, tasa));


                RequestConstanciaItem depreciacionItem = _requestConstanciaItemInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.RequestConstanciaId == solicitudConstanciaId && r.RequestItemId == depreciacionConceptoId);
                if (depreciacionItem.IsNotNull())
                {
                    ConstanciaTrabajoIngresoDeduccionDto depreciacionIngreso = new ConstanciaTrabajoIngresoDeduccionDto
                    {
                        Id = depreciacionItem.RequestItemId,
                        Nombre = depreciacionItem.Name,
                        Monto = depreciacionItem.Value
                    };

                    if (depreciacionItem.Moneda == (int)Moneda.Lempiras && tasa != 1)
                        depreciacionIngreso.Monto = depreciacionItem.Value / tasa;
                    else if (depreciacionItem.Moneda == (int)Moneda.Dolares && tasa == 1)
                        depreciacionIngreso.Monto = depreciacionItem.Value * tasa;


                    constanciaTrabajo.Ingresos.Add(depreciacionIngreso);
                }

                constanciaTrabajo.Deducciones.AddRange(ObtenerDeducciones(empleadoIdConstancia, solicitudConstanciaId, tasa));

                constanciaTrabajo.TotalIngresos = constanciaTrabajo.Ingresos.Sum(x => x.Monto);
                constanciaTrabajo.TotalDeducciones = constanciaTrabajo.Deducciones.Sum(x => x.Monto);

                constanciaTrabajo.IngresosNetos = constanciaTrabajo.TotalIngresos - constanciaTrabajo.TotalDeducciones;

                return Response<ConstanciaTrabajoDto>.Success(constanciaTrabajo);
            }
            catch (Exception e)
            {
                return Response<ConstanciaTrabajoDto>.Excepcion(e.Message);
            }
        }
        #endregion


        #region Vacacion

        public Response<ValidarVacacionDto> ValidarFechasVacacion(ValidarVacacionDto validarVacacion)
        {
            try
            {
                List<Feriado> feriadosDelAnioActual = _feriadoInstance.AsQueryable().AsNoTracking()
                                                    .Where(x => x.FechaInicio.Year == DateTime.Now.Year)
                                                    .ToList();


                validarVacacion = InicializarDiasVacacion(validarVacacion, feriadosDelAnioActual);

                validarVacacion = DescontarDiasFeriados(validarVacacion, feriadosDelAnioActual);

                validarVacacion = DescontarDomingos(validarVacacion, feriadosDelAnioActual);

                if (validarVacacion.CantidadDiasVacacion < 0)
                {
                    validarVacacion.CantidadDiasVacacion = 0;
                }

                if (validarVacacion.CantidadDiasVacacion > 20)
                {
                    return Response<ValidarVacacionDto>.Validation("El máximo de días feriados a solicitar son 20 días");
                }

                return Response<ValidarVacacionDto>.Success(validarVacacion);
            }
            catch (Exception ex)
            {
                return Response<ValidarVacacionDto>.Excepcion(ex.Message);
            }
        }

        public Response<List<SolicitudVacacionDto>> ObtenerSolicitudesDeVacacionPorEmpleadoId(int empleadoId)
        {
            try
            {
                Employee empleado = _employeeInstance.AsQueryable().AsNoTracking().Where(m => m.Id == empleadoId).Include(x => x.Parent).FirstOrDefault();

                List<SolicitudVacacionDto> solicitudes = _requestVacacionInstance.AsQueryable().AsNoTracking().Where(y => y.Enable == true && y.EmployeeId == empleadoId)
                    .Select(x => new SolicitudVacacionDto
                    {
                        Id = x.Id,
                        EmployeeId = x.EmployeeId,
                        Employee = empleado.Name,
                        RequestStateId = x.RequestStateId,
                        RequestState = x.RequestState.Name,
                        FechaInicio = x.FechaInicio,
                        FechaFin = x.FechaFin,
                        FechaReintegro = x.FechaReintegro,
                        CantidadDiasVacacion = x.CantidadDiasVacacion,
                        CreatedDate = x.CreatedDate,
                        Observaciones = x.Observaciones,
                        Comment = x.Comment,
                        CubreVacaciones = x.CubreVacaciones,
                        JefeInmediato = (empleado.Parent != null) ? (empleado.Parent.Name != null) ? empleado.Parent.Name : "" : "",
                    }).OrderByDescending(x => x.CreatedDate).ToList();


                return Response<List<SolicitudVacacionDto>>.Success(solicitudes);
            }
            catch (Exception ex)
            {
                return Response<List<SolicitudVacacionDto>>.Excepcion(ex.Message);
            }
        }

        public Response<decimal> ObtenerDiasPendientesDeVacacion(int employeeId)
        {
            try
            {
                List<int> requestStateID = new List<int> { 1,2,4,6 };

                decimal totalDiasVacacion = _peridoVacacionInstance.AsQueryable().AsNoTracking()
                                    .Where(x => x.EmployeeId == employeeId)
                                    .Sum(x => x.Days);

                decimal cantidadDiasGozados =  rrhh_Web_DBContext.Leave.AsQueryable().AsNoTracking()
                                                    .Where(x => x.EmployeeId == employeeId && x.State == "validate" && x.HolidayStatusId == 5)
                                                    .Sum(x => x.NumberOfDays);

                decimal cantidadDiasSolicitados = _acs_DBContext.RequestVacacion.AsQueryable().AsNoTracking()
                                                    .Where(x => x.EmployeeId == employeeId && x.Enable == true && x.SincronizadoEnOdoo == false && requestStateID.Contains(x.RequestStateId))
                                                    .Sum(x => x.CantidadDiasVacacion);

                decimal totalDiasGozadosYSolicitados = cantidadDiasGozados + cantidadDiasSolicitados;

                decimal totalDiasPendientesDeVacacion = totalDiasVacacion - totalDiasGozadosYSolicitados;

                return Response<decimal>.Success(totalDiasPendientesDeVacacion);
            }
            catch (Exception e)
            {
                return Response<decimal>.Excepcion(e.Message);
            }
        }

        public Response<List<SolicitudVacacionDto>> GuardarSolicitudDeVacacion(int empleadoId, NuevaSolicitudVacacionDto nuevaSolicitudVacacionDto)
        {
            try
            {

                bool coincideConOtraVacacionLeave = rrhh_Web_DBContext.Leave.AsQueryable().AsNoTracking()
                                .Any(x => x.EmployeeId == empleadoId
                                        && x.State == "validate"
                                        && x.HolidayStatusId == 5
                                        && (x.DateFrom.Date <= nuevaSolicitudVacacionDto.FechaFin.Date && nuevaSolicitudVacacionDto.FechaInicio.Date <= x.DateTo.Date));


                bool coincideConOtraVacacionRequest = _requestVacacionInstance.AsQueryable().AsNoTracking()
                                                .Any(x => x.EmployeeId == empleadoId
                                                        && x.Enable == true
                                                        && x.SincronizadoEnOdoo == false
                                                        && x.RequestStateId != (int)EstadoSolicitudEnum.RechazadoPorJefeInmediato
                                                        && x.RequestStateId != (int)EstadoSolicitudEnum.RechazadoPorRRHH
                                                        && (x.FechaInicio.Date <= nuevaSolicitudVacacionDto.FechaFin.Date && nuevaSolicitudVacacionDto.FechaInicio.Date <= x.FechaFin.Date));

                if (coincideConOtraVacacionLeave || coincideConOtraVacacionRequest)
                {
                    return Response<List<SolicitudVacacionDto>>.Validation("Las fechas seleccionadas coinciden con una vacación gozada o solicitud de vacación.");
                }

                decimal cantidadDiasPendientes = ObtenerDiasPendientesDeVacacion(empleadoId).Data;
                if (cantidadDiasPendientes <= 0)
                {
                    return Response<List<SolicitudVacacionDto>>.Validation("No tiene días pendientes de vacación.");
                }

                if (nuevaSolicitudVacacionDto.CantidadDiasVacacion > cantidadDiasPendientes)
                {
                    return Response<List<SolicitudVacacionDto>>.Validation("No tiene suficientes días pendientes de vacación.");
                }

                rrhh_Web_DBContext.Database.BeginTransaction();

                Employee empleado = _employeeInstance.AsQueryable().AsNoTracking().Where(m => m.Id == empleadoId).Include(x => x.Parent).FirstOrDefault();

                if (empleado.Parent == null)
                {
                    return Response<List<SolicitudVacacionDto>>.Validation("No se pudo completar la solicitud, debido a que no tiene configurado un jefe inmediato a nivel del sistema. Por favor ponerse en contacto con Recursos Humanos.");
                }

                //List<SolicitudVacacionDto> solicitudesDeVacaciones = ObtenerSolicitudesDeVacacionPorEmpleadoId(empleadoId).Data;

                //bool haySolicitudesVacacionPendientes = solicitudesDeVacaciones
                //    .Where(x => x.RequestStateId == (int)EstadoSolicitudEnum.EnProceso).Any();

                //if (haySolicitudesVacacionPendientes)
                //{
                //    return Response<List<SolicitudVacacionDto>>.Validation("Aún tiene una solicitud 'En Proceso'.");
                //}

                RequestVacacion solicitudVacacion = new RequestVacacion
                {
                    EmployeeId = empleadoId,
                    RequestStateId = (int)EstadoSolicitudEnum.EnProceso,
                    CantidadDiasVacacion = nuevaSolicitudVacacionDto.CantidadDiasVacacion,
                    FechaInicio = nuevaSolicitudVacacionDto.FechaInicio,
                    FechaFin = nuevaSolicitudVacacionDto.FechaFin,
                    FechaReintegro = nuevaSolicitudVacacionDto.FechaReintegro,
                    Observaciones = nuevaSolicitudVacacionDto.Observaciones,
                    CubreVacaciones = nuevaSolicitudVacacionDto.CubreVacaciones,
                    JefeInmediatoId = (empleado.ParentId != null) ? (int)empleado.ParentId : 0,
                    TipoVacacionId = nuevaSolicitudVacacionDto.TipoVacacion.Id,
                    ActividadesPendientes = nuevaSolicitudVacacionDto.ActividadesPendientes
                };

                    _requestVacacionInstance.Add(solicitudVacacion);
                    rrhh_Web_DBContext.SaveChanges();

                RequestVacacionTracking requestVacacionTracking = new RequestVacacionTracking
                {
                    RequestVacacionId = solicitudVacacion.Id,
                    Descripcion = $"La solicitud se ha creado en estado { (int)EstadoSolicitudEnum.EnProceso }"
                };

                _requestVacacionTrackingInstance.Add(requestVacacionTracking);
                rrhh_Web_DBContext.SaveChanges();
                rrhh_Web_DBContext.Database.CommitTransaction();


                ///Correo a el solicitante
                var mailSolicitante = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == solicitudVacacion.EmployeeId).WorkEmail;
                var nameSolicitante = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == solicitudVacacion.EmployeeId).Name;

                //Correo de Jefe Inmediato
                var mailJefeInmediato = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == solicitudVacacion.JefeInmediatoId)?.WorkEmail ?? "";
                var nameJefeInmediato = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == solicitudVacacion.JefeInmediatoId)?.Name ?? "";

                if (!string.IsNullOrEmpty(mailSolicitante))
                {

                    _emailSendParams.Destinatarios = new List<string>();

                    _emailSendParams.Subject = "Se ha creado una solicitud de Vacaciones";
                    _emailSendParams.Body = "<p>Estimado(a)  <strong>" + nameSolicitante + "</strong>, reciba un cordial saludo,</p>" +
                                            "<p>Se ha creado una solicitud de vacaciones, se le estará notificando el avance del mismo<p>" +
                                            "<p>Saludos,<p>";
                    _emailSendParams.Destinatarios.Add(mailSolicitante);

                    EnviarCorreo(_emailSendParams);
                }
                else
                {
                    _emailSendParams.Destinatarios = new List<string>();

                    _emailSendParams.Subject = "Se ha creado una solicitud de Vacaciones";
                    _emailSendParams.Body = "<p>Srs(as) de RRHH, <strong>" + nameSolicitante + "</strong> ha creado una solicitud de Vacaciones.</p>" +
                                            "<p>Sin embargo no tiene configurada una dirección de correo, por tanto se ha redirigido el correo a este dirección</p>" +
                                             "<p>Saludos,</p>";
                    //+"<p>Saludos,</p>";
                    _emailSendParams.Destinatarios.Add(_emailConfiguration.RRHHEmail);

                    EnviarCorreo(_emailSendParams);
                }


                if (!string.IsNullOrEmpty(mailJefeInmediato))
                {

                    //Correo a RRHH
                    _emailSendParams.Subject = "Se ha creado una solicitud";
                    _emailSendParams.Body = "<p>Estimado(a) <strong>" + nameJefeInmediato + "</strong>, " + nameSolicitante + " ha creado una solicitud de vacaciones,</p>" +
                                            "<p>Usted puede acceder al portal de <a href='http://10.50.11.32:82/#/auth/login'>Stand RRHH<a> para poder gestionar su solicitud</p>" +
                                            "<p>Saludos,</p>";
                    _emailSendParams.Destinatarios = new List<string> { mailJefeInmediato };
                    EnviarCorreo(_emailSendParams);
                }

                return ObtenerSolicitudesDeVacacionPorEmpleadoId(empleadoId);
            }
            catch (Exception e)
            {
                rrhh_Web_DBContext.Database.RollbackTransaction();
                return Response<List<SolicitudVacacionDto>>.Excepcion(e.Message);
            }
        }

        public Response<List<SolicitudVacacionDto>> EliminarSolicitudDeVacacionComoAdministrador(int empleadoId, int solicitudId)
        {
            try
            {
                rrhh_Web_DBContext.Database.BeginTransaction();

                Employee empleado = _employeeInstance.AsQueryable().AsNoTracking().Include(x => x.UserDelegation).FirstOrDefault(x => x.Id == empleadoId);
                if (!empleado.EsEmpleadoRRHHAdministrador())
                {
                    return Response<List<SolicitudVacacionDto>>.Validation("No tiene permiso para realizar esta acción");
                }
                                
                RequestVacacion solicitud = _requestVacacionInstance.AsQueryable()
                                                .Where(x => x.Id == solicitudId)
                                                .FirstOrDefault();
                solicitud.Enable = false;
                _acs_DBContext.RequestVacacion.Update(solicitud);

                RequestVacacionTracking requestVacacionTracking = new RequestVacacionTracking
                {
                    RequestVacacionId = solicitudId,
                    Descripcion = $"La solicitud ha sido eliminada como administrador por el colaborador {empleadoId}"
                };
                _requestVacacionTrackingInstance.Add(requestVacacionTracking);

                rrhh_Web_DBContext.SaveChanges();
                rrhh_Web_DBContext.Database.CommitTransaction();

                return ObtenerSolicitudesDeVacacionPorEstadoId(empleadoId);
            }
            catch (Exception e)
            {
                rrhh_Web_DBContext.Database.RollbackTransaction();
                return Response<List<SolicitudVacacionDto>>.Excepcion(e.Message);
            }
        }

        public Response<List<SolicitudVacacionDto>> SincronizarVacacionEnOdoo(int empleadoId, int solicitudId)
        {
            try
            {
                Employee empleado = _employeeInstance.AsQueryable().AsNoTracking().Include(x => x.UserDelegation).FirstOrDefault(x => x.Id == empleadoId);
                if (!empleado.EsEmpleadoRRHHAdministrador())
                {
                    return Response<List<SolicitudVacacionDto>>.Validation("No tiene permiso para realizar esta acción");
                }

                RequestVacacion solicitud = _requestVacacionInstance.AsQueryable()
                                                .Where(x => x.Id == solicitudId)
                                                .FirstOrDefault();
                solicitud.SincronizadoEnOdoo = true;

                _acs_DBContext.RequestVacacion.Update(solicitud);
                _acs_DBContext.SaveChanges();

                return ObtenerSolicitudesDeVacacionPorEstadoId(empleadoId);
            }
            catch (Exception e)
            {
                return Response<List<SolicitudVacacionDto>>.Excepcion(e.Message);
            }
        }

        public Response<List<SolicitudVacacionDto>> EliminarSolicitudDeVacacion(int empleadoId, int solicitudId)
        {
            try
            {
                rrhh_Web_DBContext.Database.BeginTransaction();
                
                RequestVacacion solicitud = _requestVacacionInstance.AsQueryable().Where(x => x.Id == solicitudId).FirstOrDefault();
                solicitud.Enable = false;
                _acs_DBContext.RequestVacacion.Update(solicitud);

                RequestVacacionTracking requestVacacionTracking = new RequestVacacionTracking
                {
                    RequestVacacionId = solicitudId,
                    Descripcion = $"La solicitud ha sido eliminada por el colaborador {empleadoId}"
                };
                _requestVacacionTrackingInstance.Add(requestVacacionTracking);

                _acs_DBContext.SaveChanges();
                _acs_DBContext.Database.CommitTransaction();

                return ObtenerSolicitudesDeVacacionPorEmpleadoId(empleadoId);
            }
            catch (Exception e)
            {
                rrhh_Web_DBContext.Database.RollbackTransaction();
                return Response<List<SolicitudVacacionDto>>.Excepcion(e.Message);
            }
        }

        public Response<List<SolicitudVacacionDto>> ObtenerSolicitudesDeVacacionPorEstadoId(int empleadoId, int estadoId = 0)
        {
            try
            {
                Employee empleado = _employeeInstance.AsQueryable().AsNoTracking()
                                        //.Include(x => x.UserDelegation)
                                        .FirstOrDefault(x => x.Id == empleadoId);

                var userDelegation = _userDelegationInstance.Where(u=> u.EmployeeId == empleadoId).FirstOrDefault();

                empleado.UserDelegation= userDelegation != null ? userDelegation : null;


                bool esVistaRRHHAdministrador = empleado.EsEmpleadoRRHHAdministrador();
                bool esAdministrador = empleado.EsEmpleadoAdministrador();

                List<int> empleadosACargo = ObtenerEmpleadosACargo(empleadoId);
                bool esVistaJefatura = empleadosACargo.Count() > 0;

                IEnumerable<SolicitudVacacionDto> solicitudes = _requestVacacionInstance.AsQueryable().AsNoTracking()
                                                                    .Where(x => x.Enable == true
                                                                            && x.SincronizadoEnOdoo == false
                                                                            && x.RequestStateId != (int)EstadoSolicitudEnum.RechazadoPorJefeInmediato
                                                                            && x.RequestStateId != (int)EstadoSolicitudEnum.RechazadoPorRRHH)
                                                                    .Select(x => new SolicitudVacacionDto
                                                                    {
                                                                        Id = x.Id,
                                                                        EmployeeId = x.EmployeeId,
                                                                        Employee = empleado.Name,
                                                                        RequestStateId = x.RequestStateId,
                                                                        RequestState = x.RequestState.Name,
                                                                        FechaInicio = x.FechaInicio,
                                                                        FechaFin = x.FechaFin,
                                                                        FechaReintegro = x.FechaReintegro,
                                                                        CantidadDiasVacacion = x.CantidadDiasVacacion,
                                                                        CreatedDate = x.CreatedDate,
                                                                        Observaciones = x.Observaciones,
                                                                        CubreVacaciones = x.CubreVacaciones,
                                                                        Comment = x.Comment,
                                                                        //JefeInmediato = (x.Employee.Parent != null) ? x.Employee.Parent.Name : "", //Codigo Antiguo
                                                                        JefeInmediato = (empleado.Parent != null) ? empleado.Parent.Name : "",//Codigo Nuevo
                                                                        //MailJefeInmediato = (x.Employee.Parent != null) ? x.Employee.Parent.WorkEmail : "", //Codigo Antiguo
                                                                        MailJefeInmediato = (empleado.Parent != null) ? empleado.Parent.WorkEmail : "", //Codigo Nuevo
                                                                        EsVistaRRHHAdministrador = esVistaRRHHAdministrador,
                                                                        EsVistaJefatura = empleadosACargo.Contains(x.EmployeeId),
                                                                        EsVistaAdministrador= esAdministrador
                                                                    });

                bool consultarSolicitudesFiltradasPorEstado = estadoId != 0;

                 

                if (consultarSolicitudesFiltradasPorEstado) //Si en la pantalla se quiere filtrar por un estado en especiico
                {
                    solicitudes = solicitudes.Where(x => x.RequestStateId == estadoId);
                }

                if (esVistaJefatura && esAdministrador)
                {
                    var listado = new List<SolicitudVacacionDto>();
                    listado = solicitudes.ToList();
                     
                    solicitudes = listado;
                }
                else if (esVistaJefatura && esVistaRRHHAdministrador == false) // si es Jefe y no es administrador
                {
                    solicitudes = solicitudes.Where(x => empleadosACargo.Contains(x.EmployeeId)); //Las solicitudes que apareceran seran las de su personal a cargo,                 
                }

                else if (esVistaJefatura == false && esVistaRRHHAdministrador)//No es Jefe pero si es admin de RRH
                {

                    //solicitudes = solicitudes.Where(x => x.RequestStateId == (int)EstadoSolicitudEnum.AprobadoPorJefeInmediato); //Las solicitudes que apareceran seran las que tienen estado 'Aprobado por jefe inmediato'
                    solicitudes = solicitudes.Where(x => x.RequestStateId == (int)EstadoSolicitudEnum.AprobadoPorJefeInmediato || x.RequestStateId == (int)EstadoSolicitudEnum.AprobadoPorRRHH || x.RequestStateId == (int)EstadoSolicitudEnum.EnProceso); //Las solicitudes que apareceran seran las que tienen estado 'Aprobado por jefe inmediato'
                                                                                                                                                                                                 //var test = solicitudes.Where(x => x.EmployeeId == 288);
                }
                else if (esVistaJefatura && esVistaRRHHAdministrador) //Si es jefe y es admin de RRHH
                {
                    var listado = new List<SolicitudVacacionDto>();                    
                    var solicitudesDelPersonalACargo = solicitudes.Where(x => empleadosACargo.Contains(x.EmployeeId)); //Se cargaran las solicutudes del personal a cargo
                    listado.AddRange(solicitudesDelPersonalACargo);
                    
                    var solicitudesAutorizadasPorJefatura = solicitudes.Where(x => x.RequestStateId == (int)EstadoSolicitudEnum.AprobadoPorJefeInmediato);  //Se cargaran todas las solicitudes con estado 'Aprobadas por jefe Inmediato'
                    listado.AddRange(solicitudesAutorizadasPorJefatura);

                    var listaSolicitudes = new List<SolicitudVacacionDto>();
                    foreach (var solicitud in listado)
                    {
                        if (listaSolicitudes.Any(x=>x.Id == solicitud.Id) == false)
                        {
                            listaSolicitudes.Add(solicitud);
                        }
                    }

                    solicitudes = listaSolicitudes;
                }

                return Response<List<SolicitudVacacionDto>>.Success(solicitudes.ToList());
            }
            catch (Exception ex)
            {
                return Response<List<SolicitudVacacionDto>>.Excepcion(ex.Message);
            }
        }

        public Response<List<SolicitudVacacionDto>> CambiarEstadoSolicitudDeVacacion(int empleadoId, CambioEstadoSolicitudDto cambioEstadoSolicitudDto)
        {
            try
            {
                rrhh_Web_DBContext.Database.BeginTransaction();

                string estadoSolicitud = _requestStateInstance.AsQueryable().AsNoTracking().Where(x => x.Id == cambioEstadoSolicitudDto.EstadoId).FirstOrDefault().Name;

                RequestVacacion solicitudVacacion = _requestVacacionInstance.AsQueryable().Where(x => x.Id == cambioEstadoSolicitudDto.SolicitudId).FirstOrDefault();
                solicitudVacacion.Comment = cambioEstadoSolicitudDto.Comentario ?? "";
                solicitudVacacion.RequestStateId = cambioEstadoSolicitudDto.EstadoId;

                RequestVacacionTracking tracking = new RequestVacacionTracking
                {
                    RequestVacacionId = cambioEstadoSolicitudDto.SolicitudId,
                    Descripcion = $"La solicitud ha cambiado a {estadoSolicitud}",
                };

                _acs_DBContext.RequestVacacion.Update(solicitudVacacion);
                _acs_DBContext.RequestVacacionTracking.Add(tracking);

                _acs_DBContext.SaveChanges();
                _acs_DBContext.Database.CommitTransaction();


                ///Correo a el solicitante
                var mailSolicitante = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == solicitudVacacion.EmployeeId).WorkEmail;
                var nameSolicitante = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == solicitudVacacion.EmployeeId).Name;

                //Correo de Jefe Inmediato
                var mailJefeInmediato = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == solicitudVacacion.JefeInmediatoId)?.WorkEmail ?? "";
                var nameJefeInmediato = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(r => r.Id == solicitudVacacion.JefeInmediatoId)?.Name ?? "";


                switch (solicitudVacacion.RequestStateId)
                {
                    case 4://Es Aprobado por Jefe Inmediato
                        if (!string.IsNullOrEmpty(mailSolicitante))
                        {

                            _emailSendParams.Destinatarios = new List<string>();

                            _emailSendParams.Subject = "Su solicitud de vacaciones ha cambiado de estado";
                            _emailSendParams.Body = "<p>Estimado(a) " + nameSolicitante + ", reciba un cordial saludo,</p>" +
                                                    "<p>Le notificamos que su jefe inmediato <strong>" + nameJefeInmediato + "</strong> ha aprobado sus solicitud de vacaciones, por tanto la solicitud ha pasado a ser revisada por RRHH<p>" +
                                                    "<p>Le estaremos informando una vez haya sido procesada<p>" +
                                                    "<p>Saludos,<p>";
                            _emailSendParams.Destinatarios.Add(mailSolicitante);

                            EnviarCorreo(_emailSendParams);
                        }
                        else
                        {
                            _emailSendParams.Destinatarios = new List<string>();

                            _emailSendParams.Subject = "Su solicitud de vacaciones ha cambiado de estado";
                            _emailSendParams.Body = "<p>Srs(as) de RRHH, <strong>" + nameSolicitante + "</strong> ha solicitado vacaciones las cuales han sido aprobadas por su jefe inmediato.</p>" +
                                                    "<p>Sin embargo no tiene configurada una dirección de correo, por tanto se ha redirigido el correo a esta dirección</p>" +
                                                     "<p>Saludos,</p>";
                            //+"<p>Saludos,</p>";
                            _emailSendParams.Destinatarios.Add(_emailConfiguration.RRHHEmail);

                            EnviarCorreo(_emailSendParams);
                        }


                        //Correo a RRHH
                        _emailSendParams.Destinatarios = new List<string>();

                        _emailSendParams.Subject = "Se ha creado una solicitud de Vacaciones";
                        _emailSendParams.Body = "<p>Srs(as) de RRHH, <strong>" + nameSolicitante + "</strong> ha creado una solicitud de Vacaciones las cuales ya han sido aprobadas por el jefe inmediato.</p>" +
                                                "<p>Sin embargo no tiene configurada una dirección de correo, por tanto se ha redirigido el correo a este dirección</p>" +
                                                 "<p>Saludos,</p>";
                        //+"<p>Saludos,</p>";
                        _emailSendParams.Destinatarios.Add(_emailConfiguration.RRHHEmail);

                        EnviarCorreo(_emailSendParams);
                        break;

                    case 5://Es Aprobado por RRHH
                        if (!string.IsNullOrEmpty(mailSolicitante))
                        {

                            _emailSendParams.Destinatarios = new List<string>();

                            _emailSendParams.Subject = "Su solicitud de vacaciones ha cambiado de estado";
                            _emailSendParams.Body = "<p>Estimado(a) " + nameSolicitante + ", reciba un cordial saludo,</p>" +
                                                    "<p>Le notificamos que <strong>RRHH</strong> ha aprobado su solicitud de vacaciones<p>" +
                                                    "<p>Le deseamos que tenga unas excelentes vacaciones!<p>" +
                                                    "<p>Saludos,<p>";
                            _emailSendParams.Destinatarios.Add(mailSolicitante);

                            EnviarCorreo(_emailSendParams);
                        }
                        else
                        {
                            _emailSendParams.Destinatarios = new List<string>();

                            _emailSendParams.Subject = "Su solicitud de vacaciones ha cambiado de estado";
                            _emailSendParams.Body = "<p>Srs(as) de RRHH, <strong>" + nameSolicitante + "</strong> ha solicitado vacaciones las cuales ya han sido aprobas.</p>" +
                                                    "<p>Sin embargo no tiene configurada una dirección de correo, por tanto se ha redirigido el correo a esta dirección</p>" +
                                                     "<p>Saludos,</p>";
                            //+"<p>Saludos,</p>";
                            _emailSendParams.Destinatarios.Add(_emailConfiguration.RRHHEmail);

                            EnviarCorreo(_emailSendParams);
                        }
                        break;

                    case 6://Es Aprobadas por RRHH
                        if (!string.IsNullOrEmpty(mailSolicitante))
                        {

                            _emailSendParams.Destinatarios = new List<string>();

                            _emailSendParams.Subject = "Su solicitud de vacaciones ha cambiado de estado";
                            _emailSendParams.Body = "<p>Estimado(a) " + nameSolicitante + ", reciba un cordial saludo,</p>" +
                                                    "<p>Le notificamos que <strong>RRHH</strong> ha Aprobado su solicitud de vacaciones<p>" +
                                                    "<p>Saludos,<p>";
                            _emailSendParams.Destinatarios.Add(mailSolicitante);

                            EnviarCorreo(_emailSendParams);
                        }
                        else
                        {
                            _emailSendParams.Destinatarios = new List<string>();

                            _emailSendParams.Subject = "Su solicitud de vacaciones ha cambiado de estado";
                            _emailSendParams.Body = "<p>Srs(as) de RRHH, <strong>" + nameSolicitante + "</strong> ha solicitado vacaciones las cuales ya han sido aprobadas.</p>" +
                                                    "<p>Sin embargo no tiene configurada una dirección de correo, por tanto se ha redirigido el correo a esta dirección</p>" +
                                                     "<p>Saludos,</p>";
                            //+"<p>Saludos,</p>";
                            _emailSendParams.Destinatarios.Add(_emailConfiguration.RRHHEmail);

                            EnviarCorreo(_emailSendParams);
                        }
                        break;

                    case 7://Es Rechazada por Jefe Inmediato
                        if (!string.IsNullOrEmpty(mailSolicitante))
                        {

                            _emailSendParams.Destinatarios = new List<string>();

                            _emailSendParams.Subject = "Su solicitud de vacaciones ha cambiado de estado";
                            _emailSendParams.Body = "<p>Estimado(a) " + nameSolicitante + ", reciba un cordial saludo,</p>" +
                                                    "<p>Le notificamos que su jefe inmediato <strong>" + nameJefeInmediato + "</strong> ha rechazado su solicitud vacaciones<p>" +
                                                    //"<p>Le estaremos informando una vez haya sido procesada<p>" +
                                                    "<p>Saludos,<p>";
                            _emailSendParams.Destinatarios.Add(mailSolicitante);

                            EnviarCorreo(_emailSendParams);
                        }
                        else
                        {
                            _emailSendParams.Destinatarios = new List<string>();

                            _emailSendParams.Subject = "Su solicitud de vacaciones ha cambiado de estado";
                            _emailSendParams.Body = "<p>Srs(as) de RRHH, <strong>" + nameSolicitante + "</strong> ha solicitado vacaciones las cuales han sido rechazadas por su jefe inmediato.</p>" +
                                                    "<p>Sin embargo no tiene configurada una dirección de correo, por tanto se ha redirigido el correo a esta dirección</p>" +
                                                     "<p>Saludos,</p>";
                            //+"<p>Saludos,</p>";
                            _emailSendParams.Destinatarios.Add(_emailConfiguration.RRHHEmail);

                            EnviarCorreo(_emailSendParams);
                        }
                        break;
                }

                //if (!string.IsNullOrEmpty(mailSolicitante))
                //{

                //    _emailSendParams.Destinatarios = new List<string>();

                //    _emailSendParams.Subject = "Se ha creado una solicitud de Vacaciones";
                //    _emailSendParams.Body = "<p>Estimado(a) " + nameSolicitante + ", reciba un cordial saludo,</p>" +
                //                            "<p>Se ha creado una solicitud de vacaciones, se le estará notificando el avance del mismo<p>" +
                //                            "<p>Saludos,<p>";
                //    _emailSendParams.Destinatarios.Add(mailSolicitante);

                //    EnviarCorreo(_emailSendParams);
                //}
                //else
                //{
                //    _emailSendParams.Destinatarios = new List<string>();

                //    _emailSendParams.Subject = "Se ha creado una solicitud de Constancia";
                //    _emailSendParams.Body = "<p>Srs(as) de RRHH, <strong>" + nameSolicitante + "</strong> ha creado una solicitud de Vacaciones.</p>" +
                //                            "<p>Sin embargo no tiene configurada una dirección de correo, por tanto se ha redirigido el correo a este dirección</p>" +
                //                             "<p>Saludos,</p>";
                //    //+"<p>Saludos,</p>";
                //    _emailSendParams.Destinatarios.Add(_emailConfiguration.RRHHEmail);

                //    EnviarCorreo(_emailSendParams);
                //}


                //if (!string.IsNullOrEmpty(mailJefeInmediato))
                //{

                //    //Correo a RRHH
                //    _emailSendParams.Subject = "Se ha creado una solicitud de Constancia";
                //    _emailSendParams.Body = "<p>Estimado(a) <strong>" + nameJefeInmediato + "</strong>, " + nameSolicitante + " ha creado una solicitud de vacaciones,</p>" +
                //                            "<p>Usted puede acceder al portal de <a href='http://10.50.11.32:82/#/auth/login'>Stand RRHH<a> para poder gestionar su solicitud</p>" +
                //                            "<p>Saludos,</p>";
                //    _emailSendParams.Destinatarios = new List<string> { mailJefeInmediato };
                //    EnviarCorreo(_emailSendParams);
                //}


                return ObtenerSolicitudesDeVacacionPorEstadoId(empleadoId);
            }
            catch (Exception e)
            {
                rrhh_Web_DBContext.Database.RollbackTransaction();
                return Response<List<SolicitudVacacionDto>>.Excepcion(e.Message);
            }
        }

        public Response<VacacionDto> ObtenerVacacionParaImpresion(int empleadoId, int solicitudVacacionId)
        {
            try
            {
                DateTimeFormatInfo spanishformat = new CultureInfo("es-ES", false).DateTimeFormat;
                spanishformat.LongDatePattern = "dddd, dd 'de' MMMM 'del' yyyy";
                string format = "dddd, dd 'de' MMMM 'del' yyyy";
                ConvertirNumeroALetras numeroALetras = new ConvertirNumeroALetras();

                VacacionDto vacacion = _requestVacacionInstance.AsQueryable().AsNoTracking()
                    .Where(y => y.Id == solicitudVacacionId)
                    .Select(x => new VacacionDto
                    {
                        EmployeeId = x.EmployeeId,
                        Barcode = x.Employee.BarCode,
                        Employee = x.Employee.Name,
                        Job = x.Employee.Job.Name,
                        Department = x.Employee.Department.Name,
                        FechaIngreso = x.Employee.Contract.DateStart.ToString(format, new CultureInfo("es-ES")),
                        CantidadDiasVacacion = x.CantidadDiasVacacion,
                        CantidadDiasVacacionEnLetras =  x.CantidadDiasVacacion==1? numeroALetras.NumeroALetras(Convert.ToInt32(x.CantidadDiasVacacion))+" día": numeroALetras.NumeroALetras(Convert.ToInt32(x.CantidadDiasVacacion)) + " días",
                        //FechaInicio = x.FechaInicio.ToString(format, new CultureInfo("es-ES")),
                        FechaInicio = x.FechaInicio.ToString("dd/MM/yyyy"),
                        //FechaFin = x.FechaFin.ToString(format, new CultureInfo("es-ES")),
                        FechaFin = x.FechaFin.ToString("dd/MM/yyyy"),
                        Observaciones = x.Observaciones,
                        CubreVacaciones = x.CubreVacaciones,
                        //FechaReintegro = x.FechaReintegro.ToString(format, new CultureInfo("es-ES")),
                        FechaReintegro = x.FechaReintegro.ToString("dd/MM/yyyy"),
                        JefeInmediatoId = x.JefeInmediatoId,
                        TipoVacacionId = x.TipoVacacionId,
                        TipoVacacionName="",
                        ActividadesPendientes = x.ActividadesPendientes
                    }).FirstOrDefault();
                //vacacion.Observaciones = "is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry'sss";
                //Convert.ToDateTime(x.FechaInicio, spanishformat).ToLongDateString()
                if (vacacion.JefeInmediatoId > 0)
                {
                    vacacion.JefeInmediato = _employeeInstance.AsQueryable().FirstOrDefault(p => p.Id == vacacion.JefeInmediatoId).Name;
                }

                return Response<VacacionDto>.Success(vacacion);
            }
            catch (Exception ex)
            {
                return Response<VacacionDto>.Excepcion(ex.Message);
            }
        }

        public Response<List<TipoVacacionDto>> ObtenerTipoVacaciones()
        {
            try
            {
                var tipoVacaciones = _tipoVacacionesInstance.AsNoTracking().Select(x => new TipoVacacionDto{
                    Id = x.Id,
                    Descripcion = x.Descripcion
                }).ToList();

                return Response<List<TipoVacacionDto>>.Success(tipoVacaciones);
            }
            catch (Exception exc)
            {
                return Response<List<TipoVacacionDto>>.Excepcion(exc.Message);
            }
            
        }
        #endregion


        private void EnviarCorreo(EmailSendParams emailSendParams)
        {

            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(_emailConfiguration.From, _emailConfiguration.DisplayName);

            foreach (var item in emailSendParams.Destinatarios)
            {
                message.To.Add(item);
            }

            //message.To.Add("reuceda05@hotmail.com");
            message.Subject = emailSendParams.Subject;
            message.Body = emailSendParams.Body;
            //message.Body = "<p>Estimado(a) " + nombre + ", reciba un cordial saludo,</p> <p>Se adjunta el archivo de horas trabajas que usted solicitó</p> ";
            message.IsBodyHtml = true;

            smtp.EnableSsl = true;
            smtp.Port = _emailConfiguration.Port;
            smtp.Host = _emailConfiguration.SmtpServer;
            //smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential(_emailConfiguration.UserName, _emailConfiguration.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtp.Send(message);

        }

    }
}
