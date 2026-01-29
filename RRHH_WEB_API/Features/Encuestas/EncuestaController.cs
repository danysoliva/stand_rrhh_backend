using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRHH_WEB_API._Common;
using RRHH_WEB_API.Features.Encuestas;
using RRHH_WEB_API.Features.Encuestas.Dto;


namespace RRHH_WEB_API.Features.Encuestas
{
        [ApiController]
        [Route("api-rrhh/[controller]")]
    public class EncuestaController : Controller
    {
        private readonly EncuestaService _encuestaService;

        public EncuestaController(EncuestaService encuestaService)
        {
            _encuestaService = encuestaService;
        }


        [HttpPost("saveEncuestaCreator")]
        public IActionResult GuardarDediccionPlanilla([FromBody]  EncuestaSaveParamsDto encuesta)
        {
            var response = _encuestaService.SaveEncuestaCreator(encuesta);
            return this.ActionResultFrom(response);
        }

        [HttpGet("getEncuesta/{id}")]
        public IActionResult GetEncuesta(int id)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _encuestaService.EncuestaView(id,empleadoId);
            return this.ActionResultFrom(response);
        }


        [HttpPost("saveEncuestaComplete")]
        [Authorize]
        public IActionResult GuardarEncuestaRespuestas([FromBody] System.Collections.Generic.List<EncuestaAnswerDto> respuestas)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _encuestaService.GuardarEncuesta(respuestas, empleadoId);
            return this.ActionResultFrom(response);
        }


        [HttpGet("obtenerEncuestas")]
        public IActionResult GetEmployees()
        {
            var response = _encuestaService.GetEncuestas();
            return this.ActionResultFrom(response);
        }


        [HttpGet("cerrarEncuesta/{encuestaId}")]
        public IActionResult CerrarEncuesta(int encuestaId)
        {
            var response = _encuestaService.CerrarEncuesta(encuestaId);
            return this.ActionResultFrom(response);
        }


        [HttpGet("obtenerFiltrosEncuestas")]
        [Authorize]
        public IActionResult ObtenerFiltrosDeEncuesta()
        {
            var response = _encuestaService.ObtenerEncuestaFiltros();
            return this.ActionResultFrom(response);
        }

        [HttpGet("obtenerEncuestasFiltradasPorEstado/{estadoId}")]
        [Authorize]
        public IActionResult ObtenerEncuestasFiltradasPorEstado(int estadoId)
        {
            var response = _encuestaService.ObtenerEncuestaFiltrosPorEstado(estadoId);
            return this.ActionResultFrom(response);
        }


        [HttpGet("obtenerEncuestaTabulacion/{encuestaId}")]
        //[Authorize]
        public IActionResult ObtenerEncuestaTabulacion(int encuestaId)
        {
            var response = _encuestaService.ObtenerTabulacionEncuesta(encuestaId);
            return this.ActionResultFrom(response);
        }


    }
}
