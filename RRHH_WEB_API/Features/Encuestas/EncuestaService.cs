using Microsoft.Extensions.Configuration;
using RRHH_WEB_API._Common;
using RRHH_WEB_API._Infraestructura;
using RRHH_WEB_API._Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using RRHH_WEB_API.Features.Encuestas.Dto;
using RRHH_WEB_API._Entidades.Encuestas;
using RRHH_WEB_API._Entidades.Encuesta;

namespace RRHH_WEB_API.Features.Encuestas
{
    public class EncuestaService
    {
        private readonly IConfiguration _configuration;
        private readonly RRHH_DBContext rrhh_Web_DBContext;
        private readonly ACS_DBContext _acs_DBContext;

        private readonly DbSet<EncuestaH> _encuestaInstance;
        private readonly DbSet<EncuestaPregunta> _encuestaPreguntaInstance;
        private readonly DbSet<EncuestaOpcion> _encuestaOpcionInstance;
        private readonly DbSet<EncuestaRespuesta> _encuestaRespuestaInstance;
        private readonly DbSet<EncuestaEstado> _encuestaEstado ;



        public EncuestaService(RRHH_DBContext rrhh_DBContext, IConfiguration configuration, ACS_DBContext acs_DBContext)
        {
            _configuration = configuration;
            rrhh_Web_DBContext = rrhh_DBContext;
            _acs_DBContext = acs_DBContext;

            _encuestaInstance = _acs_DBContext.Encuesta;
            _encuestaPreguntaInstance = _acs_DBContext.EncuestaPreguntas;
            _encuestaOpcionInstance = _acs_DBContext.EncuestaOpciones;
            _encuestaRespuestaInstance = _acs_DBContext.EncuestaRespuestas;
            _encuestaEstado = _acs_DBContext.EncuestaEstado;

        }

        public Response<bool> SaveEncuestaCreator(EncuestaSaveParamsDto encuesta)
        {
            try
            {
                EncuestaH encuestaSave = new EncuestaH();
                EncuestaPregunta pregunta = new EncuestaPregunta();
                EncuestaOpcion opcion = new EncuestaOpcion();

                _acs_DBContext.Database.BeginTransaction();

                encuestaSave.Titulo = encuesta.Titulo.ToUpper();
                encuestaSave.FechaCreacion = DateTime.Now;
                encuestaSave.EstadoId = 1;
                encuestaSave.Enable = true;

                _encuestaInstance.Add(encuestaSave);
                _acs_DBContext.SaveChanges();

                foreach (var item in encuesta.Preguntas)
                {
                    pregunta = new EncuestaPregunta();

                    pregunta.Descripcion = item.Pregunta;
                    pregunta.EncuestaId = encuestaSave.Id;
                    pregunta.Enable = true;

                    _encuestaPreguntaInstance.Add(pregunta);
                    _acs_DBContext.SaveChanges();

                    foreach (var elementos in item.Opciones)
                    {
                        opcion = new EncuestaOpcion();

                        opcion.PreguntaId = pregunta.Id;
                        opcion.EncuestaId = encuestaSave.Id;
                        opcion.Opcion = elementos;
                        opcion.Enable = true;

                        _encuestaOpcionInstance.Add(opcion);
                        _acs_DBContext.SaveChanges();

                    }
                }

                _acs_DBContext.Database.CommitTransaction();

                return Response<bool>.Success(false);


            }
            catch (Exception ex)
            {
                rrhh_Web_DBContext.Database.RollbackTransaction();
                return Response<bool>.Validation(ex.Message);
            }
        }


