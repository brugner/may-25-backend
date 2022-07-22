using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace May25.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/tests")]
    public class TestsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}
