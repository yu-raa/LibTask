using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Library_Task.Server.UseCases;
using Library_Task.Server.DTO;

namespace Library_Task.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IServiceAsync<Author> _service1;

        public AuthorsController(IServiceAsync<Author> service1)
        {
            _service1 = service1;
        }

       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
        [HttpPost("admin/add")]
        public async Task<IActionResult> Add([FromBody] Author author)
        {   
            await _service1.AddAsync(author);

            return Ok(author);
        }

     [HttpGet("user/page{ind}")]
        public async Task<IActionResult> Users([FromRoute] int ind, [FromQuery] int pageSize)
        {
            var res = await _service1.GetAllAsync(ind, pageSize, null, null, null);
            return Ok(res);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
        [HttpGet("admin/page{ind}")]
        public async Task<IActionResult> Admin([FromRoute] int ind, [FromQuery] int pageSize)
        {
            var res = await _service1.GetAllAsync(ind, pageSize, null, null, null);

            return Ok(res);
        }
    }
}
