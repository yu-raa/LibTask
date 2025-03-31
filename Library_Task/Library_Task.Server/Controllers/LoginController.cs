using Library_Task.Server.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Modsen_Library_Test_Task.Entities;
using System.Security.Claims;
using System.Text;

namespace Library_Task.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<DatabaseUser> _userManager;
        private readonly SignInManager<DatabaseUser> _signInManager;
        private readonly IConfiguration _config;

        public LoginController(UserManager<DatabaseUser> userManager,
                      SignInManager<DatabaseUser> signInManager,
                      IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LogInModel model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);

            if (User == null)
            {
                return NotFound("User does not exist");
            }

            var result = await _signInManager.PasswordSignInAsync(User, model.Password, true, false);

            if (result.Succeeded)
            {

                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:secret"]);
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var role = identity?.FindFirst(ClaimTypes.Role)?.Value;

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("role", role as string) }),
                    Expires = DateTime.UtcNow.AddDays(14),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new ResponseModel(tokenString, User.Id, role));
            }

            return BadRequest("bad request");
        }
    }
}
