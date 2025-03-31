using Library_Task.Server.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library_Task.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private UserManager<Reader> _userManager;

        public RegisterController(UserManager<Reader> userManager)
        {
            _userManager = userManager;
        }



        [HttpPost]
        public async Task<IActionResult> Register([FromBody] LogInModel model)
        {
            var user = new Reader { IsAdmin = model.IsAdmin?? false, Email = model.Email, Id=_userManager.Users.Count() > 0? Convert.ToString(Convert.ToInt32(_userManager.Users.Select(us => us.Id).ToList().OrderBy(Convert.ToInt32).Last()) + 1) : "2" };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {

                Claim[] userCliams =
                    [
                                new Claim(ClaimTypes.Role, model.IsAdmin is null || model.IsAdmin is false? "user" : (user.Id == "1"? "none" : "admin"))
                    ];

                await _userManager.AddClaimsAsync(user, userCliams);
                return Ok("User registered successfully!");

            }

            return BadRequest("bad request");
        }
    }
}
