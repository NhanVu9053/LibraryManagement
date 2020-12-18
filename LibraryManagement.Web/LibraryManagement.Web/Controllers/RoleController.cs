using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Web.Models.Role;
using LibraryManagement.Web.Ultilities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            if (Request.Cookies["roleName"] == "System Admin")
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
        [Route("/role/gets")]
        public IActionResult Gets()
        {
            if (Request.Cookies["roleName"] == "System Admin")
            {
                var students = ApiHelper<List<RoleView>>.HttpGetAsync("role/gets");
                return Json(new { data = students });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpGet]
        [Route("/role/get/{id}")]
        public IActionResult Get(string id)
        {
            if (Request.Cookies["roleName"] == "System Admin")
            {
                var student = ApiHelper<RoleView>.HttpGetAsync(@$"role/get/{id}");
                return Json(new { data = student });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPost]
        [Route("/role/create")]
        public IActionResult Create([FromBody] SaveRoleReq request)
        {
            if (Request.Cookies["roleName"] == "System Admin")
            {
                var result = ApiHelper<SaveRoleRes>.HttpAsync($"role/create", "POST", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        [Route("/role/edit")]
        public IActionResult Edit([FromBody] SaveRoleReq request)
        {
            if (Request.Cookies["roleName"] == "System Admin")
            {
                var result = ApiHelper<SaveRoleRes>.HttpAsync($"role/edit", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpDelete]
        [Route("/role/delete/{id}")]
        public IActionResult Delete(string id)
        {
            if (Request.Cookies["roleName"] == "System Admin")
            {
                var result = ApiHelper<SaveRoleRes>.HttpDeleteAsync(@$"role/delete/{id}");
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
    }
}
