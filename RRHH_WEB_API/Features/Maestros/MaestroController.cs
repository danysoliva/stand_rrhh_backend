using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRHH_WEB_API._Common;
using RRHH_WEB_API.Features.GestionesVarias.Dto;
using RRHH_WEB_API.Features.Maestros.Dtos;

namespace RRHH_WEB_API.Features.Maestros
{
    [ApiController]
    [Route("api-rrhh/[controller]")]
    public class MaestroController : Controller
    {
        private readonly MaestroService _maestroService;

        public MaestroController(MaestroService maestroService)
        {
            _maestroService = maestroService;
        }

        [HttpGet("getEmployeeProfile")]
        [Authorize]
        public IActionResult GetEmployeeProfile()
        {
            IActionResult userResponse = this.GetClaim("EmpleadoId", out int employeeId);
            if (employeeId == 0) return userResponse;

            ////int employeeId = 367;

            var response = _maestroService.GetEmployee(employeeId);
            return this.ActionResultFrom(response);
        }

        [HttpPost("getEmployee")]
        public IActionResult GetEmployee([FromQuery] int id)
        {
            var response = _maestroService.GetEmployee(id);
            return this.ActionResultFrom(response);
        }

        [HttpGet("getVoucher")]
        [Authorize]
        public IActionResult GetVoucher([FromQuery] int payslipRunId)
        {
            IActionResult userResponse = this.GetClaim("EmpleadoId", out int employeeId);
            if (employeeId == 0) return userResponse;

            //int employeeId = 367;

            var response = _maestroService.GetVoucher(employeeId, payslipRunId);
            return this.ActionResultFrom(response);
        }

        [HttpGet("sendVoucher")]
        [Authorize]
        public IActionResult SendVoucher([FromQuery] int payslipRunId)
        {
            IActionResult userResponse = this.GetClaim("EmpleadoId", out int employeeId);
            if (employeeId == 0) return userResponse;

            //int employeeId = 367;

            var response = _maestroService.SendEmailVoucher(employeeId, payslipRunId);
            return this.ActionResultFrom(response);
        }

        [HttpPost("getNominaEncabezado")]
        [Authorize]
        public IActionResult GetNominaEncabezado()
        {
            IActionResult userResponse = this.GetClaim("EmpleadoId", out int employeeId);
            if (employeeId == 0) return userResponse;

            var response = _maestroService.GetNominaEncabezado(employeeId);
            return this.ActionResultFrom(response);
        }


        [HttpPost("getDetalleHorasEmpleado")]
        //[Authorize]
        public IActionResult GetDetalleHoras([FromBody] RangoFechaHorasEmpleadoParamsDto horasEmpleado)
        {
            IActionResult userResponse = this.GetClaim("EmpleadoId", out int employeeId);
            if (employeeId == 0) return userResponse;

            horasEmpleado.EmployeeId = employeeId;


            var response = _maestroService.DetalleHorasEmpleado(horasEmpleado);
            return this.ActionResultFrom(response);
        }


        [HttpGet("obtenerRolesUsuarios")]
        public IActionResult ObtenerRoles()
        {
            var response = _maestroService.ObtenerRolesUsuarios();
            return this.ActionResultFrom(response);
        }


        [HttpPost("cambiarRolUsuario")]
        [Authorize]
        public IActionResult CambiarRolUsuario([FromBody] RolUsuarioParamsDto rolParam)
        {
            IActionResult userResponse = this.GetClaim("EmpleadoId", out int employeeId);
            if (employeeId == 0) return userResponse;

            var response = _maestroService.CambiarRolUsuario(rolParam);
            return this.ActionResultFrom(response);
        }

        [HttpPost("cambiarPIN")]
        [Authorize]
        public IActionResult CambiarPIN([FromBody] CambiarPinDto cambiarPin)
        {
            IActionResult userResponse = this.GetClaim("EmpleadoId", out int employeeId);
            if (employeeId == 0) return userResponse;

            var response = _maestroService.CambiarPIN(employeeId, cambiarPin.NuevoPin);
            return this.ActionResultFrom(response);
        }

        [HttpPost("cambiarPinDeEmpleado")]
        [Authorize]
        public IActionResult CambiarPinDeEmpleado([FromBody] CambiarPinDto cambiarPin)
        {
            IActionResult userResponse = this.GetClaim("EmpleadoId", out int employeeId);
            if (employeeId == 0) return userResponse;

            var response = _maestroService.CambiarPIN(cambiarPin.EmployeeId, cambiarPin.NuevoPin);
            return this.ActionResultFrom(response);
        }

    }
}
