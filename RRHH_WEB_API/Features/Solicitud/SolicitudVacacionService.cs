using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeDetective.Storage.Xml.v2;
using RRHH_WEB_API._Common;
using RRHH_WEB_API._Entidades;

using RRHH_WEB_API._Infraestructura;
using RRHH_WEB_API.Features.Solicitud.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace RRHH_WEB_API.Features.Solicitud
{
    public class SolicitudVacacionService
    {
        private readonly RRHH_DBContext _rrhh_dbContext;
        private readonly ACS_DBContext _acs_dbContext;

        private readonly SolicitudService _solicitudService;
        private readonly DbSet< RequestVacacion> _solicitudVacaciones;
        private readonly DbSet< Employee> _empleados;
        private const decimal medioDia = (decimal)0.5;
        private const decimal diaCompleto = 1;



        public SolicitudVacacionService(RRHH_DBContext rrhh_DBContext, SolicitudService solicitudService, ACS_DBContext acs_DBCotext)
        {
            _rrhh_dbContext = rrhh_DBContext;
            _acs_dbContext = acs_DBCotext;

            _solicitudService = solicitudService;

            _solicitudVacaciones = _acs_dbContext.RequestVacacion;
            _empleados = _rrhh_dbContext.Employee;
        }

        private ValidarVacacionDto InicializarVacacion(ValidarVacacionDto validarVacacion, List<Feriado> feriadosDelAnioActual)
        {

            if (validarVacacion.FechaFin < validarVacacion.FechaInicio)
                validarVacacion.FechaInicio = validarVacacion.FechaFin;

            if (validarVacacion.CantidadDiasVacacion < 0)
                validarVacacion.CantidadDiasVacacion = 0;

            if (validarVacacion.CantidadDiasVacacion < 1)
                validarVacacion.CantidadDiasVacacion = medioDia;

            validarVacacion.FechaReintegro = validarVacacion.FechaFin;

            if (validarVacacion.TipoVerificacion == TipoVerificacionEnum.PorDias || validarVacacion.TipoVerificacion == TipoVerificacionEnum.PorJornada)
            {
                validarVacacion.FechaFin = validarVacacion.FechaInicio.Date.AddDays((int)Math.Round(validarVacacion.CantidadDiasVacacion * 2, MidpointRounding.AwayFromZero) - 1);
            }

            List<DateTime> listadoFechas = Enumerable.Range(0, 1 + validarVacacion.FechaFin
                                            .Subtract(validarVacacion.FechaInicio).Days)
                                            .Select(incremento => validarVacacion.FechaInicio.AddDays(incremento))
                                            .ToList();

            validarVacacion.ListadoFechas = listadoFechas;

            validarVacacion.ListadoFechasVacacion = new List<FechaVacacion>();
            foreach (DateTime fecha in listadoFechas)
            {
                Feriado fechaFeriado = feriadosDelAnioActual.FirstOrDefault(x => fecha == x.FechaInicio);

                FechaVacacion fechaVacacion = new FechaVacacion
                {
                    Fecha = fecha,
                    CantidadDiaFeriado = (fechaFeriado.IsNotNull()) ? fechaFeriado.CantidadDias : 0
                };

                fechaVacacion.CantidadDiaVacacion = (fechaVacacion.CantidadDiaFeriado == (decimal)0.5) ? (decimal)0.5 : (fechaVacacion.CantidadDiaFeriado == 1) ? 0 : (fechaVacacion.CantidadDiaFeriado == 0) ? 1 : 1;
                fechaVacacion.EsDiaCompleto = fechaVacacion.CantidadDiaVacacion == 1;
                fechaVacacion.EsFeriadoCompleto = fechaVacacion.CantidadDiaFeriado == 1;
                fechaVacacion.EsMedioFeriado = fechaVacacion.CantidadDiaFeriado == (decimal)0.5;
                fechaVacacion.EsSabado = fechaVacacion.Fecha.DayOfWeek == DayOfWeek.Saturday;
                fechaVacacion.EsDomingo = fechaVacacion.Fecha.DayOfWeek == DayOfWeek.Sunday;
            
                validarVacacion.ListadoFechasVacacion.Add(fechaVacacion);
            }

            if (validarVacacion.TipoVerificacion == TipoVerificacionEnum.PorDias || validarVacacion.TipoVerificacion == TipoVerificacionEnum.PorJornada)
            {
                DescontarDomingos(validarVacacion);

                DescontarDiasFeriados(validarVacacion, feriadosDelAnioActual);

                if (validarVacacion.CantidadDiasVacacion <= 3)
                    DescontarSabados(validarVacacion);

                List<FechaVacacion> fechasVacacion = new List<FechaVacacion>();
                decimal sumaDiasVacacion = 0;
                foreach (FechaVacacion fecha in validarVacacion.ListadoFechasVacacion)
                { 
                    sumaDiasVacacion += fecha.CantidadDiaVacacion;
                    if (sumaDiasVacacion > (int)Math.Round(validarVacacion.CantidadDiasVacacion, MidpointRounding.AwayFromZero))
                    {
                        break;
                    }
                    
                    validarVacacion.FechaFin = fecha.Fecha;
                    validarVacacion.FechaReintegro = fecha.Fecha;
                    fechasVacacion.Add(fecha);
                }
                validarVacacion.ListadoFechasVacacion = fechasVacacion;
            }

            return validarVacacion;
        }

        private ValidarVacacionDto DescontarDomingos(ValidarVacacionDto validarVacacion)
        { 
            validarVacacion.ListadoFechasVacacion = validarVacacion.ListadoFechasVacacion.Where(x=> !x.EsDomingo).ToList();

            return validarVacacion;
        }

        private ValidarVacacionDto DescontarDiasFeriados(ValidarVacacionDto validarVacacion, List<Feriado> feriadosDelAnioActual)
        {
            validarVacacion.ListadoFechasVacacion = validarVacacion.ListadoFechasVacacion.Where(x => !x.EsFeriadoCompleto).ToList();

            return validarVacacion;
        }

        private ValidarVacacionDto DescontarSabados(ValidarVacacionDto validarVacacion)
        {
            validarVacacion.ListadoFechasVacacion = validarVacacion.ListadoFechasVacacion.Where(x => !x.EsSabado).ToList();

            return validarVacacion;
        }

        public Response<ValidarVacacionDto> ValidarFechasVacacion(ValidarVacacionDto validarVacacion)
        {
            try
            {
                List<Feriado> feriadosDelAnioActual = _acs_dbContext.Feriado.AsQueryable().AsNoTracking()
                                                        .Where(x => x.FechaInicio.Year == DateTime.Now.Year)
                                                        .ToList();

                InicializarVacacion(validarVacacion, feriadosDelAnioActual);

                DescontarDomingos(validarVacacion);

                DescontarDiasFeriados(validarVacacion, feriadosDelAnioActual);

                if (validarVacacion.ListadoFechasVacacion.Sum(x => x.CantidadDiaVacacion) <= 3)
                    DescontarSabados(validarVacacion);

                VerificarFechaReintegro(validarVacacion, feriadosDelAnioActual);
                if (validarVacacion.CantidadDiasVacacion.EsDecimal() && validarVacacion.ListadoFechasVacacion.Sum(x => x.CantidadDiaVacacion).EsEntero())
                {
                    validarVacacion.Jornada = Jornada.Mañana;
                }
                else
                {
                    validarVacacion.Jornada = Jornada.Ocultar;
                }

                if (validarVacacion.TipoVerificacion == TipoVerificacionEnum.PorFecha ||
                    validarVacacion.TipoVerificacion == TipoVerificacionEnum.PorDias && validarVacacion.ListadoFechasVacacion.Sum(x => x.CantidadDiaVacacion).EsDecimal())
                {
                    validarVacacion.CantidadDiasVacacion = validarVacacion.ListadoFechasVacacion.Sum(x => x.CantidadDiaVacacion);
                }
                
                if ((validarVacacion.TipoVerificacion == TipoVerificacionEnum.PorFecha || validarVacacion.TipoVerificacion == TipoVerificacionEnum.PorDias) && 
                    validarVacacion.ListadoFechasVacacion.Sum(x => x.CantidadDiaVacacion).EsDecimal())
                {
                    validarVacacion.FechaReintegro.AddDays(1);
                }

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
        bool validado;
        private ValidarVacacionDto VerificarFechaReintegro(ValidarVacacionDto validarVacacion, List<Feriado> feriadosDelAnioActual)
        {
            bool fechaReingresoEsFeriadoCompleto = feriadosDelAnioActual
                                            .Any(f => f.FechaInicio == validarVacacion.FechaReintegro && f.CantidadDias == diaCompleto);

            bool fechaReingresoEsFeriado = feriadosDelAnioActual
                                            .Any(f => f.FechaInicio == validarVacacion.FechaReintegro);

            bool fecheFinalEsMedioFeriado = feriadosDelAnioActual
                                            .Any(f => f.FechaInicio == validarVacacion.FechaFin && f.CantidadDias == medioDia);

            bool fechaReingresoEsMedioFeriado = feriadosDelAnioActual
                                            .Any(f => f.FechaInicio == validarVacacion.FechaReintegro.AddDays(1) && f.CantidadDias == medioDia);

            bool hayFeriadosDeMedioDiaEnRangoDeFechas = feriadosDelAnioActual
                                                        .Any(x => x.CantidadDias.EsDecimal() && (x.FechaInicio.Date <= validarVacacion.FechaFin.Date && validarVacacion.FechaInicio.Date <= x.FechaFin.Date));

            bool sumaDiasEsNumeroEntero = (validarVacacion.ListadoFechasVacacion.Sum(x => x.CantidadDiaVacacion) % 1) == 0;
            
            bool cantidadEsNumeroEntero = (validarVacacion.CantidadDiasVacacion % 1) == 0;

            bool cantidadValida = false;
           
            
            
            if (validarVacacion.TipoVerificacion == TipoVerificacionEnum.PorFecha)
                cantidadValida = sumaDiasEsNumeroEntero && validarVacacion.FechaReintegro == validarVacacion.FechaFin;
            else if (validarVacacion.TipoVerificacion == TipoVerificacionEnum.PorDias)
                cantidadValida = cantidadEsNumeroEntero && validarVacacion.FechaReintegro == validarVacacion.FechaFin;

            if (cantidadValida)
            {
                validarVacacion.FechaReintegro = validarVacacion.FechaReintegro.AddDays(1);
                VerificarFechaReintegro(validarVacacion, feriadosDelAnioActual);
            }
            else if (validarVacacion.ListadoFechasVacacion.Sum(x => x.CantidadDiaVacacion).EsDecimal() && hayFeriadosDeMedioDiaEnRangoDeFechas && validado == false)
            {
                validarVacacion.FechaReintegro = validarVacacion.FechaReintegro.AddDays(1);
                validado = true;
                VerificarFechaReintegro(validarVacacion, feriadosDelAnioActual);
            }
            else if (fechaReingresoEsFeriadoCompleto)
            {
                validarVacacion.FechaReintegro = validarVacacion.FechaReintegro.AddDays(1);
                VerificarFechaReintegro(validarVacacion, feriadosDelAnioActual);
            }            
            else if (!cantidadEsNumeroEntero && validarVacacion.Jornada == Jornada.Ocultar && feriadosDelAnioActual.Any(x => x.FechaInicio.Date == validarVacacion.FechaReintegro && x.CantidadDias.EsEntero()))
            {
                validarVacacion.FechaReintegro = validarVacacion.FechaReintegro.AddDays(1);
                VerificarFechaReintegro(validarVacacion, feriadosDelAnioActual);
            }
            else if (validarVacacion.FechaReintegro.DayOfWeek == DayOfWeek.Sunday)
            {
                validarVacacion.FechaReintegro = validarVacacion.FechaReintegro.AddDays(1);
                VerificarFechaReintegro(validarVacacion, feriadosDelAnioActual);
            }

            //Validar el fin de semana
            if (validarVacacion.FechaReintegro.DayOfWeek == DayOfWeek.Saturday)
            {
                validarVacacion.FechaReintegro=validarVacacion.FechaReintegro.AddDays(2); // Si es sábado, ajustar al lunes siguiente
            }
            else if (validarVacacion.FechaReintegro.DayOfWeek == DayOfWeek.Sunday)
            {
                validarVacacion.FechaReintegro=validarVacacion.FechaReintegro.AddDays(1); // Si es domingo, ajustar al lunes siguiente
            }

                return validarVacacion;
        }
                
        public Response<List<SolicitudVacacionDto>> ObtenerSolicitudesDeVacacionPorEmpleadoId(int empleadoId)
        {
            try
            {
                Employee empleado = _rrhh_dbContext.Employee.AsQueryable().AsNoTracking().Where(m => m.Id == empleadoId).Include(x => x.Parent).FirstOrDefault();

                List<SolicitudVacacionDto> solicitudes = _solicitudVacaciones.AsQueryable().AsNoTracking().Where(y => y.Enable == true && y.EmployeeId == empleadoId)
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

        public Response<List<SolicitudVacacionDto>> GuardarSolicitudDeVacacion(int empleadoId, NuevaSolicitudVacacionDto nuevaSolicitudVacacionDto)
        {
            try
            {
                _rrhh_dbContext.Database.BeginTransaction();

                Employee empleado = _rrhh_dbContext.Employee.AsQueryable().AsNoTracking().Where(m => m.Id == empleadoId).Include(x => x.Parent).FirstOrDefault();

                if (empleado.Parent == null)
                {

                }

                List<SolicitudVacacionDto> solicitudesDeVacaciones = ObtenerSolicitudesDeVacacionPorEmpleadoId(empleadoId).Data;

                bool haySolicitudesVacacionPendientes = solicitudesDeVacaciones
                    .Where(x => x.RequestStateId == (int)EstadoSolicitudEnum.EnProceso).Any();

                if (haySolicitudesVacacionPendientes)
                {
                    return Response<List<SolicitudVacacionDto>>.Validation("Aún tiene una solicitud 'En Proceso'.");
                }

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
                    JefeInmediatoId = (empleado.Parent.ParentId != null) ? (int)empleado.Parent.ParentId : 0,
                };

               _solicitudVacaciones.Add(solicitudVacacion);
                _acs_dbContext.SaveChanges();

                RequestVacacionTracking requestVacacionTracking = new RequestVacacionTracking
                {
                    RequestVacacionId = solicitudVacacion.Id,
                    Descripcion = $"La solicitud se ha creado en estado { (int)EstadoSolicitudEnum.EnProceso }"
                };

                _acs_dbContext.RequestVacacionTracking.Add(requestVacacionTracking);
                _acs_dbContext.SaveChanges();
                _acs_dbContext.Database.CommitTransaction();

                return ObtenerSolicitudesDeVacacionPorEmpleadoId(empleadoId);
            }
            catch (Exception e)
            {
                _rrhh_dbContext.Database.RollbackTransaction();
                return Response<List<SolicitudVacacionDto>>.Excepcion(e.Message);
            }
        }

        public Response<List<SolicitudVacacionDto>> EliminarSolicitudDeVacacion(int empleadoId, int solicitudId)
        {
            try
            {
                RequestVacacion solicitud = _acs_dbContext.RequestVacacion.AsQueryable().Where(x => x.Id == solicitudId).FirstOrDefault();
                solicitud.Enable = false;

                _acs_dbContext.RequestVacacion.Update(solicitud);
                _acs_dbContext.SaveChanges();

                return ObtenerSolicitudesDeVacacionPorEmpleadoId(empleadoId);
            }
            catch (Exception e)
            {
                return Response<List<SolicitudVacacionDto>>.Excepcion(e.Message);
            }
        }
        public Response<List<SolicitudVacacionDto>> ObtenerSolicitudesDeVacacionPorEstadoId(int empleadoId, int estadoId = 0)
        {
            try
            {


                List<RequestVacacion> solicitudesDeVacaciones = _acs_dbContext.RequestVacacion.AsQueryable().AsNoTracking()
                    //.Include(x => x.Employee).ThenInclude(x => x.Parent)
                    .Include(x => x.RequestState)
                    .Where(x => x.Enable).ToList();

                bool consultarSolicitudesFiltradasPorEstado = estadoId != 0;
                if (consultarSolicitudesFiltradasPorEstado)
                {
                    solicitudesDeVacaciones = solicitudesDeVacaciones.Where(x => x.RequestStateId == estadoId).ToList();
                }

                Employee empleado = _rrhh_dbContext.Employee.AsQueryable().AsNoTracking().Include(x => x.UserDelegation).FirstOrDefault(x => x.Id == empleadoId);
               
                List<int> empleadosACargo = _solicitudService.ObtenerEmpleadosACargo(empleadoId);
                if (empleado.EsEmpleadoNormal())
                {
                    solicitudesDeVacaciones = solicitudesDeVacaciones.Where(x => empleadosACargo.Contains(x.EmployeeId)).ToList();
                }

                List<SolicitudVacacionDto> solicitudes = solicitudesDeVacaciones
                    .Select(x => new SolicitudVacacionDto
                    {
                        Id = x.Id,
                        EmployeeId = x.EmployeeId,
                        Employee = _empleados.AsQueryable().AsNoTracking().FirstOrDefault (f=>f.Id ==x.EmployeeId).Name,
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
                        JefeInmediato = (_empleados.AsQueryable().AsNoTracking().FirstOrDefault(f => f.Id == x.EmployeeId).Parent != null) ? _empleados.AsQueryable().AsNoTracking().FirstOrDefault(f => f.Id == x.EmployeeId).Parent.Name : "",
                        MailJefeInmediato = (_empleados.AsQueryable().AsNoTracking().FirstOrDefault(f => f.Id == x.EmployeeId).Parent != null) ? _empleados.AsQueryable().AsNoTracking().FirstOrDefault(f => f.Id == x.EmployeeId).Parent.WorkEmail : "",
                        EsVistaRRHHAdministrador = empleado.EsEmpleadoRRHHAdministrador(),
                        EsVistaJefatura = empleado.EsEmpleadoNormal(),
                    }).OrderByDescending(x => x.CreatedDate).ToList();

                return Response<List<SolicitudVacacionDto>>.Success(solicitudes);
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
                _rrhh_dbContext.Database.BeginTransaction();

                string estadoSolicitud = _acs_dbContext.RequestState.AsQueryable().AsNoTracking().Where(x => x.Id == cambioEstadoSolicitudDto.EstadoId).FirstOrDefault().Name;

                RequestVacacion solicitudVacacion = _acs_dbContext.RequestVacacion.AsQueryable().Where(x => x.Id == cambioEstadoSolicitudDto.SolicitudId).FirstOrDefault();
                solicitudVacacion.Comment = cambioEstadoSolicitudDto.Comentario ?? "";
                solicitudVacacion.RequestStateId = cambioEstadoSolicitudDto.EstadoId;

                RequestVacacionTracking tracking = new RequestVacacionTracking
                {
                    RequestVacacionId = cambioEstadoSolicitudDto.SolicitudId,
                    Descripcion = $"La solicitud ha cambiado a {estadoSolicitud}",
                };

                _acs_dbContext.RequestVacacion.Update(solicitudVacacion);
                _acs_dbContext.RequestVacacionTracking.Add(tracking);

                _acs_dbContext.SaveChanges();
                _acs_dbContext.Database.CommitTransaction();

                return ObtenerSolicitudesDeVacacionPorEstadoId(empleadoId);
            }
            catch (Exception e)
            {
                _rrhh_dbContext.Database.RollbackTransaction();
                return Response<List<SolicitudVacacionDto>>.Excepcion(e.Message);
            }
        }

        public Response<VacacionDto> ObtenerVacacionParaImpresion(int empleadoId, int solicitudVacacionId)
        {
            try
            {
                Employee empleado = _rrhh_dbContext.Employee.AsQueryable().AsNoTracking().Include(x => x.UserDelegation).FirstOrDefault(x => x.Id == empleadoId);
                
                if (!empleado.EsEmpleadoRRHHAdministrador())
                {
                    return Response<VacacionDto>.Excepcion("No tiene permiso para imprimir");
                }

                DateTimeFormatInfo spanishformat = new CultureInfo("es-ES", false).DateTimeFormat;
                spanishformat.LongDatePattern = "dddd, dd 'de' MMMM 'del' yyyy";
                string format = "dddd, dd 'de' MMMM 'del' yyyy";

                VacacionDto vacacion = _acs_dbContext.RequestVacacion.AsQueryable().AsNoTracking()
                    .Where(y => y.Id == solicitudVacacionId)
                    .Select(x => new VacacionDto
                    {
                        EmployeeId = x.Id,
                        Employee = _empleados.AsQueryable().AsNoTracking().FirstOrDefault(f => f.Id == x.EmployeeId).Name,
                        Job = _empleados.AsQueryable().AsNoTracking().FirstOrDefault(f => f.Id == x.EmployeeId).Job.Name,
                        Department = _empleados.AsQueryable().AsNoTracking().FirstOrDefault(f => f.Id == x.EmployeeId).Department.Name,
                        FechaIngreso = _empleados.AsQueryable().AsNoTracking().FirstOrDefault(f => f.Id == x.EmployeeId).Contract.DateStart.Value.ToString(format, new CultureInfo("es-ES")),
                        CantidadDiasVacacion = x.CantidadDiasVacacion,
                        FechaInicio = x.FechaInicio.ToString(format, new CultureInfo("es-ES")),
                        FechaFin = x.FechaFin.ToString(format, new CultureInfo("es-ES")),
                        Observaciones = x.Observaciones,
                        CubreVacaciones = x.CubreVacaciones,
                        FechaReintegro = x.FechaReintegro.ToString(format, new CultureInfo("es-ES")),
                        JefeInmediatoId = x.JefeInmediatoId
                    }).FirstOrDefault();
                vacacion.Observaciones = "is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry'sss";

                if (vacacion.JefeInmediatoId > 0)
                {
                    vacacion.JefeInmediato = _rrhh_dbContext.Employee.AsQueryable().FirstOrDefault(p => p.Id == vacacion.JefeInmediatoId).Name;
                }

                return Response<VacacionDto>.Success(vacacion);
            }
            catch (Exception ex)
            {
                return Response<VacacionDto>.Excepcion(ex.Message);
            }
        }

    }
}
