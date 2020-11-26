using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Web.Models.Category;
using LibraryManagement.Web.Ultilities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            var data = ApiHelper<List<CategoryView>>.HttpGetAsync("category/gets");
            return View(data);
        }
    }
}
