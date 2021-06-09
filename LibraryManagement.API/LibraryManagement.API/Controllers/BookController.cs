using LM.BAL.Interface;
using LM.Domain.Request.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }
        [Authorize]
        [HttpGet("api/book/gets")]
        public async Task<OkObjectResult> Gets()
        {
            var books = await bookService.Gets();
            return Ok(books);
        }
        [HttpGet("api/book/topLoan")]
        public async Task<OkObjectResult> GetTopLoan()
        {
            var books = await bookService.GetTopLoanBook();
            return Ok(books);
        }
        [HttpGet("api/book/topNew")]
        public async Task<OkObjectResult> GetTopNew()
        {
            var books = await bookService.GetTopNewBook();
            return Ok(books);
        }
        [HttpGet("api/book/random")]
        public async Task<OkObjectResult> GetRandom()
        {
            var books = await bookService.GetRandomBook();
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

        [HttpGet("api/book/getByCategory/{categoryId}")]
        public async Task<OkObjectResult> GetsBookByCategoryId(int categoryId)
        {
            var book = await bookService.GetByCategoryId(categoryId);
            return Ok(book);
        }

        [HttpGet("api/book/search/{resultid}")]
        public async Task<OkObjectResult> Search(string resultid)
        {
            var book = await bookService.Search(resultid);
            return Ok(book);
        }
    }
}
