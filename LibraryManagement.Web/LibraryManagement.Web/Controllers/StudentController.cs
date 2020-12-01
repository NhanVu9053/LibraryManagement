using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Web.Models.Student;
using LibraryManagement.Web.Models.Wiki;
using LibraryManagement.Web.Ultilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var students = ApiHelper<List<StudentView>>.HttpGetAsync("student/gets");
            return View(students);
        }
        public IActionResult Details(int id)
        {
            var student = ApiHelper<StudentView>.HttpGetAsync(@$"student/get/{id}");
            return View(student);
        }
        [HttpGet]
        [Route("/student/status/gets")]
        public JsonResult GetStatus()
        {
            var status = ApiHelper<List<Status>>.HttpGetAsync($"wiki/status/{(int)Common.Table.Student},{false}");
            return Json(new { data = status });
        }
        [HttpPost]
        [Route("/student/save")]
        public JsonResult Save([FromForm] SaveStudentReq request)
        {
            var avatarPathOld = request.AvatarPath;
            if (request.Avatar != null)
            {
                request.AvatarPath = ProcessAvatarPath(request.Avatar);
            }
            var result = ApiHelper<SaveStudentRes>.HttpAsync($"student/save", "POST", request);
            if (result.Message == "Thao tác tạo mới Student thành công")
            {
                CreateAvatar(request.Avatar, request.AvatarPath);
            }
            if (result.Message == "Thao tác cập nhật Student thành công")
            {
                EditAvatar(request.Avatar, request.AvatarPath, avatarPathOld);
            }
            return Json(new { data = result });
        }
        [HttpPatch]
        public IActionResult Delete(int id)
        {
            var request = new StatusStudentReq()
            {
                StudentId = id,
                StatusId = 4,
                ModifiedBy = "admin"
            };
            var result = ApiHelper<SaveStudentRes>.HttpAsync($"student/changeStatus", "PATCH", request);
            return ResultMessage(result);
        }
        public IActionResult ResultMessage(SaveStudentRes result)
        {
            if (result != null)
            {
                TempData["Message"] = result.Message;
                return Ok(true);
            }
            TempData["Message"] = result.Message;
            return Ok(false);
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
