using LibraryManagement.Web.Models.User;
using LibraryManagement.Web.Ultilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace LibraryManagement.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public UserController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            if(Request.Cookies["roleName"] =="System Admin")
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
        [Route("/user/gets")]
        public IActionResult Gets()
        {
            if (Request.Cookies["roleName"] == "System Admin")
            {
                var users = ApiHelper<List<UserView>>.HttpGetAsync("user/gets");
                return Json(new { data = users });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpGet]
        [Route("/user/get/{id}")]
        public IActionResult Get(string id)
        {
            if (Request.Cookies["roleName"] == "System Admin")
            {
                var user = ApiHelper<UserView>.HttpGetAsync(@$"user/get/{id}");
                return Json(new { data = user });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPost]
        [Route("/user/login")]
        public IActionResult Login([FromBody] LoginReq request)
        {
            var result = ApiHelper<SaveUserRes>.HttpAsync($"user/login", "POST", request);
            if(result.Email != null)
            {
                Response.Cookies.Append("email", result.Email);
                Response.Cookies.Append("avatar", result.Avatarpath);
                Response.Cookies.Append("userId", result.UserId);
                Response.Cookies.Append("name", result.FullName);
                Response.Cookies.Append("roleName", result.RoleName);
            }
            return Json(new { data = result });
        }

        [HttpPost]
        [Route("/user/logOut")]
        public IActionResult LogOut()
        {
            var result = ApiHelper<LogOutRes>.HttpPostAsync("user/logOut");
            Response.Cookies.Delete("email");
            Response.Cookies.Delete("avatar");
            Response.Cookies.Delete("userId");
            Response.Cookies.Delete("name");
            Response.Cookies.Delete("roleName");
            return Json(new { data = result });
        }
        [HttpPost]
        [Route("/user/create")]
        public IActionResult Create([FromForm] SaveUserReq request)
        {
            if (Request.Cookies["roleName"] == "System Admin")
            {
                request.ModifiedBy = Request.Cookies["userId"];
                if (request.Avatar != null)
                {
                    request.AvatarPath = ProcessAvatarPath(request.Avatar);
                }
                var result = ApiHelper<SaveUserRes>.HttpAsync($"user/create", "POST", request);
                if (result.Email != null && request.AvatarPath != "none-avatar.png")
                {
                    CreateAvatar(request.Avatar, request.AvatarPath);
                }
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        [Route("/user/edit")]
        public IActionResult Edit([FromForm] SaveUserReq request)
        {
            if (Request.Cookies["roleName"] == "System Admin")
            {
                request.ModifiedBy = Request.Cookies["userId"];
                var avatarPathOld = request.AvatarPath;
                if (request.Avatar != null)
                {
                    request.AvatarPath = ProcessAvatarPath(request.Avatar);
                }
                var result = ApiHelper<SaveUserRes>.HttpAsync($"user/edit", "PATCH", request);
                if (result.Email != null)
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
        [Route("/user/changeStatusToActive/{id}")]
        public IActionResult ChangeStatusToActive(string id)
        {
            if (Request.Cookies["roleName"] == "System Admin")
            {
                var request = new StatusUserReq()
                {
                    UserId = id,
                    StatusId = 1,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveUserRes>.HttpAsync($"user/changeStatus", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        [Route("/user/changeStatusToBlocked/{id}")]
        public IActionResult ChangeStatusToBlocked(string id)
        {
            if (Request.Cookies["roleName"] == "System Admin")
            {
                var request = new StatusUserReq()
                {
                    UserId = id,
                    StatusId = 2,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveUserRes>.HttpAsync($"user/changeStatus", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        [Route("/user/delete/{id}")]
        public IActionResult Delete(string id)
        {
            if (Request.Cookies["roleName"] == "System Admin")
            {
                var request = new StatusUserReq()
                {
                    UserId = id,
                    StatusId = 4,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveUserRes>.HttpAsync($"user/changeStatus", "PATCH", request);
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
