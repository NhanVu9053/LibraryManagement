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
        [Route("/bookarchive/gets")]
        public JsonResult Gets()
        {
            var bookArchive = ApiHelper<List<BookArchiveView>>.HttpGetAsync("bookarchive/gets");
            return Json(new { data = bookArchive });
        }
        [HttpGet]
        [Route("/bookArchive/get/{bookArchiveId}")]
        public JsonResult Get(int bookArchiveId)
        {
            var bookArchive = ApiHelper<BookArchiveView>.HttpGetAsync(@$"bookArchive/get/{bookArchiveId}");
            return Json(new { data = bookArchive });
        }
        [HttpPost]
        [Route("/bookarchive/save")]
        public JsonResult Save([FromBody] SaveBookArchiveReq request)
        {
            var result = ApiHelper<SaveBookArchiveRes>.HttpAsync($"bookarchive/save", "POST", request);
            return Json(new { data = result });
        }

        [HttpPatch]
        [Route("/bookarchive/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = ApiHelper<SaveBookArchiveRes>.HttpPatchAsync(@$"bookarchive/delete/{id}");
            return Json(new { data = result });
        }

        [HttpGet]
        [Route("/bookarchive/status/gets")]
        public JsonResult GetStatus()
        {
            var status = ApiHelper<List<Status>>.HttpGetAsync($"wiki/status/{(int)Common.Table.BookArchive},{ false}");
            /**/
            return Json(new { data = status });
        }
    }
}
