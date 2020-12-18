using LibraryManagement.Web.Models.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.Category
{
    public class CategoryView : ResView
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
