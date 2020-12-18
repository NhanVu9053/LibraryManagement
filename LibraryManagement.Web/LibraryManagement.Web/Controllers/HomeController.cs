using LibraryManagement.Web.Models;
using LibraryManagement.Web.Models.Book;
using LibraryManagement.Web.Models.Category;
using LibraryManagement.Web.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using X.PagedList;

namespace LibraryManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            TempData["checkLogin"] = Request.Cookies["email"];
            return View();
        }
        [Route("Home/GetBookByCategory/{categoryId}")]
        public IActionResult GetBookByCategory(int categoryId, int? page)
        {
            TempData["checkLogin"] = Request.Cookies["email"];
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var books = ApiHelper<List<BookView>>.HttpGetAsync($"book/getByCategory/{categoryId}");
            if(books.Count != 0)
            {
                ViewBag.Title = books[0].CategoryName;
            }
            return View(books.ToPagedList(pageNumber, pageSize));
        }

        [Route("Home/ViewProduct/{id}")]
        public IActionResult ViewProduct(int id)
        {
            ViewBag.ListCategory = ApiHelper<List<CategoryView>>.HttpGetAsync("category/gets");
            var book = ApiHelper<BookView>.HttpGetAsync($"book/get/{id}");
            if (book == null)
            {
                ViewBag.ErrorMessage = "Không tìm thấy sản phẩm!";
                return View("~/Views/Error/PageNotFound.cshtml");
            }           
            return View(book);
        }
        
        public IActionResult Search(string resultid, int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            ViewBag.ListCategory = ApiHelper<List<CategoryView>>.HttpGetAsync($"book/search/{resultid}");
            ViewBag.search = resultid;
           var result= ApiHelper<List<BookView>>.HttpGetAsync($"book/search/{resultid}");
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
