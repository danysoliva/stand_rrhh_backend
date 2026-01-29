using Microsoft.AspNetCore.Mvc;
using RRHH_WEB_API._Common;
using RRHH_WEB_API.Features.Login.Dtos;

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
    }
}
