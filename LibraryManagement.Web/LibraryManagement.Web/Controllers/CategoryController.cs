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
            return View();
        }
        [HttpGet]
        [Route("/category/gets")]
        public JsonResult Gets()
        {
            var categories = ApiHelper<List<CategoryView>>.HttpGetAsync("category/gets");
            return Json(new { data = categories });
        }
        [HttpGet]
        [Route("/category/get/{id}")]
        public JsonResult Get(int id)
        {
            var category = ApiHelper<CategoryView>.HttpGetAsync(@$"category/get/{id}");
            return Json(new { data = category });
        }
        [HttpPost]
        [Route("/category/save")]
        public JsonResult Save([FromBody] SaveCategoryReq request)
        {
            var result = ApiHelper<SaveCategoryRes>.HttpAsync($"category/save", "POST", request);
            return Json(new { data = result });
        }
        [HttpPatch]
        [Route("/category/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = ApiHelper<SaveCategoryRes>.HttpPatchAsync(@$"category/delete/{id}");
            return Json(new { data = result });
        }

        [HttpGet]
        [Route("/category/status/gets")]
        public JsonResult GetStatus()
        {
            var status = ApiHelper<List<Status>>.HttpGetAsync($"wiki/status/{(int)Common.Table.Category},{ false}");
            return Json(new { data = status });
        }
    }
}
