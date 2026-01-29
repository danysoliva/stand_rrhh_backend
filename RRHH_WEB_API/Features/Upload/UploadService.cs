using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RRHH_WEB_API._Common;
using RRHH_WEB_API._Entidades;
using RRHH_WEB_API._Infraestructura;
using RRHH_WEB_API.Features.Maestros.Dtos;
using RRHH_WEB_API.Features.Upload.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RRHH_WEB_API.Features.Upload
{
    public class UploadService
    {
        private readonly IWebHostEnvironment _enviroment;
        private readonly RRHH_Web_DBContext _rrhh_DBContext;
        private readonly DbSet<RepositoryImage> _repositoryImage;
        private readonly DbSet<RepositoryDocument> _repositoryDocumento;
        private readonly DbSet<ParametrosGenerales> _parametrosGenerales;
        private readonly DbSet<RepositoryGroup> _repositoryGroupInstance;

        public UploadService(RRHH_Web_DBContext rrhh_DBContext, IWebHostEnvironment environment)
        {
            _enviroment = environment;
            _rrhh_DBContext = rrhh_DBContext;
            _repositoryImage = rrhh_DBContext.RepositoryImage;
            _repositoryDocumento = rrhh_DBContext.RepositoryDocument;
            _parametrosGenerales = rrhh_DBContext.ParametrosGenerales;
            _repositoryGroupInstance = rrhh_DBContext.RepositoryGroups;
        }

        public Response<bool> SaveFiles(List<IFormFile> archivos,string host)
        {
            string pathFileDestiny = $"/upload/";
            //string host = "http://10.50.11.32:8091";
            //string host = "http://localhost:10178";
            string pathServerUpload = $"{_enviroment.ContentRootPath}\\wwwroot\\upload\\";
            string fileName;

            try
            {
                for (int i = 0; i < archivos.Count; i++)
                {
                    if (!Directory.Exists(pathServerUpload))
                    {
                        Directory.CreateDirectory(pathServerUpload);
                    }

                    fileName = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace('/', (char)32).Replace('-', (char)32).Replace(':', (char)32).Replace(" ", String.Empty) + Path.GetExtension(archivos[i].Name);

                    //using (FileStream fileStream = System.IO.File.Create(_enviroment.WebRootPath + "\\Upload\\" + long.Parse(DateTime.Now.ToString("ddMMyyyyHHmmss"))+ Path.GetExtension(archivos[i].Name)))
                    using (FileStream fileStream = File.Create(pathServerUpload + fileName))
                    {
                        archivos[i].CopyTo(fileStream);
                        fileStream.Flush();

                        RepositoryImage image = new RepositoryImage
                        {
                            Path = pathFileDestiny + fileName,
                            FileName = fileName,
                            ReferenceFileName=archivos[i].FileName,
                            Host = host,
                            Enabled = true
                        };

                        _repositoryImage.Add(image);
                        _rrhh_DBContext.SaveChanges();
                    }
                }
                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                RepositoryImage image = new RepositoryImage
                {
                    Path = ex.Message,
                    FileName = ex.Message,
                    Host = host,
                    Enabled = true
                };

                _repositoryImage.Add(image);
                _rrhh_DBContext.SaveChanges();

                return Response<bool>.Excepcion("Ocurrió un error al subir el archivo: " + ex.Message);
            }

        }

        public Response<List<NoticiaDto>> GetFiles()
        {
            List<NoticiaDto> noticias = _repositoryImage.AsQueryable().AsNoTracking()
                .Where(x => x.Enabled)
                .Select(x => new NoticiaDto
                {
                    Url = x.Path + x.FileName,
                    FileName = x.FileName
                }).ToList();

            return Response<List<NoticiaDto>>.Success(noticias);
        }


        public Response<NoticiasConConfiguracionDto> ObtenerImagenesNoticias()
        {
            try
            {
                var imagenes = _repositoryImage.AsQueryable().AsNoTracking()
                                .Where(f => f.Enabled == true)
                                .Select(p => new RepositorioImagenesDto
                                {
                                    Id = p.Id,
                                    FileName = p.FileName,
                                    Path = p.Path,
                                    Host = p.Host,
                                    ReferenceFileName=p.ReferenceFileName,
                                    FullPath = p.Host  + p.Path
                                }).ToList();

                int duracion = Convert.ToInt32( _parametrosGenerales.AsQueryable().AsNoTracking().FirstOrDefault(p => p.Id == 1).Valor); //1 es la duracion es entre imagenes

                NoticiasConConfiguracionDto noticias = new NoticiasConConfiguracionDto();

                noticias.DuracionImagenes = duracion;
                noticias.RepositorioImagenes = imagenes;

                return Response<NoticiasConConfiguracionDto>.Success(noticias);
            }
            catch (Exception ex)
            {
                return Response<NoticiasConConfiguracionDto>.Excepcion(ex.Message);
            }
        }


        public Response<List<RepositorioImagenesDto>> EliminarImagen(int repositorioId)
        {
            try
            {

                var imagen = _repositoryImage.AsQueryable().AsNoTracking().Where(x => x.Id == repositorioId).FirstOrDefault();

                imagen.Enabled = false;

                _repositoryImage.Update(imagen);

                _rrhh_DBContext.SaveChanges();


                var imagenes = _repositoryImage.AsQueryable().AsNoTracking()
                                .Where(f => f.Enabled == true)
                                .Select(p => new RepositorioImagenesDto
                                {
                                    Id = p.Id,
                                    FileName = p.FileName,
                                    Path = p.Path,
                                    Host = p.Host,
                                    FullPath = p.Host + p.Path,
                                    ReferenceFileName = p.ReferenceFileName,
                                }).ToList();


                return Response<List<RepositorioImagenesDto>>.Success(imagenes);
            }
            catch (Exception ex)
            {
                return Response<List<RepositorioImagenesDto>>.Excepcion(ex.Message);
            }
        }


        public Response<bool> CambiarDuracionImagen(int duracion)
        {
            try
            {

                var parametro = _parametrosGenerales.AsQueryable().AsNoTracking().Where(d => d.Id == 1).FirstOrDefault();

                parametro.Valor = (duracion*1000).ToString();

                _parametrosGenerales.Update(parametro);

                _rrhh_DBContext.SaveChanges();



                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Excepcion(ex.Message);
            }
        }

        public Response<bool> GuardarDocumento(int tipo, List<IFormFile> archivos, string host, int id_grupo)
        {
            string pathFileDestiny = $"/formatos/";
            //string host = "http://10.50.11.32:8091";
            //string host = "http://localhost:10178";
            string pathServerUpload = $"{_enviroment.ContentRootPath}\\wwwroot\\formatos\\";
            string fileName;

            try
            {
                for (int i = 0; i < archivos.Count; i++)
                {
                    if (!Directory.Exists(pathServerUpload))
                    {
                        Directory.CreateDirectory(pathServerUpload);
                    }

                    fileName = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace('/', (char)32).Replace('-', (char)32).Replace(':', (char)32).Replace(" ", String.Empty) + Path.GetExtension(archivos[i].Name);

                    //using (FileStream fileStream = System.IO.File.Create(_enviroment.WebRootPath + "\\Upload\\" + long.Parse(DateTime.Now.ToString("ddMMyyyyHHmmss"))+ Path.GetExtension(archivos[i].Name)))
                    using (FileStream fileStream = File.Create(pathServerUpload + fileName))
                    {
                        archivos[i].CopyTo(fileStream);
                        fileStream.Flush();

                        RepositoryDocument image = new RepositoryDocument
                        {
                            Path = pathFileDestiny + fileName,
                            FileName = fileName,
                            ReferenceFileName = archivos[i].FileName,
                            Host = host,
                            Enabled = true,
                            Tipo = tipo,
                            GrupoID = id_grupo
                        };

                       _repositoryDocumento.Add(image);
                        _rrhh_DBContext.SaveChanges();
                    }
                }
                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                RepositoryImage image = new RepositoryImage
                {
                    Path = ex.Message,
                    FileName = ex.Message,
                    Host = host,
                    Enabled = true
                };

                _repositoryImage.Add(image);
                _rrhh_DBContext.SaveChanges();

                return Response<bool>.Excepcion("Ocurrió un error al subir el archivo: " + ex.Message);
            }

        }

        public Response<List<RepositorioDocumentoDto>> CambiarGrupo(int id_repositorio, int id_grupo)
        {
            try
            {

                var repositorio = _repositoryDocumento.AsQueryable().FirstOrDefault(f => f.Id == id_repositorio);
                
                repositorio.GrupoID = id_grupo;

                _repositoryDocumento.Update(repositorio);
                _rrhh_DBContext.SaveChanges();


               return ObtenerDocumentosPorTipo((int)TipoDocumentoEnum.Formatos);
            }
            catch (Exception ex)
            {
                return Response<List<RepositorioDocumentoDto>>.Excepcion("Ocurrió un error al subir el archivo: " + ex.Message);
            }

        }


        public Response<List<RepositorioDocumentoDto>> ObtenerDocumentosPorTipo(int tipo)
        {
            try
            {

                string sql = $"EXEC rrhh_web.usp_GetDocumentosRepositorioFiltradoPorTipo  {tipo}";

                var f = _rrhh_DBContext.RepositoryDocument

                .FromSqlRaw(sql).ToList();

                //var documentos = _rrhh_DBContext.RepositoryDocument

                //.FromSqlRaw(sql).ToList().Select(p => new RepositorioDocumentoDto
                //{
                //    Id = p.Id,
                //    FileName = p.FileName,
                //    Path = p.Path,
                //    Host = p.Host,
                //    ReferenceFileName = p.ReferenceFileName,
                //    FullPath = p.Host + p.Path,
                //    GrupoId = p.GrupoID,
                //    //Descripcion = p.RepositoryGroup.Descripcion
                //}).ToList();



                var documentos = (from doctos in _repositoryDocumento
                                  join grupos in _repositoryGroupInstance on doctos.GrupoID equals grupos.Id into DoctosGrupos
                                  from doctos_grupos in DoctosGrupos.DefaultIfEmpty()
                                  where doctos.Enabled == true && doctos.Tipo == tipo
                                  //orderby score descending
                                  select new RepositorioDocumentoDto
                                  {
                                      Id = doctos.Id,
                                      FileName = doctos.FileName,
                                      Path = doctos.Path,
                                      Host = doctos.Host,
                                      ReferenceFileName = doctos.ReferenceFileName,
                                      FullPath = doctos.Host + doctos.Path,
                                      GrupoId = Convert.ToInt32(doctos.GrupoID == null ? 0 : doctos.GrupoID),
                                      GrupoDocumento = doctos_grupos.Descripcion == null ? "No Definido": doctos_grupos.Descripcion
                                  }).ToList();


                //var documentos = _repositoryDocumento.AsQueryable().AsNoTracking()
                //                .Where(f => f.Enabled == true && f.Tipo == tipo)
                //                .Select(p => new RepositorioDocumentoDto
                //                {
                //                    Id = p.Id,
                //                    FileName = p.FileName,
                //                    Path = p.Path,
                //                    Host = p.Host,
                //                    ReferenceFileName = p.ReferenceFileName,
                //                    FullPath = p.Host + p.Path,
                //                    GrupoId = p.GrupoID,
                //                    GrupoDocumento = p.RepositoryGroup.Descripcion
                //                }).ToList();

                return Response<List<RepositorioDocumentoDto>>.Success(documentos);
            }
            catch (Exception ex)
            {
                return Response<List<RepositorioDocumentoDto>>.Excepcion(ex.Message);
            }
        }

        public Response<List<RepositorioDocumentoDto>> EliminarDocumento(int tipo, int repositorioId)
        {
            try
            {

                var documento = _repositoryDocumento.AsQueryable().AsNoTracking().Where(x => x.Id == repositorioId).FirstOrDefault();

                documento.Enabled = false;

                _repositoryDocumento.Update(documento);

                _rrhh_DBContext.SaveChanges();

                var documentos = ObtenerDocumentosPorTipo(tipo).Data;

                return Response<List<RepositorioDocumentoDto>>.Success(documentos);
            }
            catch (Exception ex)
            {
                return Response<List<RepositorioDocumentoDto>>.Excepcion(ex.Message);
            }
        }

        #region RepositoryGroup
        public Response<List<RepositoryGroupDto>> ObtenerGrupoRepositorio()
        {
            try
            {
                var grupos = _repositoryGroupInstance.AsQueryable().AsNoTracking()
                                .Where(f => f.Enable == true)
                                .Select(p => new RepositoryGroupDto
                                {
                                    Id = p.Id,
                                   Descripcion = p.Descripcion
                                }).ToList();

                return Response<List<RepositoryGroupDto>>.Success(grupos);
            }
            catch (Exception ex)
            {
                return Response<List<RepositoryGroupDto>>.Excepcion(ex.Message);
            }
        }


        public Response<bool> CRUD_DocumentosGrupo(RepositoryGroupCRUDDto grupo)
        {
            try
            {
                bool result = false;

                switch (grupo.TipoCRUD )
                {
                    case 1:

                        RepositoryGroup grupoInserted = new RepositoryGroup();

                        grupoInserted.Id = 0;
                        grupoInserted.Descripcion = grupo.Descripcion;
                        grupoInserted.Enable = true;

                        _repositoryGroupInstance.Add(grupoInserted);
                        _rrhh_DBContext.SaveChanges();
                        break;

                    case 2:

                        var group = _repositoryGroupInstance.FirstOrDefault(f => f.Id == grupo.Id);

                        group.Descripcion = grupo.Descripcion;

                        _repositoryGroupInstance.Update(group);

                        _rrhh_DBContext.SaveChanges();
                        break;

                    case 3:
                        var group2 = _repositoryGroupInstance.FirstOrDefault(f => f.Id == grupo.Id);

                        group2.Enable = false;

                        _repositoryGroupInstance.Update(group2);

                        _rrhh_DBContext.SaveChanges();
                        break;
                }

                return Response<bool>.Success(result);
            }
            catch (Exception ex)
            {
                return Response<bool>.Excepcion(ex.Message);
            }
        }
        #endregion
    }
}
