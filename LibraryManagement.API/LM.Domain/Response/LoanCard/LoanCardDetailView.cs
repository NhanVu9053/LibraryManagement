using LM.Domain.Response.Book;
using System.Collections.Generic;

namespace LM.Domain.Response.LoanCard
{
    public class LoanCardDetailView
    {
        public LoanCardView loanCard { get; set; }
        public IEnumerable<BookView> bookList { get; set; }
    }
}
