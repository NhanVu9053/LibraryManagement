using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Web.Models.BookArchive;
using LibraryManagement.Web.Models.Wiki;
using LibraryManagement.Web.Ultilities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers
{
    public class BookArchiveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("/bookArchive/gets")]
        public JsonResult Gets()
        {
            var bookArchive = ApiHelper<List<BookArchiveView>>.HttpGetAsync("bookArchive/gets");
            return Json(new { data = bookArchive });
        }
        [HttpGet]
        [Route("/bookArchive/get/{bookArchiveId}")]
        public JsonResult Get(int bookArchiveId)
        {
            var bookArchive = ApiHelper<BookArchiveView>.HttpGetAsync(@$"bookArchive/get/{bookArchiveId}");
            return Json(new { data = bookArchive });
        }
        [HttpGet]
        [Route("/BookArchive/Details/{bookArchiveId}")]
        public IActionResult Details(int bookArchiveId)
        {
            var bookArchive = ApiHelper<BookArchiveView>.HttpGetAsync(@$"bookArchive/get/{bookArchiveId}");
            return View(bookArchive);
        }
        [HttpPatch]
        [Route("/bookArchive/save")]
        public JsonResult Save([FromBody] SaveBookArchiveReq request)
        {
            var result = ApiHelper<SaveBookArchiveRes>.HttpAsync($"bookArchive/save", "POST", request);
            return Json(new { data = result });
        }

        [HttpPatch]
        [Route("/bookArchive/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = ApiHelper<SaveBookArchiveRes>.HttpPatchAsync(@$"bookArchive/delete/{id}");
            return Json(new { data = result });
        }
    }
}
