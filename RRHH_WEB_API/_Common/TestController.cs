using Microsoft.AspNetCore.Mvc;

namespace RRHH_WEB_API._Common
{
    [ApiController]
    [Route("api-rrhh")]
    public class TestController : ControllerBase
    {
        [HttpGet("connection")]
        public IActionResult Get()
        {
            return Ok("Success Connection!");
        }
    }
}
