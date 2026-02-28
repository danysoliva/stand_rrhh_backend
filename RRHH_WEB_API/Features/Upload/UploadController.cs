using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RRHH_WEB_API._Common;
using RRHH_WEB_API.Features.Upload.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Upload
{
    [ApiController]
     [Route("api-rrhh")]
    public class UploadController : ControllerBase
    {
        private UploadService _uploadService;

        // requires using Microsoft.Extensions.Configuration;
        private readonly IConfiguration Configuration;

        public UploadController(UploadService uploadService, IConfiguration configuration)
        {
            _uploadService = uploadService;
            Configuration = configuration;

           

        }

        [HttpPost]
        [Route("upload")]
        public async Task<Response<bool>> Post()
        {
            IFormCollection form = await Request.ReadFormAsync();

            if (!form.Files.Any())
            {
                return Response<bool>.Excepcion("No se envió ningun archivo.");
            }

            List<IFormFile> files = new List<IFormFile>();

            for (int i = 0; i < form.Files.Count; i++)
            {
                files.Add(form.Files[i]);
            }

            string host = $"http://" + HttpContext.Request.Host;

            return _uploadService.SaveFiles(files,host);
        }

        [HttpGet]
        [Route("getFiles")]
        public IActionResult GetFiles()
        {
            return this.ActionResultFrom(_uploadService.GetFiles());
        }


        [HttpGet("getImagenesNoticias")]
        public IActionResult GetImagenesNoticias()
        {
            var response = _uploadService.ObtenerImagenesNoticias();
            return this.ActionResultFrom(response);
        }

        [HttpPost]
        [Route("borrarImagen")]
        public IActionResult BorrarImagen([FromBody] int repositoryId)
        {
            var response = _uploadService.EliminarImagen(repositoryId);
            return this.ActionResultFrom(response);
        }

        [HttpPost]
        [Route("cambiarDuracionImagen")]
        public IActionResult CambiarDuracionImagen([FromBody] int duracion)
        {
            var response = _uploadService.CambiarDuracionImagen(duracion);
            return this.ActionResultFrom(response);
        }

      
        [HttpPost]
        [Route("uploadDocument/{tipo}/{id_grupo}")]
        public async Task<Response<bool>> GuardarDocumento(int tipo, int id_grupo)
        {
            IFormCollection form = await Request.ReadFormAsync();

            if (!form.Files.Any())
            {
                return Response<bool>.Excepcion("No se envió ningún archivo.");
            }

            List<IFormFile> files = new List<IFormFile>();

            for (int i = 0; i < form.Files.Count; i++)
            {
                files.Add(form.Files[i]);
            }

            string host = $"http://" + HttpContext.Request.Host;

            return _uploadService.GuardarDocumento(tipo, files, host, id_grupo);
        }


        [HttpGet("getDocuments/{tipo}")]
        public IActionResult ObtenerDocumentosPorTipo(int tipo)
        {
            var response = _uploadService.ObtenerDocumentosPorTipo(tipo);
            return this.ActionResultFrom(response);
        }

        [HttpGet("getDocumentsGroup")]
        public IActionResult ObtenerDocumentosGrupo()
        {
            var response = _uploadService.ObtenerGrupoRepositorio();
            return this.ActionResultFrom(response);
        }

        [HttpPost]
        [Route("deleteDocument/{tipo}")]
        public IActionResult EliminarDocumento(int tipo, [FromBody] int repositoryId)
        {
            var response = _uploadService.EliminarDocumento(tipo, repositoryId);
            return this.ActionResultFrom(response);
        }

        [HttpGet]
        [Route("changeGroupDocument/{id_repo}/{id_grupo}")]
        public IActionResult CambiarGrupoDocumento(int id_repo, int id_grupo)
        {
            var response = _uploadService.CambiarGrupo(id_repo, id_grupo);
            return this.ActionResultFrom(response);
        }


        [HttpPost("grupoCRUD")]
        //[Authorize]
        public IActionResult GrupoCRUD([FromBody] RepositoryGroupCRUDDto grupoDto)
        {
     
            var response = _uploadService.CRUD_DocumentosGrupo(grupoDto);
            return this.ActionResultFrom(response);
        }
    }
}
