using LibraryManagement.Web.Models.Book;
using System.Collections.Generic;

namespace LibraryManagement.Web.Models.LoanCard
{
    public class LoanCardDetailView
    {
        public LoanCardView loanCard { get; set; }
        public IEnumerable<BookView> bookList { get; set; }
    }
}
