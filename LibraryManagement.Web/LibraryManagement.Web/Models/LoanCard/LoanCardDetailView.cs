using LibraryManagement.Web.Models.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.LoanCard
{
    public class LoanCardDetailView
    {
        public LoanCardView loanCard { get; set; }
        public IEnumerable<BookView> bookList { get; set; }
    }
}
