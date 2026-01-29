using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RRHH_WEB_API._Common;
using RRHH_WEB_API.Features.GestionesVarias;
using RRHH_WEB_API.Features.GestionesVarias.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.GestionesVarias
{
    [ApiController]
    [Route("api-rrhh/[controller]")]
    public class GestionesVariasController : Controller
    {
        private readonly GestionesVariasService gestionesVariasService;

        public GestionesVariasController(GestionesVariasService deduccionPorPlanillaService)
        {
            gestionesVariasService = deduccionPorPlanillaService;
        }

        [HttpPost("guardarDeduccionPlanilla")]
        [Authorize]
        public IActionResult GuardarDediccionPlanilla([FromBody] ParamsDeduccionPlanillaDto paramsDeduccionPlanillaDto)
        {
            IActionResult userResponse = this.GetClaim("EmpleadoId", out int employeeId);
            if (employeeId == 0) return userResponse;

            paramsDeduccionPlanillaDto.UsuarioCreacionId = employeeId;

            var response = gestionesVariasService.GuardarDeduccion(paramsDeduccionPlanillaDto);
            return this.ActionResultFrom(response);
        }


        [HttpGet("obtenerEmpleados")]
        public IActionResult GetEmployees()
        {
            var response = gestionesVariasService.GetEmployees();
            return this.ActionResultFrom(response);
        }

        [HttpGet("obtenerDeducciones")]
        public IActionResult ObtenerDeducciones()
        {
            var response = gestionesVariasService.ObtenerDeducciones();
            return this.ActionResultFrom(response);
        }

        [HttpPost("imprimirFormatoDeduccionPlanilla")]
        public IActionResult ImprimirFormatoDeduccionPlanilla([FromBody] int deduccion_id)
        {
            var response = gestionesVariasService.ImprimirFormatoDeduccionPorPlanilla(deduccion_id);
            return this.ActionResultFrom(response);
        }

        [HttpGet("obtenerDepartamentos")]
        public IActionResult ObtenerDepartamentos()
        {
            var response = gestionesVariasService.GetDepartments();
            return this.ActionResultFrom(response);
        }

        [HttpGet("obtenerPlazas")]
        public IActionResult ObtenerPlazas()
        {
            var response = gestionesVariasService.ObtenerPlazas();
            return this.ActionResultFrom(response);
        }


        [HttpPost("guardarPlaza")]
        //[Authorize]
        public IActionResult GuardarPlaza([FromBody] PlazaDto plaza)
        {
            //IActionResult userResponse = this.GetClaim("EmpleadoId", out int employeeId);
            //if (employeeId == 0) return userResponse;

            var response = gestionesVariasService.GuardarPlaza(plaza);
            return this.ActionResultFrom(response);
        }

        [HttpPost("eliminarPlaza")]
        public IActionResult EliminarPlaza([FromBody] int plazaId)
        {
            var response = gestionesVariasService.EliminarPlaza(plazaId);
            return this.ActionResultFrom(response);
        }

        [HttpPost("eliminarDeduccion")]
        public IActionResult EliminarDeduccion([FromBody] int deduccionId)
        {
            var response = gestionesVariasService.EliminarDeduccion(deduccionId);
            return this.ActionResultFrom(response);
        }

        //[HttpPost]
        //[Authorize]
        //[Route("guardarPostulante")]
        //public async Task<IActionResult> Post()
        //{
        //    IActionResult userResponse = this.GetClaim("EmpleadoId", out int employeeId);
        //    if (employeeId == 0) return userResponse;

        //    IFormCollection form = await Request.ReadFormAsync();
        //    List<IFormFile> archivos = new List<IFormFile>();

        //    if (form.Files.Any())
        //    {
        //        archivos = (from f in form.Files select f).ToList();
        //    }

        //    PlazaVacantePostulanteDto postulante = JsonConvert.DeserializeObject<PlazaVacantePostulanteDto>(form["dto"]);

        //    //postulante.EmpleadoId = employeeId;

        //    string host = $"http://" + HttpContext.Request.Host;
        //    var response = gestionesVariasService.GuardarPostulante(postulante, archivos, host);

        //    return this.ActionResultFrom(response);
        //}


        [HttpPost("guardarPostulante")]
        [Authorize]
        public IActionResult GuardarPostulante([FromBody] PlazaVacantePostulanteDto postulante)
        {
            var response = gestionesVariasService.GuardarPostulante(postulante);
            return this.ActionResultFrom(response);
        }

        [HttpPost("getPostulantesByIdPlaza")]
        public IActionResult GetPostulantesByPlazaId([FromBody] int plazaId)
        {
            var response = gestionesVariasService.GetPostulantesByIdPlaza(plazaId);
            return this.ActionResultFrom(response);
        }


        [HttpPost("descartarPostulante")]
        public IActionResult DescartarPostulante([FromBody] int postulanteId)
        {
            var response = gestionesVariasService.DescartarPostulante(postulanteId);
            return this.ActionResultFrom(response);
        }

        [HttpPost("guardarQuejaSugerenciaDenuncia")]
        public IActionResult GuardarQuejaSugerencia([FromBody] QuejaSugerenciaDenunciaDto quejaSugerenciaDto)
        {
            var response = gestionesVariasService.GuardarQuejaSugerenciaDenuncia(quejaSugerenciaDto);
            return this.ActionResultFrom(response);
        }


        [HttpGet("obtenerQuejasSugerenciasDenunciasType")]
        public IActionResult ObtenerQuejasSugerenciasDenunciasTypes()
        {
            var response = gestionesVariasService.ObtenerQuejasSugerenciasDenunciasTypes();
            return this.ActionResultFrom(response);
        }

        [HttpGet("obtenerQuejasSugerenciasDenunciasStates")]
        public IActionResult ObtenerQuejasSugerenciasDenunciasStates()
        {
            var response = gestionesVariasService.ObtenerQuejasSugerenciasDenunciasStates();
            return this.ActionResultFrom(response);
        }

        [HttpGet("obtenerQuejasSugerenciasDenuncias")]
        public IActionResult ObtenerQuejasSugerenciasDenuncias()
        {
            var response = gestionesVariasService.ObtenerQuejasSugerenciasDenuncias();
            return this.ActionResultFrom(response);
        }

        [HttpPost("cambiarEstadoQuejaSugerenciaDenuncia")]
        public IActionResult CambiarEstadoQuejaSugerenciaDenuncia([FromBody] int id)
        {
            var response = gestionesVariasService.CambiarEstadoQuejaSugerenciaDenuncia(id);
            return this.ActionResultFrom(response);
        }


        [HttpGet("cambiarEstadoQuejaSugerenciaDenunciaManual/{idQuejaDenunciaSugerencia}/{estadoId}")]
        public IActionResult CambiarEstadoQuejaSugerenciaDenunciaManual( int idQuejaDenunciaSugerencia,int estadoId)
        {
            var response = gestionesVariasService.CambiarEstadoQuejaSugerenciaDenunciaManual(idQuejaDenunciaSugerencia, estadoId);
            return this.ActionResultFrom(response);
        }
    }
}
