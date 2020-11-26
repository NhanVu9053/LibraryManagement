using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LM.BAL.Interface;
using LM.Domain.Request.BookArchive;
using LM.Domain.Response.BookArchive;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    public class BookArchiveController : Controller
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
        [HttpPatch]
        [Route("api/bookArchive/save")]
        public async Task<OkObjectResult> Save(SaveBookArchiveReq request)
        {
            var result = await bookArchiveService.Save(request);
            return Ok(result);
        }
        [HttpPatch("api/bookArchive/Delete/{id}")]
        public async Task<SaveBookArchiveRes> Delete(int id)
        {
            return await bookArchiveService.Delete(id);
        }
    }
}
