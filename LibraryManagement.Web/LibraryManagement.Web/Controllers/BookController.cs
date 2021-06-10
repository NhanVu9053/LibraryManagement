using LibraryManagement.Web.Models.Book;
using LibraryManagement.Web.Models.Category;
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
    public class BookController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public BookController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        [Route("book/index")]
        public IActionResult Index()
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                TempData["email"] = Request.Cookies["email"];
                TempData["avatar"] = Request.Cookies["avatar"];
                TempData["name"] = Request.Cookies["name"];
                TempData["role"] = Request.Cookies["roleName"];
                return View();
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpGet]
        [Route("/book/gets")]
        public IActionResult Gets()
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var books = ApiHelper<List<BookView>>.HttpGetAsync("book/gets");
                return Json(new { data = books });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpGet]
        [Route("/book/get/{id}")]
        public IActionResult Get(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var books = ApiHelper<BookView>.HttpGetAsync(@$"book/get/{id}");
                return Json(new { data = books });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpGet]
        [Route("/book/status/gets")]
        public IActionResult GetStatus()
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var status = ApiHelper<List<Status>>.HttpGetAsync($"wiki/status/{(int)Common.Table.Book},{false}");
                return Json(new { data = status });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPost]
        [Route("/book/save")]
        public IActionResult Save([FromForm] SaveBookReq request)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var imagePathOld = request.ImagePath;
                if (request.Image != null)
                {
                    request.ImagePath = ProcessImagePath(request.Image);
                }
                request.CreatedBy = Request.Cookies["userId"];
                request.ModifiedBy = Request.Cookies["userId"];
                var result = ApiHelper<SaveBookRes>.HttpAsync($"book/save", "POST", request);
                if (result.Message == "Thao tác tạo mới sách thành công")
                {
                    CreateImg(request.Image, request.ImagePath);
                }
                if (result.Message == "Thao tác cập nhật sách thành công")
                {
                    EditImg(request.Image, request.ImagePath, imagePathOld);
                }
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        [Route("/book/delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var request = new StatusBookReq()
                {
                    BookId = id,
                    StatusId = 4,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveBookRes>.HttpAsync($"book/changeStatus", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        [Route("/book/changeStatusToOver/{id}")]
        public IActionResult ChangeStatusToOver(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var request = new StatusBookReq()
                {
                    BookId = id,
                    StatusId = 2,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveBookRes>.HttpAsync($"book/changeStatus", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        [Route("/book/changeStatusToStochking/{id}")]
        public IActionResult ChangeStatusToStochking(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var request = new StatusBookReq()
                {
                    BookId = id,
                    StatusId = 1,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveBookRes>.HttpAsync($"book/changeStatus", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        [Route("/book/changeStatusToPending/{id}")]
        public IActionResult ChangeStatusToPending(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var request = new StatusBookReq()
                {
                    BookId = id,
                    StatusId = 3,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveBookRes>.HttpAsync($"book/changeStatus", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        public string ProcessImagePath(IFormFile file)
        {
            string fileName = null;
            if (file != null)
            {
                fileName = $"{Guid.NewGuid()}_{file.FileName}";
            }
            return fileName;
        }

        [HttpGet]
        [Route("/book/topLoan")]
        public IActionResult TopLoan()
        {
            var books = ApiHelper<List<BookView>>.HttpGetAsync("book/topLoan");
            return Json(new { data = books });
        }
        [HttpGet]
        [Route("/book/topNew")]
        public IActionResult TopNew()
        {
            var books = ApiHelper<List<BookView>>.HttpGetAsync("book/topNew");
            return Json(new { data = books });
        }
        [HttpGet]
        [Route("/book/random")]
        public IActionResult Random()
        {
            var books = ApiHelper<List<BookView>>.HttpGetAsync("book/random");
            return Json(new { data = books });
        }
        [HttpGet]
        [Route("/book/getsCategory")]
        public IActionResult GetsCategory()
        {
            var categories = ApiHelper<List<CategoryView>>.HttpGetAsync("category/gets");
            return Json(new { data = categories });
        }
        public void CreateImg(IFormFile file, string fileName)
        {
            string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "img");
            var filePath = Path.Combine(uploadFolder, fileName);
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fs);
            }
        }
        public void EditImg(IFormFile file, string fileName, string fileNameOld)
        {
            if (file != null)
            {
                string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "img");
                var filePath = Path.Combine(uploadFolder, fileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }
                if (!string.IsNullOrEmpty(fileNameOld) && (fileNameOld != "none-imgbook.png"))
                {
                    string delFile = Path.Combine(webHostEnvironment.WebRootPath
                                        , "img", fileNameOld);
                    System.IO.File.Delete(delFile);
                }
            }
        }
    }
}
