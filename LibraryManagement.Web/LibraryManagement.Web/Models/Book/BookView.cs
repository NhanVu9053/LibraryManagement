using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.Book
{
    public class BookView : ResView
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public DateTime Dop { get; set; }
        public string DopStr { get; set; }
        public string PublishCompany { get; set; }
        public string Author { get; set; }
        public int Page { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImagePath { get; set; }
        public int Quantity { get; set; }
        public int QuanityRemain { get; set; }
       
    }
}
