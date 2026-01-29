using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RRHH_WEB_API._Common;
using RRHH_WEB_API.Features.Email.Dto;

namespace RRHH_WEB_API.Features.Email
{
    [ApiController]
    [Route("api-rrhh/[controller]")]
    public class EmailController : Controller
    {
        private readonly EmailService _emailService;
        private IConfiguration _configuration;
        private EmailConfiguration emailConfiguration = new EmailConfiguration();

        public EmailController(EmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;


        }

        [HttpPost("enviarDetalleHorasPorEmpleado")]
        [Authorize]
        public IActionResult EnviarDetalleHorasPorEmpleado([FromBody] EnviarDetalleHorasParamsDto enviarDetalleHorasParamsDto)
        {
            IActionResult userResponse = this.GetClaim("EmpleadoId", out int employeeId);
            if (employeeId == 0) return userResponse;

            enviarDetalleHorasParamsDto.EmployeeId = employeeId;

            var response = _emailService.EnviarDetalleHorasPorEmpleado(enviarDetalleHorasParamsDto);
            return this.ActionResultFrom(response);
        }


        //[HttpPost("testEmail")]
        ////[Authorize]
        //public IActionResult TestEmail()
        //{
        //    //Read SMTP settings from AppSettings.json.
        //    string host = this._configuration.GetValue<string>("Smtp:Server");
        //    int port = this._configuration.GetValue<int>("Smtp:Port");
        //    string fromAddress = this._configuration.GetValue<string>("Smtp:FromAddress");
        //    string userName = this._configuration.GetValue<string>("Smtp:UserName");
        //    string password = this._configuration.GetValue<string>("Smtp:Password");


        //    var response = _emailService.TestEmail();
        //    return this.ActionResultFrom(response);
        //}
    }
}
