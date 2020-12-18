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
            ViewBag.ListCategory = ApiHelper<List<CategoryView>>.HttpGetAsync("category/gets");
            ViewBag.ListBook = ApiHelper<List<BookView>>.HttpGetAsync("book/gets");
            ViewBag.TopLoan= ApiHelper<List<BookView>>.HttpGetAsync("book/toploan");
            ViewBag.TopNew = ApiHelper<List<BookView>>.HttpGetAsync("book/topnew");
            return View();
        }
        [Route("Home/Category/{categoryId}")]
        public IActionResult Category(int categoryId, int? page)
        {
            TempData["checkLogin"] = Request.Cookies["email"];
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var categories = ApiHelper<List<BookView>>.HttpGetAsync($"book/getby/{categoryId}");
            ViewBag.ListCategory = ApiHelper<List<CategoryView>>.HttpGetAsync("category/gets");
            ViewBag.Category = ApiHelper<List<CategoryView>>.HttpGetAsync($"book/getby/{categoryId}");
            ViewBag.ListBook = ApiHelper<List<BookView>>.HttpGetAsync("book/gets");
            ViewBag.CategoryId = categoryId;
            var books= ApiHelper<List<BookView>>.HttpGetAsync("book/gets");
            if (categories.Count != 0)
            {
                ViewBag.title = categories.FirstOrDefault().CategoryName;
            }
            else
            {
                ViewBag.title = "Không có Danh mục";
            }
            return View(books.ToPagedList(pageNumber, pageSize));
        }

        [Route("Home/Book/get/{id}")]
        public IActionResult ViewProduct(int id)
        {
            ViewBag.ListCategory = ApiHelper<List<CategoryView>>.HttpGetAsync("category/gets");
            var book = ApiHelper<BookView>.HttpGetAsync($"book/get/{id}");
            if (book == null)
            {
                ViewBag.ErrorMessage = "Không tìm thấy sản phẩm!";
                return View("~/Views/Error/PageNotFound.cshtml");
            }

            ViewBag.Categories = ApiHelper<List<CategoryView>>.HttpGetAsync($"book/gets");
           
            return View(book);
        }
        //public IActionResult Listtype( int? page)
        //{
          
        //    int pageSize = 6;
        //    int pageNumber = (page ?? 1);
        //    var games = ApiHelper<List<BookView>>.HttpGetAsync($"Book/gets");
        //    return View(games.ToPagedList(pageNumber, pageSize));
          
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
