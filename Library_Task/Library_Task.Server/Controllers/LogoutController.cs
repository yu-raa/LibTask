using Library_Task.Server.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library_Task.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly SignInManager<Reader> _manager;

        public LogoutController(SignInManager<Reader> manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await _manager.SignOutAsync();
            return Ok();
        }
    }
}
