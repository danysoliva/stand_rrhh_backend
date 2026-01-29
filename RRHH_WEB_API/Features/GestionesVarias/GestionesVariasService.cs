using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RRHH_WEB_API._Common;
using RRHH_WEB_API._Entidades;
using RRHH_WEB_API._Entidades.QuejasSugerenciasDenuncias;
using RRHH_WEB_API._Infraestructura;
using RRHH_WEB_API.Features.GestionesVarias.Dto;
using RRHH_WEB_API.Features.GestionesVarias.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.GestionesVarias
{
    public class GestionesVariasService
    {
        private readonly RRHH_Web_DBContext rrhh_Web_DBContext;
        private readonly DbSet<AutorizacionDeduccionPlanilla> _autorizacionDeduccionPlanillasInstance;
        private readonly DbSet<AutorizacionDeduccionPlanillaEstado> _autorizacionDeduccionPlanillasEstadoInstance;
        private readonly DbSet<Employee> _employeeInstance;
        private readonly DbSet<Department> _departmentInstance;
        private readonly DbSet<PlazaVacante> _plazaVacanteInstance;
        private readonly DbSet<PlazaVacanteAdjunto> _plazaVacanteAdjuntoInstance;
        private readonly DbSet<PlazaVacantePostulante> _plazaVacantePostulanteInstance;
        private readonly DbSet<QuejaSugerenciaDenuncia> _quejaSugerenciaDenunciaInstance;
        private readonly DbSet<QuejaSugerenciaDenunciaState> _quejaSugerenciaStateInstance;
        private readonly DbSet<QuejaSugerenciaDenunciaType> _quejaSugerenciaTypeInstance;
        private readonly DbSet<PayslipLine> _payslipLineInstance;
        private readonly DbSet<PayslipRun> _payslipRunInstance;
        private readonly DbSet<Contract> _contractInstance;
        private readonly IWebHostEnvironment _enviroment;


        public GestionesVariasService(RRHH_Web_DBContext rrhh_DBContext, IWebHostEnvironment environment)
        {
            rrhh_Web_DBContext = rrhh_DBContext;
            _autorizacionDeduccionPlanillasInstance = rrhh_DBContext.AutorizacionDeduccionPlanilla;
            _autorizacionDeduccionPlanillasEstadoInstance = rrhh_DBContext.AutorizacionDeduccionPlanillaEstado;
            _employeeInstance = rrhh_DBContext.Employee;
            _departmentInstance = rrhh_DBContext.Department;
            _plazaVacanteInstance = rrhh_DBContext.PlazaVacantes;
            _plazaVacanteAdjuntoInstance = rrhh_DBContext.PlazaVacanteAdjuntos;
            _plazaVacantePostulanteInstance = rrhh_DBContext.PlazaVacantePostulantes;
            _quejaSugerenciaDenunciaInstance = rrhh_DBContext.QuejaSugerenciaDenuncia;
            _quejaSugerenciaStateInstance = rrhh_DBContext.QuejaSugerenciaDenunciaState;
            _quejaSugerenciaTypeInstance = rrhh_DBContext.QuejaSugerenciaDenunciaType;
            _contractInstance = rrhh_DBContext.Contract;
            _payslipRunInstance = rrhh_DBContext.PayslipRun;
            _payslipLineInstance = rrhh_DBContext.PayslipLine;
            _enviroment = environment;

        }

        #region Deducciones

        public Response<bool> GuardarDeduccion(ParamsDeduccionPlanillaDto paramsDeduccionplanilla)
        {
            try
            {
                AutorizacionDeduccionPlanilla deduccionPlanilla = new AutorizacionDeduccionPlanilla();


                if (paramsDeduccionplanilla.FechaDeduccion != null)
                {

                    var tasa = ObtenerTasaDeCambio(paramsDeduccionplanilla.EmployeeId);

                    deduccionPlanilla.EmployeeId = paramsDeduccionplanilla.EmployeeId;
                    deduccionPlanilla.Monto = paramsDeduccionplanilla.Monto;
                    deduccionPlanilla.FechaDeduccion = Convert.ToDateTime(paramsDeduccionplanilla.FechaDeduccion);
                    deduccionPlanilla.FechaCreacion = DateTime.Now;
                    deduccionPlanilla.Enable = true;
                    deduccionPlanilla.EstadoId = 1;
                    deduccionPlanilla.UsuarioCreacionId = paramsDeduccionplanilla.UsuarioCreacionId;
                    deduccionPlanilla.Concepto = paramsDeduccionplanilla.Concepto;
                    deduccionPlanilla.Currency = paramsDeduccionplanilla.Currency;
                    deduccionPlanilla.TasaCambio = tasa;

                    _autorizacionDeduccionPlanillasInstance.Add(deduccionPlanilla);
                    rrhh_Web_DBContext.SaveChanges();

                    return Response<bool>.Success(true);
                }
                else
                {
                    return Response<bool>.Success(false);

                }
            }
            catch (Exception ex)
            {
                return Response<bool>.Validation(ex.Message);
            }
        }

        public Response<List<EmpleadoDto>> GetEmployees()
        {
            try
            {

                var empleados = _employeeInstance.AsQueryable().AsNoTracking()
                    .Where(r => r.Resource.Active == true)
                    .Select(x => new EmpleadoDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Barcode = x.BarCode
                    }).ToList();


                return Response<List<EmpleadoDto>>.Success(empleados);
            }
            catch (Exception ex)
            {
                return Response<List<EmpleadoDto>>.Validation(ex.Message);
            }
        }


        public Response<List<DeduccionDto>> ObtenerDeducciones()
        {
            try
            {

                var deducciones = _autorizacionDeduccionPlanillasInstance.AsQueryable().AsNoTracking().Where(f => f.Enable == true)
                    .Select(x => new DeduccionDto
                    {
                        Id = x.Id,
                        NombreEmpleado = x.Empleado.Name,
                        Barcode = x.Empleado.BarCode,
                        FechaDeduccion = x.FechaDeduccion,
                        FechaCreacion = x.FechaCreacion,
                        Concepto = x.Concepto,
                        Estado = x.EstadoDeduccionPorPlanilla.Descripcion,
                        Monto = x.Monto,
                        Currency = x.Currency
                    }).OrderByDescending(c => c.Id).ToList();


                return Response<List<DeduccionDto>>.Success(deducciones);
            }
            catch (Exception ex)
            {
                return Response<List<DeduccionDto>>.Validation(ex.Message);
            }
        }



        public Response<DeduccionNewFormatDto> ImprimirFormatoDeduccionPorPlanilla(int deduccion_id)
        {
            try
            {

                var deduccion = _autorizacionDeduccionPlanillasInstance.AsQueryable().AsNoTracking()
                    .Where(t => t.Id == deduccion_id)
                    .Select(x => new DeduccionNewFormatDto
                    {
                        Id = x.Id,
                        NombreEmpleado = x.Empleado.Name,
                        Barcode = x.Empleado.BarCode,
                        FechaDeduccion = x.FechaDeduccion,
                        FechaCreacion = x.FechaCreacion,
                        Identidad = x.Empleado.IdentificationId,
                        Concepto = x.Concepto,
                        Currency=x.Currency==null ? "" :x.Currency,
                        Estado = x.EstadoDeduccionPorPlanilla.Descripcion,
                        Monto = x.Monto,
                       FechaIngreso=x.Empleado.Contract.DateStart
                    }).OrderByDescending(c => c.Id).FirstOrDefault();

                if (deduccion != null)
                {
                    var deduccionRegistro = _autorizacionDeduccionPlanillasInstance.AsQueryable().AsNoTracking()
                    .Where(t => t.Id == deduccion_id).FirstOrDefault();

                    if (deduccionRegistro.EstadoId != 2)
                    {
                        deduccionRegistro.EstadoId = 2;
                        _autorizacionDeduccionPlanillasInstance.Update(deduccionRegistro);
                        rrhh_Web_DBContext.SaveChanges();

                    }

                    return Response<DeduccionNewFormatDto>.Success(deduccion);
                }
                else
                {
                    return Response<DeduccionNewFormatDto>.Validation("No se encontro documento");
                }
            }
            catch (Exception ex)
            {
                return Response<DeduccionNewFormatDto>.Validation(ex.Message);
            }
        }

        private decimal ObtenerTasaDeCambio(int employeeId)
        {
            PayslipRun payslipRun = _payslipRunInstance.AsQueryable().AsNoTracking()
                                        .Where(x => x.Payslip.EmployeeId == employeeId)
                                        .OrderByDescending(x => x.Id)
                                        .FirstOrDefault();

            return (payslipRun.IsNotNull() && payslipRun.CurrencId == 2) ? payslipRun.Rate : 1;
        }

        public Response<List<DeduccionDto>> EliminarDeduccion(int deduccionId)
        {
            try
            {

                var deduccion = _autorizacionDeduccionPlanillasInstance.AsQueryable().AsNoTracking().FirstOrDefault(g => g.Id == deduccionId);

                deduccion.Enable = false;

                _autorizacionDeduccionPlanillasInstance.Update(deduccion);
                rrhh_Web_DBContext.SaveChanges();


                var deducciones = _autorizacionDeduccionPlanillasInstance.AsQueryable().AsNoTracking().Where(f=> f.Enable==true)
                     .Select(x => new DeduccionDto
                     {
                         Id = x.Id,
                         NombreEmpleado = x.Empleado.Name,
                         Barcode = x.Empleado.BarCode,
                         FechaDeduccion = x.FechaDeduccion,
                         FechaCreacion = x.FechaCreacion,
                         Concepto = x.Concepto,
                         Estado = x.EstadoDeduccionPorPlanilla.Descripcion,
                         Monto = x.Monto,
                         Currency = x.Currency
                     }).OrderByDescending(c => c.Id).ToList();


                return Response<List<DeduccionDto>>.Success(deducciones);
            }
            catch (Exception ex)
            {
                return Response<List<DeduccionDto>>.Validation(ex.Message);
            }
        }
        #endregion

        #region Plazas

        public Response<List<DepartmentDto>> GetDepartments()
        {
            try
            {
                var departamentos = _departmentInstance.AsQueryable().AsNoTracking()
                     .Select(p => new DepartmentDto
                     {
                         Id = p.Id,
                         Descripcion = p.Name
                     }).OrderBy(o => o.Descripcion).ToList();


                return Response<List<DepartmentDto>>.Success(departamentos);
            }
            catch (Exception ex)
            {
                return Response<List<DepartmentDto>>.Validation(ex.Message);
            }
        }

        public Response<List<PlazaDto>> ObtenerPlazas()
        {
            try
            {
                var plazas = _plazaVacanteInstance.AsQueryable().AsNoTracking().Where(p => p.Enable == true)
                     .Select(p => new PlazaDto
                     {
                         Id = p.Id,
                         Titulo = p.Titulo,
                         Departamento = p.Departamento.Name,
                         Requisitos = p.Requisitos,
                         FechaCreacion = p.FechaCreacion.ToString()
                     }).OrderByDescending(p => p.FechaCreacion).ToList();


                return Response<List<PlazaDto>>.Success(plazas);
            }
            catch (Exception ex)
            {
                return Response<List<PlazaDto>>.Validation(ex.Message);
            }
        }

        public Response<List<PlazaDto>> GuardarPlaza(PlazaDto plaza_p)
        {
            try
            {
                PlazaVacante plaza = new PlazaVacante();

                plaza.DepartmentId = plaza_p.DepartmentId;
                plaza.Titulo = plaza_p.Titulo;
                plaza.Requisitos = plaza_p.Requisitos;
                plaza.FechaCreacion = DateTime.Now;
                plaza.Enable = true;

                _plazaVacanteInstance.Add(plaza);
                rrhh_Web_DBContext.SaveChanges();

                var plazas = _plazaVacanteInstance.AsQueryable().AsNoTracking()
                    .Select(p => new PlazaDto
                    {
                        Id = p.Id,
                        Titulo = p.Titulo,
                        Departamento = p.Departamento.Name,
                        Requisitos = p.Requisitos,
                        FechaCreacion = p.FechaCreacion.ToString()
                    }).OrderByDescending(o => o.FechaCreacion).ToList();


                return Response<List<PlazaDto>>.Success(plazas);
            }
            catch (Exception ex)
            {
                return Response<List<PlazaDto>>.Validation(ex.Message);
            }
        }

        public Response<List<PlazaDto>> EliminarPlaza(int plazaId)
        {
            try
            {

                var plaza = _plazaVacanteInstance.AsQueryable().AsNoTracking().FirstOrDefault(g => g.Id == plazaId);

                plaza.Enable = false;

                _plazaVacanteInstance.Update(plaza);
                rrhh_Web_DBContext.SaveChanges();


                var plazas = _plazaVacanteInstance.AsQueryable().AsNoTracking().Where(p => p.Enable == true)
                     .Select(p => new PlazaDto
                     {
                         Id = p.Id,
                         Titulo = p.Titulo,
                         Departamento = p.Departamento.Name,
                         Requisitos = p.Requisitos,
                         FechaCreacion = p.FechaCreacion.ToString()
                     }).OrderByDescending(p => p.FechaCreacion).ToList();


                return Response<List<PlazaDto>>.Success(plazas);
            }
            catch (Exception ex)
            {
                return Response<List<PlazaDto>>.Validation(ex.Message);
            }
        }

        public Response<bool> GuardarPostulante(PlazaVacantePostulanteDto plazaVacantePostulante)
        {
            try
            {
                PlazaVacantePostulante postulante = new PlazaVacantePostulante();

                //List<PlazaVacanteAdjunto> adjuntos = new List<PlazaVacanteAdjunto>();

                PlazaVacanteAdjunto adjunto = new PlazaVacanteAdjunto();


                //rrhh_Web_DBContext.Database.BeginTransaction();

                var nombre = _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(p => p.Id == plazaVacantePostulante.EmpleadoId).Name;

                postulante.NombrePostulante = plazaVacantePostulante.EsRecomendado == true ? plazaVacantePostulante.Nombre : nombre;
                postulante.Correo = plazaVacantePostulante.Correo;
                postulante.Telefono = plazaVacantePostulante.Telefono;
                postulante.EsRecomendado = plazaVacantePostulante.EsRecomendado;
                postulante.EmpleadoId = plazaVacantePostulante.EmpleadoId;
                postulante.PlazaVacanteId = plazaVacantePostulante.plazaVacanteId;
                postulante.Enable = true;

                _plazaVacantePostulanteInstance.Add(postulante);
                rrhh_Web_DBContext.SaveChanges();

                //rrhh_Web_DBContext.Database.CommitTransaction();

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                //rrhh_Web_DBContext.Database.RollbackTransaction();
                return Response<bool>.Validation(ex.Message);
            }
        }

        public Response<List< PostulantesAdminDto>> GetPostulantesByIdPlaza(int plazaId)
        {

            try
            {
                var postulantes = _plazaVacantePostulanteInstance.AsQueryable().AsNoTracking().Where(t => t.PlazaVacanteId == plazaId && t.Enable == true)
                    .Select(r => new PostulantesAdminDto
                    {
                        Id=r.Id,
                        Nombre = r.NombrePostulante,
                        Correo = r.Correo,
                        Telefono = r.Telefono,
                        Adjuntos = r.Adjuntos.Select(l=> new PostulantesAdminDto.AdjuntosPostulante { 
                                                URL = l.Host+l.Path,
                                                FileNameReference = l.ReferenceFileName
                                                }).ToList(),
                        RecomendadoOInterno = r.EsRecomendado== false ? "Postulante Interno":"Es recomendado por: "+ _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(g=> g.Id==r.EmpleadoId).Name
                    }).ToList();

                return Response <List<PostulantesAdminDto>>.Success( postulantes);
            }
            catch (Exception ex)
            {
                return Response<List<PostulantesAdminDto>>.Validation(ex.Message);

            }
        }

        public Response<List<PostulantesAdminDto>> DescartarPostulante(int postulanteId)
        {

            try
            {

                PlazaVacantePostulante postulante = new PlazaVacantePostulante();

                postulante = _plazaVacantePostulanteInstance.AsQueryable().AsNoTracking().FirstOrDefault(t => t.Id == postulanteId );

                postulante.Enable = false;

                _plazaVacantePostulanteInstance.Update(postulante);
                rrhh_Web_DBContext.SaveChanges();

                var postulantes = _plazaVacantePostulanteInstance.AsQueryable().AsNoTracking().Where(t => t.PlazaVacanteId == postulante.PlazaVacanteId && t.Enable==true)
                    .Select(r => new PostulantesAdminDto
                    {
                        Nombre = r.NombrePostulante,
                        Correo = r.Correo,
                        Telefono = r.Telefono,
                        Adjuntos = r.Adjuntos.Select(l => new PostulantesAdminDto.AdjuntosPostulante
                        {
                            URL = l.Host + l.Path,
                            FileNameReference = l.ReferenceFileName
                        }).ToList(),
                        RecomendadoOInterno = r.EsRecomendado == true ? "Postulante Interno" : "Es recomendado por: " + _employeeInstance.AsQueryable().AsNoTracking().FirstOrDefault(g => g.Id == r.EmpleadoId).Name
                    }).ToList();

                return Response<List<PostulantesAdminDto>>.Success(postulantes);
            }
            catch (Exception ex)
            {
                return Response<List<PostulantesAdminDto>>.Validation(ex.Message);

            }
        }
        #endregion


        #region Consultas Quejas y Sugerencias
        public Response<bool> GuardarQuejaSugerenciaDenuncia(QuejaSugerenciaDenunciaDto quejaSugerenciaDto)
        {

            try
            {

                QuejaSugerenciaDenuncia quejaSugerenciaDenuncia = new QuejaSugerenciaDenuncia();

                quejaSugerenciaDenuncia.Descripcion = quejaSugerenciaDto.Descripcion;
                quejaSugerenciaDenuncia.StateId = 1;
                quejaSugerenciaDenuncia.CreateDate = Convert.ToDateTime ( DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                //quejaSugerenciaDenuncia.LastModification = DBNull.Value;
                quejaSugerenciaDenuncia.TypeId = quejaSugerenciaDto.TypeId;

                _quejaSugerenciaDenunciaInstance.Add(quejaSugerenciaDenuncia);
                rrhh_Web_DBContext.SaveChanges();

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Validation(ex.Message);

            }
        }

        public Response<List<QuejaSugerenciaDenunciaTypeDto>> ObtenerQuejasSugerenciasDenunciasTypes()
        {
            try
            {

                var quejasSugerenciasDenunciasTypes = _quejaSugerenciaTypeInstance.AsQueryable().AsNoTracking().Where(r => r.Enable == true)
                    .Select(f => new QuejaSugerenciaDenunciaTypeDto
                    {
                        Id=f.Id,
                        Descripcion=f.Descripcion

                    }).ToList();

                return Response<List<QuejaSugerenciaDenunciaTypeDto>>.Success(quejasSugerenciasDenunciasTypes);
            }
            catch (Exception ex)
            {
                return Response<List<QuejaSugerenciaDenunciaTypeDto>>.Validation(ex.Message);

            }
        }

        public Response<List<QuejaSugerenciaDenunciaStateDto>> ObtenerQuejasSugerenciasDenunciasStates()
        {
            try
            {

                var quejasSugerenciasDenunciasStates = _quejaSugerenciaStateInstance.AsQueryable().AsNoTracking().Where(r => r.Enable == true)
                    .Select(f => new QuejaSugerenciaDenunciaStateDto
                    {
                        Id = f.Id,
                        State = f.Descripcion

                    }).ToList();

                return Response<List<QuejaSugerenciaDenunciaStateDto>>.Success(quejasSugerenciasDenunciasStates);
            }
            catch (Exception ex)
            {
                return Response<List<QuejaSugerenciaDenunciaStateDto>>.Validation(ex.Message);

            }
        }


        public Response<List<QuejaSugerenciaDenunciaAdminDto>> ObtenerQuejasSugerenciasDenuncias()
        {
            try
            {

                //var test = _quejaSugerenciaDenunciaInstance.AsQueryable().AsNoTracking().ToList();

                DateTime date = new DateTime(1900, 1, 1);

                var quejasSugerenciasDenuncias = _quejaSugerenciaDenunciaInstance.AsQueryable().Include(f => f.Type)
                    .Select(f => new QuejaSugerenciaDenunciaAdminDto
                    {
                        Id = f.Id,
                        Descripcion = f.Descripcion,
                        CreateDate = f.CreateDate,
                        StateId = f.StateId,
                        Estado = f.State.Descripcion,
                        TypeId = f.TypeId,
                        Tipo = f.Type.Descripcion,
                        //LastModification = f.LastModification.HasValue?f.LastModification.Value:date
                        LastModification = f.LastModification

                    }).OrderBy(r => r.StateId == 1).ToList();

                return Response<List<QuejaSugerenciaDenunciaAdminDto>>.Success(quejasSugerenciasDenuncias);
            }
            catch (Exception ex)
            {
                return Response<List<QuejaSugerenciaDenunciaAdminDto>>.Validation(ex.Message);

            }
        }


        public Response<bool> CambiarEstadoQuejaSugerenciaDenuncia(int id)
        {
            try
            {

                var dato = _quejaSugerenciaDenunciaInstance.FirstOrDefault(r => r.Id == id);

                dato.StateId = 2;

                _quejaSugerenciaDenunciaInstance.Update(dato);
                rrhh_Web_DBContext.SaveChanges();

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Validation(ex.Message);

            }
        }

        public Response<bool> CambiarEstadoQuejaSugerenciaDenunciaManual(int idQuejaSugerenciaDenuncia,int estadoId)
        {
            try
            {

                var dato = _quejaSugerenciaDenunciaInstance.FirstOrDefault(r => r.Id == idQuejaSugerenciaDenuncia);

                if (dato != null)
                {

                    dato.StateId = estadoId;
                    dato.LastModification = DateTime.Now;

                    _quejaSugerenciaDenunciaInstance.Update(dato);
                    rrhh_Web_DBContext.SaveChanges();

                    return Response<bool>.Success(true);
                }
                else
                    return Response<bool>.Validation("Elemento no encontrado");
            }
            catch (Exception ex)
            {
                return Response<bool>.Validation(ex.Message);

            }
        }

        #endregion

    }
}
