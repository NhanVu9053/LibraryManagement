using LibraryManagement.Web.Models.Student;
using LibraryManagement.Web.Models.Wiki;
using LibraryManagement.Web.Ultilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace LibraryManagement.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public StudentController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        
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
        [Route("/student/gets")]
        public IActionResult Gets()
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var students = ApiHelper<List<StudentView>>.HttpGetAsync("student/gets");
                return Json(new { data = students });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpGet]
        [Route("/student/get/{id}")]
        public IActionResult Get(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var student = ApiHelper<StudentView>.HttpGetAsync(@$"student/get/{id}");
                return Json(new { data = student });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpGet]
        [Route("/student/status/gets")]
        public IActionResult GetStatus()
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var status = ApiHelper<List<Status>>.HttpGetAsync($"wiki/status/{(int)Common.Table.Student},{false}");
                return Json(new { data = status });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPost]
        [Route("/student/save")]
        public IActionResult Save([FromForm] SaveStudentReq request)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var avatarPathOld = request.AvatarPath;
                if (request.Avatar != null)
                {
                    request.AvatarPath = ProcessAvatarPath(request.Avatar);
                }
                request.CreatedBy = Request.Cookies["userId"];
                request.ModifiedBy = Request.Cookies["userId"];
                var result = ApiHelper<SaveStudentRes>.HttpAsync($"student/save", "POST", request);
                if (result.Message == "Thao tác tạo mới Student thành công" && request.AvatarPath != "none-avatar.png")
                {
                    CreateAvatar(request.Avatar, request.AvatarPath);
                }
                if (result.Message == "Thao tác cập nhật Student thành công")
                {
                    EditAvatar(request.Avatar, request.AvatarPath, avatarPathOld);
                }
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        public IActionResult Delete(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var request = new StatusStudentReq()
                {
                    StudentId = id,
                    StatusId = 4,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveStudentRes>.HttpAsync($"student/changeStatus", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        [Route("/student/changeStatusToBlocked/{id}")]
        public IActionResult ChangeStatusToBlocked(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var request = new StatusStudentReq()
                {
                    StudentId = id,
                    StatusId = 3,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveStudentRes>.HttpAsync($"student/changeStatus", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        public IActionResult ChangeStatusToActive(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var request = new StatusStudentReq()
                {
                    StudentId = id,
                    StatusId = 1,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveStudentRes>.HttpAsync($"student/changeStatus", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        public string ProcessAvatarPath(IFormFile file)
        {
            string fileName = null;
            if (file != null)
            {
                fileName = $"{Guid.NewGuid()}_{file.FileName}";
            }
            return fileName;
        }
        public void CreateAvatar(IFormFile file, string fileName)
        {
            string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "img");
            var filePath = Path.Combine(uploadFolder, fileName);
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fs);
            }
        }
        public void EditAvatar(IFormFile file, string fileName, string fileNameOld)
        {
            if (file != null)
            {
                string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "img");
                var filePath = Path.Combine(uploadFolder, fileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }
                if (!string.IsNullOrEmpty(fileNameOld) && (fileNameOld != "none-avatar.png"))
                {
                    string delFile = Path.Combine(webHostEnvironment.WebRootPath
                                        , "img", fileNameOld);
                    System.IO.File.Delete(delFile);
                }
            }
        }
    }
}
