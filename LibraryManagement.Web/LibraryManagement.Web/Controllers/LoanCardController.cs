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
            var loanCards = ApiHelper<List<LoanCardView>>.HttpGetAsync("loanCard/gets");
            return View(loanCards);
        }
        public IActionResult Details(int id)
        {
            var loanCard = ApiHelper<LoanCardDetailView>.HttpGetAsync(@$"loanCard/get/{id}");
            return View(loanCard);
        }
        [HttpPatch]
        public IActionResult Delete(int id)
        {
            var request = new StatusLoanCardReq()
            {
                LoanCardId = id,
                StatusId = 5,
                ModifiedBy = "admin"
            };
            var result = ApiHelper<SaveLoanCardRes>.HttpAsync($"loanCard/changeStatus", "PATCH", request);
            return ResultMessage(result);
        }
        [HttpGet]
        [Route("/loanCard/get/{id}")]
        public JsonResult Get(int id)
        {
            var loanCard = ApiHelper<LoanCardDetailView>.HttpGetAsync(@$"loanCard/get/{id}");
            var dataBook = loanCard.bookList as List<BookView>;
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataBook));
            return Json(new { data = loanCard });
        }
        [HttpPost]
        [Route("/LoanCard/Save")]
        public JsonResult Save([FromBody] SaveLoanCardReq request)
        {
            var cart = HttpContext.Session.GetString("cart");
            var bookIds = "";
            if(cart != null && cart.Length > 0 && request.StudentId > 0)
            {
                var dataCart = JsonConvert.DeserializeObject<List<BookView>>(cart);
                for(var i = 0; i < dataCart.Count; i++)
                {
                    if (i == 0)
                    {
                        bookIds += dataCart[i].BookId;
                        continue;
                    }
                    bookIds += "," + dataCart[i].BookId;
                }
            }
            if(request.LoanCardId == 0)
            {
                request.LoanOfDate = DateTime.Now;
                request.ReturnOfDate = DateTime.Now;
            }
            request.BookIds = bookIds;
            request.CreatedBy = "admin";
            request.ModifiedBy = "admin";
            var result = ApiHelper<SaveLoanCardRes>.HttpAsync($"loanCard/save", "POST", request);
            TempData["Message"] = result.Message;
            return Json(new {data = result });
        }
        [HttpPatch]
        [Route("/LoanCard/ExtendLoanCard/{id}/{dayNumber}")]
        public IActionResult ExtendLoanCard(int id, int dayNumber)
        {
            var request = new ExtendLoanCardReq()
            {
                LoanCardId = id,
                DayNumber = dayNumber,
                ModifiedBy = "admin"
            };
            var result = ApiHelper<SaveLoanCardRes>.HttpAsync($"loanCard/extendLoanCard", "PATCH", request);
            return ResultMessage(result);
        }
        [HttpPatch]
        public IActionResult ChangeStatusToCompleted(int id)
        {
            var request = new StatusLoanCardReq()
            {
                LoanCardId = id,
                StatusId = 4,
                ModifiedBy = "admin"
            };
            var result = ApiHelper<SaveLoanCardRes>.HttpAsync($"loanCard/changeStatus", "PATCH", request);
            return ResultMessage(result);
        }
        [HttpGet]
        [Route("/LoanCard/DataCartBook")]
        public JsonResult DataCartBook()
        {
            var cart = HttpContext.Session.GetString("cart");//get key cart
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
        public JsonResult AddCartBook(int id)
        {
            var cart = HttpContext.Session.GetString("cart");//get key cart
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
        public IActionResult ResultMessage(SaveLoanCardRes result)
        {
            if (result != null)
            {
                TempData["Message"] = result.Message;
                return Ok(true);
            }
            TempData["Message"] = result.Message;
            return Ok(false);
        }
    }
}
