using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LM.BAL.Interface;
using LM.Domain.Request.Book;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }
        [HttpGet("api/book/gets")]
        public async Task<OkObjectResult> Gets()
        {
            var books = await bookService.Gets();
            return Ok(books);
        }
        [HttpGet("api/book/get/{id}")]
        public async Task<OkObjectResult> Get(int id)
        {
            var book = await bookService.Get(id);
            return Ok(book);
        }
        [HttpPost, HttpPatch]
        [Route("api/book/save")]
        public async Task<OkObjectResult> SaveCourse(SaveBookReq request)
        {
            var result = await bookService.Save(request);
            return Ok(result);
        }
        [HttpPatch]
        [Route("api/book/changeStatus")]
        public async Task<OkObjectResult> ChangeStatus(StatusBookReq request)
        {
            var result = await bookService.ChangeStatus(request);
            return Ok(result);
        }
        //[HttpPatch]
        //[Route("api/book/checkStatusBookIsOver")]
        //public void CheckStatusBookIsOver(int id)
        //{
        //    bookService.CheckStatusBookIsOver(id);
        //}
    }
}