        public Response<EncuestaResponseDto> EncuestaView( int encuestaId, int employeeId)
        {
            try
            {

                var countEmployee = _encuestaRespuestaInstance.AsQueryable().AsNoTracking().Count(r => r.EmployeeId == employeeId && r.EncuestaId==encuestaId);


                if (countEmployee==0)
                {       

                EncuestaResponseDto encuestaResponse= new EncuestaResponseDto();

                var encuesta = _encuestaInstance.AsQueryable().Include(r=>r.Preguntas).ThenInclude(m=>m.Opciones).FirstOrDefault(x => x.Id == encuestaId);

                EncuestaResponsePageDto pagina = new EncuestaResponsePageDto();

                encuestaResponse.Title = encuesta.Titulo;
                encuestaResponse.StartSurveyText = "";
                encuestaResponse.Description = "";

                encuestaResponse.Pages = new List<EncuestaResponsePageDto>();
                //encuestaResponse.Pages.Add(new EncuestaResponsePageDto());

                List<EncuestaReponseElementDto> encuestaElementDtos = new List<EncuestaReponseElementDto>();
                //var opciones = _encuestaOpcionInstance.AsQueryable().AsNoTracking().Where(p => p.EncuestaId == encuestaId).ToList();

                //var op = opciones.Where(p => p.PreguntaId == g.Id);


                encuesta.Preguntas.ForEach(g =>
                        encuestaElementDtos.Add(new EncuestaReponseElementDto {
                            Type = "radiogroup",
                            Title = g.Descripcion,
                            Name = g.Id.ToString(),
                            Choices = g.Opciones.Where(p => p.PreguntaId == g.Id).Select(t => t.Opcion).ToList(),
                            ChoicesWithId = g.Opciones.Where(p => p.PreguntaId == g.Id).Select(t=> new EncuestaReponseOptionDto { 
                                    Id=t.Id,
                                    Name=t.Opcion
                            }).ToList(),
                            IsRequired = true,
                            ColCount = 5
                        }));


                pagina.Elements = encuestaElementDtos;
                encuestaResponse.Pages.Add(pagina);

                return Response<EncuestaResponseDto>.Success(encuestaResponse);
                }
                else
                {
                    return Response<EncuestaResponseDto>.Validation("Usted ya ha llenado esta encuesta");
                }
            }
            catch (Exception ex)
            {
                rrhh_Web_DBContext.Database.RollbackTransaction();
                return Response<EncuestaResponseDto>.Validation(ex.Message);
            }
        }


        public Response<bool> GuardarEncuesta( List< EncuestaAnswerDto> respuestas, int employeeId)
        {
            try
            {

                EncuestaRespuesta respuesta = new EncuestaRespuesta();

                foreach (var item in respuestas)
                {
                    respuesta = new EncuestaRespuesta();

                    respuesta.EncuestaId = item.EncuestaId;
                    respuesta.OpcionId = item.OpcionId;
                    respuesta.PreguntaId = item.PreguntaId;
                    respuesta.EmployeeId = employeeId;

                    _encuestaRespuestaInstance.Add(respuesta);
                    _acs_DBContext.SaveChanges();
                    
                }


                return Response<bool>.Success(true);

            }
            catch (Exception ex)
            {

                return Response<bool>.Validation(ex.Message);
            }
        }


        public Response<List<EncuestaDto>> GetEncuestas()
        {
            try
            {

       //         var encuestas = _encuestaInstance.AsQueryable().AsNoTracking().Where(p => p.Enable == true)
       //.ToList();

                var encuestas = _encuestaInstance.AsQueryable().AsNoTracking()
                       .Where(p => p.Enable == true && p.EstadoId==1)
                       .Select(r=> new EncuestaDto
                       {
                           Id = r.Id,
                           Titulo = r.Titulo,
                           FechaCreacion = r.FechaCreacion,
                           Estado = r.Estado.Descripcion
                       }).ToList();




                return Response<List<EncuestaDto>>.Success(encuestas);

            }
            catch (Exception ex)
            {

                return Response<List<EncuestaDto>>.Validation(ex.Message);
            }
        }


        public Response<bool> CerrarEncuesta(int encuestaId)
        {
            try
            {

                EncuestaH encuesta = new EncuestaH();


                encuesta = _encuestaInstance.AsQueryable().FirstOrDefault(r => r.Id == encuestaId);

                encuesta.EstadoId = 2;

                    _encuestaInstance.Update(encuesta);
                    rrhh_Web_DBContext.SaveChanges();

                         return Response<bool>.Success(true);

            }
            catch (Exception ex)
            {

                return Response<bool>.Validation(ex.Message);
            }
        }


