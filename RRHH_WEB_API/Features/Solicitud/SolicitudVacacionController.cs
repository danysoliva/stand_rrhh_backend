using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRHH_WEB_API._Common;
using RRHH_WEB_API.Features.Solicitud.Dtos;

namespace RRHH_WEB_API.Features.Solicitud
{
    [ApiController]
    [Route("api-rrhh/[controller]")]
    public class SolicitudVacacionController : Controller
    {
        private readonly SolicitudVacacionService _solicitudVacacionService;

        public SolicitudVacacionController(SolicitudVacacionService solicitudVacacionService)
        {
            _solicitudVacacionService = solicitudVacacionService;
        }
        
        [Authorize]
        [HttpPost("validarFechasVacacion")]
        public IActionResult ValidarFechasVacacion([FromBody] ValidarVacacionDto validarVacacion)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudVacacionService.ValidarFechasVacacion(validarVacacion);
            return this.ActionResultFrom(response);
        }


        [Authorize]
        [HttpGet("obtenerSolicitudesDeVacacionesPorEmpleadoId")]
        public IActionResult ObtenerSolicitudesDeVacacionesPorEmpleadoId()
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudVacacionService.ObtenerSolicitudesDeVacacionPorEmpleadoId(empleadoId);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpPost("guardarSolicitudDeVacacion")]
        public IActionResult GuardarSolicitudDeVacacion([FromBody] NuevaSolicitudVacacionDto nuevaSolicitudVacacionDto)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudVacacionService.GuardarSolicitudDeVacacion(empleadoId, nuevaSolicitudVacacionDto);
            return this.ActionResultFrom(response);
        }


        [Authorize]
        [HttpPost("eliminarSolicitudDeVacacion")]
        public IActionResult EliminarSolicitudDeVacacion([FromBody] int solicitudId)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudVacacionService.EliminarSolicitudDeVacacion(empleadoId, solicitudId);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpGet("obtenerSolicitudesDeVacacionPorEstadoIdParaRRHH")]
        public IActionResult ObtenerSolicitudesDeVacacionPorEstadoId([FromQuery] int estadoId)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudVacacionService.ObtenerSolicitudesDeVacacionPorEstadoId(empleadoId, estadoId);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpPost("cambiarEstadoSolicitudDeVacacion")]
        public IActionResult CambiarEstadoSolicitudDeVacacion([FromBody] CambioEstadoSolicitudDto cambioEstadoSolicitud)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudVacacionService.CambiarEstadoSolicitudDeVacacion(empleadoId, cambioEstadoSolicitud);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpGet("obtenerVacacionParaImpresion")]
        public IActionResult ObtenerVacacionParaImpresion([FromQuery] int solicitudVacacionId)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudVacacionService.ObtenerVacacionParaImpresion(empleadoId, solicitudVacacionId);
            return this.ActionResultFrom(response);
        }
    }
}
