using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Web.Models.Book;
using LibraryManagement.Web.Models.LoanCard;
using LibraryManagement.Web.Models.Wiki;
using LibraryManagement.Web.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LibraryManagement.Web.Controllers
{
    public class LoanCardController : Controller
    {
        private const int maxBook = 3;
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
        [Route("/loanCard/gets")]
        public IActionResult Gets()
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var loanCards = ApiHelper<List<LoanCardView>>.HttpGetAsync("loanCard/gets");
                return Json(new { data = loanCards });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpGet]
        [Route("/loanCard/get/{id}")]
        public IActionResult Get(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var loanCard = ApiHelper<LoanCardDetailView>.HttpGetAsync(@$"loanCard/get/{id}");
                var dataBook = loanCard.bookList as List<BookView>;
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataBook));
                return Json(new { data = loanCard });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPost]
        [Route("/LoanCard/Save")]
        public IActionResult Save([FromBody] SaveLoanCardReq request)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var cart = HttpContext.Session.GetString("cart");
                var bookIds = "";
                if (cart != null && cart.Length > 0 && request.StudentId > 0)
                {
                    var dataCart = JsonConvert.DeserializeObject<List<BookView>>(cart);
                    for (var i = 0; i < dataCart.Count; i++)
                    {
                        if (i == 0)
                        {
                            bookIds += dataCart[i].BookId;
                            continue;
                        }
                        bookIds += "," + dataCart[i].BookId;
                    }
                }
                if (request.LoanCardId == 0)
                {
                    request.LoanOfDate = DateTime.Now;
                    request.ReturnOfDate = DateTime.Now;
                }
                request.BookIds = bookIds;
                request.CreatedBy = Request.Cookies["userId"];
                request.ModifiedBy = Request.Cookies["userId"];
                var result = ApiHelper<SaveLoanCardRes>.HttpAsync($"loanCard/save", "POST", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        [Route("/LoanCard/ExtendLoanCard/{id}/{dayNumber}")]
        public IActionResult ExtendLoanCard(int id, int dayNumber)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var request = new ExtendLoanCardReq()
                {
                    LoanCardId = id,
                    DayNumber = dayNumber,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveLoanCardRes>.HttpAsync($"loanCard/extendLoanCard", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpPatch]
        [Route("/LoanCard/ChangeStatusToCompleted/{id}")]
        public IActionResult ChangeStatusToCompleted(int id)
        {
            if (Request.Cookies["roleName"] == "System Admin" || Request.Cookies["roleName"] == "Thủ thư")
            {
                var request = new StatusLoanCardReq()
                {
                    LoanCardId = id,
                    StatusId = 4,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveLoanCardRes>.HttpAsync($"loanCard/changeStatus", "PATCH", request);
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
                var request = new StatusLoanCardReq()
                {
                    LoanCardId = id,
                    StatusId = 5,
                    ModifiedBy = Request.Cookies["userId"]
                };
                var result = ApiHelper<SaveLoanCardRes>.HttpAsync($"loanCard/changeStatus", "PATCH", request);
                return Json(new { data = result });
            }
            else
            {
                return View("~/Views/Home/AccessDenied.cshtml");
            }
        }
        [HttpGet]
        [Route("/LoanCard/DataCartBook")]
        public IActionResult DataCartBook()
        {
            var cart = HttpContext.Session.GetString("cart");
            List<BookView> dataCart = new List<BookView>();
            if (cart != null && cart.Length != 0)
            {
                dataCart = JsonConvert.DeserializeObject<List<BookView>>(cart);
                return Json(new { data = dataCart });
            }
            return Json(new { data = dataCart, message = "Danh mục sách đang trống" });
        }
        [HttpPost]
        [Route("/LoanCard/AddCartBook/{id}")]
        public IActionResult AddCartBook(int id)
        {
            var cart = HttpContext.Session.GetString("cart");
            List<BookView> dataCart = new List<BookView>();
            if (id > 0)
            {
                var book = ApiHelper<BookView>.HttpGetAsync(@$"book/get/{id}");
                if (cart == null)
                {
                    dataCart.Add(book);
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                    return Json(new { data = dataCart });
                }
                else
                {
                    dataCart = JsonConvert.DeserializeObject<List<BookView>>(cart);
                    if(dataCart.Count >= maxBook)
                    {
                        return Json(new { data = dataCart, message = "Danh mục sách mượn vượt quá giới hạn (3 sách)" });
                    }
                    foreach (var item in dataCart)
                    {
                        if (item.BookId == id)
                        {
                            return Json(new { data = dataCart, message = "Sách đã tồn tại trong Danh mục sách mượn!" });
                        }
                    }
                    dataCart.Add(book);
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                    return Json(new { data = dataCart });
                }
            }
            return Json(new { message = "ID sách nhập vào không tồn tại!" });
        }
        [HttpPatch]
        [Route("/LoanCard/DeleteCartBook/{id}")]
        public IActionResult DeleteCartBook(int id)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<BookView> dataCart = JsonConvert.DeserializeObject<List<BookView>>(cart);

                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].BookId == id)
                    {
                        dataCart.RemoveAt(i);
                    }
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                return Ok(true);
            }
            return Ok(false);
        }
        [HttpPatch]
        [Route("/LoanCard/ResetCartBook")]
        public IActionResult ResetCartBook()
        {
            HttpContext.Session.Clear();
            return Ok(true);
        }
    }
}
