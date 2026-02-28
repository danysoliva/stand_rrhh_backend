using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Maestros
{
    [ApiController]
    [Route("api-rrhh")]
    public class EmployeePictureController : ControllerBase
    {
        private readonly EmployeePictureService _employeePictureService;

        public EmployeePictureController(EmployeePictureService employeePictureService)
        {
            _employeePictureService = employeePictureService;
        }

        [HttpGet("employee/picture/{idEmployee}")]
        public async Task<IActionResult> GetEmployeePicture(int idEmployee)
        {
            var imageBytes = await _employeePictureService.GetEmployeePicture(idEmployee);

            if (imageBytes == null)
            {
                return NotFound();
            }

            // Assuming images are jpeg for now, but this could be dynamic based on extension
            return File(imageBytes, "image/jpeg");
        }
    }
}