        public Response<EncuestaFiltroDto> ObtenerEncuestaFiltros()
        {
            try
            {
                var estados = _encuestaEstado.AsQueryable().AsNoTracking().Where(t => t.Enable == true)
                    .Select(x => new EncuestaEstadoFiltroDto { 
                           Id=x.Id,
                           Estado=x.Descripcion
                        }).ToList();

                var encuestasName = _encuestaInstance.AsQueryable().AsNoTracking().Where(r => r.Enable == true)
                    .Select(t => new EncuestaNameFiltroDto
                    {
                        Id=t.Id,
                        Descripcion=t.Titulo

                    }).ToList();

                EncuestaFiltroDto encuestaFiltro = new EncuestaFiltroDto();

                encuestaFiltro.Estados = estados;
                encuestaFiltro.Encuestas = encuestasName;
              



                return Response<EncuestaFiltroDto>.Success(encuestaFiltro);

            }
            catch (Exception ex)
            {

                return Response<EncuestaFiltroDto>.Validation(ex.Message);
            }
        }


        public Response<List<EncuestaNameFiltroDto>> ObtenerEncuestaFiltrosPorEstado(int estadoId)
        {
            try
            {

                List<EncuestaNameFiltroDto> encuestasName = new List<EncuestaNameFiltroDto>();

                if (estadoId != 0)
                {
                    encuestasName = _encuestaInstance.AsQueryable().AsNoTracking().Where(r => r.Enable == true && r.EstadoId == estadoId)
                       .Select(t => new EncuestaNameFiltroDto
                       {
                           Id = t.Id,
                           Descripcion = t.Titulo

                       }).ToList();
                }
                else
                {
                    encuestasName = _encuestaInstance.AsQueryable().AsNoTracking().Where(r => r.Enable == true)
                  .Select(t => new EncuestaNameFiltroDto
                  {
                      Id = t.Id,
                      Descripcion = t.Titulo

                  }).ToList();
                }


                return Response<List<EncuestaNameFiltroDto>>.Success(encuestasName);

            }
            catch (Exception ex)
            {

                return Response<List<EncuestaNameFiltroDto>>.Validation(ex.Message);
            }
        }


        public Response<List< EncuestaTabulacionDto>> ObtenerTabulacionEncuesta(int encuestaId)
        {
            try
            {


                var preguntas = _encuestaPreguntaInstance.AsQueryable().AsNoTracking().Include(x=> x.Opciones).Where(r => r.EncuestaId == encuestaId && r.Enable == true).ToList();
                EncuestaTabulacionDto tabulacion = new EncuestaTabulacionDto();
                List<EncuestaTabulacionDto> tabulaciones = new List<EncuestaTabulacionDto>();

                foreach (var pregunta in preguntas)
                {

                    foreach (var opcion in pregunta.Opciones)
                    {
                        tabulacion = new EncuestaTabulacionDto();
                        tabulacion.PreguntaId = pregunta.Id;
                        tabulacion.Pregunta = pregunta.Descripcion;
                        tabulacion.OpcionId = opcion.Id;
                        tabulacion.Opcion = opcion.Opcion;
                        tabulacion.Conteo = _encuestaRespuestaInstance.AsQueryable().AsNoTracking().Count(t => t.OpcionId == opcion.Id && t.PreguntaId == pregunta.Id);

                        tabulaciones.Add(tabulacion);
                    }
                
                }

                //var result = 

                var tab = tabulaciones.GroupBy(r => new { r.OpcionId, r.Pregunta, r.Opcion,r.PreguntaId })
                            .Select(c => new EncuestaTabulacionDto
                            {
                                PreguntaId = c.Key.PreguntaId,
                                Pregunta = c.Key.Pregunta,
                                OpcionId = c.Key.OpcionId,
                                Opcion = c.Key.Opcion,
                                Conteo = c.Sum(e => e.Conteo)
                            }).ToList();


                return Response<List<EncuestaTabulacionDto>>.Success(tab);

            }
            catch (Exception ex)
            {

                return Response<List<EncuestaTabulacionDto>>.Validation(ex.Message);
            }
        }
    }
}
