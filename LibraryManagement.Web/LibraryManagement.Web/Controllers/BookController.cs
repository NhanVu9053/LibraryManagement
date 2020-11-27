using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Web.Models.Book;
using LibraryManagement.Web.Models.Wiki;
using LibraryManagement.Web.Ultilities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            var books = ApiHelper<List<BookView>>.HttpGetAsync("book/gets");
            return View(books);
        }
        //[HttpGet]
        //[Route("/book/gets")]
        //public JsonResult Gets()
        //{
        //    var books = ApiHelper<List<BookView>>.HttpGetAsync("book/gets");
        //    return Json(new { data = books });
        //}
        //[HttpGet]
        //[Route("/book/get/{id}")]
        //public JsonResult Get(int id)
        //{
        //    var books = ApiHelper<BookView>.HttpGetAsync(@$"book/get/{id}");
        //    return Json(new { data = books });
        //}
        public IActionResult Details(int id)
        {
            var data = ApiHelper<BookView>.HttpGetAsync(@$"book/get/{id}");
            return View(data);
        }
        [HttpGet]
        [Route("/book/status/gets")]
        public JsonResult GetStatus()
        {
            var status = ApiHelper<List<Status>>.HttpGetAsync($"wiki/status/{(int)Common.Table.Book},{false}");
            return Json(new { data = status });
        }
        [HttpPost]
        [Route("/book/save")]
        public JsonResult Save([FromBody] SaveBookReq request)
        {
            var result = ApiHelper<SaveBookRes>.HttpAsync($"book/save", "POST", request);
            return Json(new { data = result });
        }
        [HttpPatch]
        //[Route("/book/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var request = new StatusBookReq()
            {
                BookId = id,
                StatusId = 4,
                ModifiedBy = "admin"
            };
            var result = ApiHelper<SaveBookRes>.HttpAsync($"book/changeStatus", "PATCH", request);
            return ResultMessage(result);
        }
        [HttpPatch]
        //[Route("/book/changeStatusToOver/{id}")]
        public IActionResult ChangeStatusToOver(int id)
        {
            var request = new StatusBookReq()
            {
                BookId = id,
                StatusId = 2,
                ModifiedBy = "admin"
            };
            var result = ApiHelper<SaveBookRes>.HttpAsync($"book/changeStatus","PATCH", request);
            return ResultMessage(result);
        }
        [HttpPatch]
        //[Route("/book/changeStatusToPending/{id}")]
        public IActionResult ChangeStatusToPending(int id)
        {
            var request = new StatusBookReq()
            {
                BookId = id,
                StatusId = 3,
                ModifiedBy = "admin"
            };
            var result = ApiHelper<SaveBookRes>.HttpAsync($"book/changeStatus", "PATCH", request);
            return ResultMessage(result);
        }
        public IActionResult ResultMessage(SaveBookRes result)
        {
            if (result != null)
            {
                TempData["Message"] = result.Message;
                return Ok(true);
            }
            TempData["Message"] = result.Message;
            return Ok(false);
        }
    }
}
