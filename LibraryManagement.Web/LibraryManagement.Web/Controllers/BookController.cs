using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Web.Models.Book;
using LibraryManagement.Web.Models.Wiki;
using LibraryManagement.Web.Ultilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public BookController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var books = ApiHelper<List<BookView>>.HttpGetAsync("book/gets");
            return View(books);
        }
        [HttpGet]
        [Route("/book/gets")]
        public JsonResult Gets()
        {
            var books = ApiHelper<List<BookView>>.HttpGetAsync("book/gets");
            return Json(new { data = books });
        }
        [HttpGet]
        [Route("/book/get/{id}")]
        public JsonResult Get(int id)
        {
            var books = ApiHelper<BookView>.HttpGetAsync(@$"book/get/{id}");
            return Json(new { data = books });
        }
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
        public JsonResult Save([FromForm] SaveBookReq request)
        {
            var imagePathOld = request.ImagePath;
            if (request.Image != null)
            {
                request.ImagePath = ProcessImagePath(request.Image);
            }
            var result = ApiHelper<SaveBookRes>.HttpAsync($"book/save", "POST", request);
            if (result.Message == "Thao tác tạo mới sách thành công")
            {
                CreateImg(request.Image, request.ImagePath);
            }
            if (result.Message == "Thao tác cập nhật sách thành công")
            {
                EditImg(request.Image, request.ImagePath, imagePathOld);
            }
            TempData["Message"] = result.Message;
            return Json(new { data = result });
        }
        [HttpPatch]
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
        public IActionResult ChangeStatusToStochking(int id)
        {
            var request = new StatusBookReq()
            {
                BookId = id,
                StatusId = 1,
                ModifiedBy = "admin"
            };
            var result = ApiHelper<SaveBookRes>.HttpAsync($"book/changeStatus", "PATCH", request);
            return ResultMessage(result);
        }
        [HttpPatch]
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
        public string ProcessImagePath(IFormFile file)
        {
            string fileName = null;
            if (file != null)
            {
                fileName = $"{Guid.NewGuid()}_{file.FileName}";
            }
            return fileName;
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
