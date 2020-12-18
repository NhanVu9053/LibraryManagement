using LM.BAL.Interface;
using LM.Domain.Request.BookArchive;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    public class BookArchiveController : ControllerBase
    {
        private readonly IBookArchiveService bookArchiveService;

        public BookArchiveController(IBookArchiveService bookArchiveService)
        {
            this.bookArchiveService = bookArchiveService;
        }
        [HttpGet("api/bookArchive/get/{bookArchiveId}")]
        public async Task<OkObjectResult> Get(int bookArchiveId)
        {
            var bookArchive = await bookArchiveService.Get(bookArchiveId);
            return Ok(bookArchive);
        }
        [HttpGet("api/bookArchive/gets")]
        public async Task<OkObjectResult> Gets()
        {
            var bookArchive = await bookArchiveService.Gets();
            return Ok(bookArchive);
        }
        [HttpPost, HttpPatch]
        [Route("api/bookArchive/save")]
        public async Task<OkObjectResult> Save([FromBody] SaveBookArchiveReq request)
        {
            var result = await bookArchiveService.Save(request);
            return Ok(result);
        }

        [HttpPatch("api/bookArchive/delete")]
        public async Task<OkObjectResult> Delete(StatusBookArchiveReq request)
        {
            var result = await bookArchiveService.Delete(request);
            return Ok(result);
        }
    }
}
