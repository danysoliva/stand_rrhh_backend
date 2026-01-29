using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRHH_WEB_API._Common;
using RRHH_WEB_API.Features.Solicitud.Dtos;

namespace RRHH_WEB_API.Features.Solicitud
{
    [ApiController]
    [Route("api-rrhh/[controller]")]
    public class SolicitudController : Controller
    {
        private readonly SolicitudService _solicitudService;
        private readonly SolicitudVacacionService _solicitudVacacionService;

        public SolicitudController(SolicitudService solicitudService, SolicitudVacacionService solicitudVacacionService)
        {
            _solicitudService = solicitudService;
            _solicitudVacacionService = solicitudVacacionService;
        }

        #region Constancia


        
        [Authorize]
        [HttpGet("obtenerConceptosConfigurables")]
        public IActionResult ObtenerConceptosConfigurables()
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.ObtenerConceptosConfigurables();
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpGet("obtenerSolicitudesDeConstanciasPorEmpleadoId")]
        public IActionResult ObtenerSolicitudesDeConstanciasPorEmpleadoId()
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.ObtenerSolicitudesDeConstanciasPorEmpleadoId(empleadoId);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpPost("guardarSolicitudDeConstancia")]
        public IActionResult GuardarSolicitudDeConstancia([FromBody] int tipoConstanciaId)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.GuardarSolicitudDeConstancia(empleadoId, tipoConstanciaId);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpPost("eliminarSolicitudDeConstancia")]
        public IActionResult EliminarSolicitudDeConstancia([FromBody] int solicitudId)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.EliminarSolicitudDeConstancia(empleadoId, solicitudId);
            return this.ActionResultFrom(response);
        }        

        [Authorize]
        [HttpGet("obtenerSolicitudesDeConstanciasPorEstadoIdParaRRHH")]
        public IActionResult ObtenerSolicitudesDeConstanciasPorEstadoId([FromQuery] int estadoId)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.ObtenerSolicitudesDeConstanciasPorEstadoId(empleadoId, estadoId);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpPost("cambiarEstadoSolicitudConstancia")]
        public IActionResult CambiarEstadoSolicitudDeConstancia([FromBody] CambioEstadoSolicitudDto cambioEstadoSolicitud)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.CambiarEstadoSolicitudDeConstancia(empleadoId, cambioEstadoSolicitud);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpGet("obtenerConstanciaParaImpresion")]
        public IActionResult ObtenerConstanciaParaImpresion([FromQuery] int solicitudConstanciaId)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.ObtenerConstanciaParaImpresion(empleadoId, solicitudConstanciaId);
            return this.ActionResultFrom(response);
        }
        #endregion


        #region Vacacion

        
        //[Authorize]
        //[HttpPost("validarFechasVacacion")]
        //public IActionResult ValidarFechasVacacion([FromBody] ValidarVacacionDto validarVacacion)
        //{
        //    IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
        //    if (empleadoId == 0) return claimsResponse;

        //    var response = _solicitudService.ValidarFechasVacacion(validarVacacion);
        //    return this.ActionResultFrom(response);
        //}

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
        [HttpGet("obtenerDiasPendientesDeVacacion")]
        public IActionResult ObtenerDiasPendientesDeVacacion()
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.ObtenerDiasPendientesDeVacacion(empleadoId);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpGet("obtenerSolicitudesDeVacacionesPorEmpleadoId")]
        public IActionResult ObtenerSolicitudesDeVacacionesPorEmpleadoId()
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.ObtenerSolicitudesDeVacacionPorEmpleadoId(empleadoId);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpPost("guardarSolicitudDeVacacion")]
        public IActionResult GuardarSolicitudDeVacacion([FromBody] NuevaSolicitudVacacionDto nuevaSolicitudVacacionDto)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.GuardarSolicitudDeVacacion(empleadoId, nuevaSolicitudVacacionDto);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpPost("eliminarSolicitudDeVacacionComoAdministrador")]
        public IActionResult EliminarSolicitudDeVacacionComoAdministrador([FromBody] int solicitudId)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.EliminarSolicitudDeVacacionComoAdministrador(empleadoId, solicitudId);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpPost("sincronizarVacacionEnOdoo")]
        public IActionResult SincronizarVacacionEnOdoo([FromBody] int solicitudId)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.SincronizarVacacionEnOdoo(empleadoId, solicitudId);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpPost("eliminarSolicitudDeVacacion")]
        public IActionResult EliminarSolicitudDeVacacion([FromBody] int solicitudId)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.EliminarSolicitudDeVacacion(empleadoId, solicitudId);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpGet("obtenerSolicitudesDeVacacionPorEstadoIdParaRRHH")]
        public IActionResult ObtenerSolicitudesDeVacacionPorEstadoId([FromQuery] int estadoId)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.ObtenerSolicitudesDeVacacionPorEstadoId(empleadoId, estadoId);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpPost("cambiarEstadoSolicitudDeVacacion")]
        public IActionResult CambiarEstadoSolicitudDeVacacion([FromBody] CambioEstadoSolicitudDto cambioEstadoSolicitud)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.CambiarEstadoSolicitudDeVacacion(empleadoId, cambioEstadoSolicitud);
            return this.ActionResultFrom(response);
        }

        [Authorize]
        [HttpGet("obtenerVacacionParaImpresion")]
        public IActionResult ObtenerVacacionParaImpresion([FromQuery] int solicitudVacacionId)
        {
            IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.ObtenerVacacionParaImpresion(empleadoId, solicitudVacacionId);
            return this.ActionResultFrom(response);
        }


        [HttpGet("obtenerTipoVacaciones")]
        public IActionResult ObtenerTipoVacaciones()
        {
            //IActionResult claimsResponse = this.GetClaim("EmpleadoId", out int empleadoId);
            //if (empleadoId == 0) return claimsResponse;

            var response = _solicitudService.ObtenerTipoVacaciones();
            return this.ActionResultFrom(response);
        }
        #endregion
    }
}
