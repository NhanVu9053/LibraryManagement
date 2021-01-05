using LibraryManagement.Web.Models.BookArchive;
using LibraryManagement.Web.Ultilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LibraryManagement.Web.Controllers
{
    public class BookArchiveController : Controller
    {
        [Route("bookarchive/index")]
        public IActionResult Index()
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                TempData["email"] = Request.Cookies["email"];
                TempData["avatar"] = Request.Cookies["avatar"];
                TempData["name"] = Request.Cookies["name"];
                return View();
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpGet]
        [Route("/bookArchive/gets")]
        public IActionResult Gets()
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var bookArchive = ApiHelper<List<BookArchiveView>>.HttpGetAsync("bookArchive/gets");
                return Json(new { data = bookArchive });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpGet]
        [Route("/bookArchive/get/{bookArchiveId}")]
        public IActionResult Get(int bookArchiveId)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var bookArchive = ApiHelper<BookArchiveView>.HttpGetAsync(@$"bookArchive/get/{bookArchiveId}");
                return Json(new { data = bookArchive });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpGet]
        [Route("/BookArchive/Details/{bookArchiveId}")]
        public IActionResult Details(int bookArchiveId)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var bookArchive = ApiHelper<BookArchiveView>.HttpGetAsync(@$"bookArchive/get/{bookArchiveId}");
                return View(bookArchive);
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        [Route("/bookArchive/save")]
        public IActionResult Save([FromBody] SaveBookArchiveReq request)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                request.CreatedBy = Request.Cookies["userId"];
                request.ModifiedBy = Request.Cookies["userId"];
                var result = ApiHelper<SaveBookArchiveRes>.HttpAsync($"bookArchive/save", "POST", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }

        [HttpPatch]
        [Route("/bookArchive/delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var request = new StatusBookArchiveReq()
                {
                    BookArchiveId = id,
                    StatusId = 4,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveBookArchiveRes>.HttpAsync($"bookArchive/delete", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
    }
}
