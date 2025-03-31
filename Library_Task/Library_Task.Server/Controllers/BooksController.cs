using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Library_Task.Server.UseCases;
using Library_Task.Server.Policies;
using Library_Task.Server.DTO;

namespace Library_Task.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IServiceAsync<Book> _service;

        public BooksController(IServiceAsync<Book> service1)
        {
            _service = service1;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Book book)
        {
            var res  = await _service.AddAsync(book);

                return res as IActionResult;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] Book book)
        {
            var res = await _service.AlterAsync(book);

            return res as IActionResult;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Book book)
        {
            await _service.DeleteAsync(book.Id);

            return Ok();
        }

        [HttpGet("page{ind}")]
        public async Task<IActionResult> GetAll([FromRoute] int ind, [FromQuery] int pageSize, [FromQuery] string? search , [FromQuery] string? authToFilter, [FromQuery] string? genreToFilter)
        {
            var res = await _service.GetAllAsync(ind, pageSize, search, authToFilter, genreToFilter);
            return Ok(res);
        }

        [HttpPost("get")]
        public async Task<IActionResult> Get([FromBody] ResponseModel dat, [FromQuery] string id1)
        {
            
            await _service.GiveReaderBook(dat.Id, id1);

            return Ok();
        }

        [HttpPost("notif")]
        public async Task<IActionResult> Notify([FromBody] ResponseModel dat, [FromQuery] string id1)
        {
            var res = await _service.NotifyAboutReturningBook(dat.Id, id1);

            return Ok(res);
        }
    }
}
