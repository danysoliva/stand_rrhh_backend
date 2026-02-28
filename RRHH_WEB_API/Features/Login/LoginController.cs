using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRHH_WEB_API._Common;
using RRHH_WEB_API.Features.Login.Dtos;
using RRHH_WEB_API.Features.Maestros;
using RRHH_WEB_API.Features.Maestros.Dtos;
using System.Collections.Generic;

namespace RRHH_WEB_API.Features.Login
{
    [ApiController]
    [Route("api-rrhh")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;
        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] CredencialUsuarioDto credencialUsuarioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return this.ActionResultFrom(_loginService.Acceder(credencialUsuarioDto));
        }

        //[AllowAnonymous]
        [HttpPost("bulkActualizarPIN")]
        [Authorize]
        public IActionResult BulkActualizarPIN([FromBody] List<BulkCambiarPinDto> listaPines)
        {
            //IActionResult userResponse = this.GetClaim("EmpleadoId", out int employeeId);
            //if (employeeId == 0) return userResponse;

            var response = _loginService.BulkActualizarPIN(listaPines);
            return this.ActionResultFrom(response);
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenRequestDto refreshTokenRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return this.ActionResultFrom(_loginService.RefrescarToken(refreshTokenRequestDto));
        }
    }
}
