using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Web.Models.Category;
using LibraryManagement.Web.Models.Wiki;
using LibraryManagement.Web.Ultilities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers
{
    public class CategoryController : Controller
    {
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
        [Route("/category/gets")]
        public IActionResult Gets()
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var categories = ApiHelper<List<CategoryView>>.HttpGetAsync("category/gets");
                return Json(new { data = categories });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpGet]
        [Route("/category/get/{id}")]
        public IActionResult Get(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var category = ApiHelper<CategoryView>.HttpGetAsync(@$"category/get/{id}");
                return Json(new { data = category });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPost]
        [Route("/category/save")]
        public IActionResult Save([FromBody] SaveCategoryReq request)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                request.CreatedBy = Request.Cookies["userId"];
                request.ModifiedBy = Request.Cookies["userId"];
                var result = ApiHelper<SaveCategoryRes>.HttpAsync($"category/save", "POST", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        [Route("/category/delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var request = new StatusCategoryReq()
                {
                    CategoryId = id,
                    StatusId = 4,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveCategoryRes>.HttpAsync($"category/delete", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }

        [HttpGet]
        [Route("/category/status/gets")]
        public IActionResult GetStatus()
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var status = ApiHelper<List<Status>>.HttpGetAsync($"wiki/status/{(int)Common.Table.Category},{ false}");
                return Json(new { data = status });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
    }
}
