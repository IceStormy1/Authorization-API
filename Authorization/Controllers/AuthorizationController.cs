using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthorizationController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Registration()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Authorize()
        {
            return Ok();
        }
    }
}
