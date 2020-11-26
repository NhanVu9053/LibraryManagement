using LM.Domain.Request.LoanCard;
using LM.Domain.Response.Book;
using LM.Domain.Response.LoanCard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LM.DAL.Interface
{
    public interface ILoanCardRepository
    {
        Task<IEnumerable<LoanCardView>> Gets();
        Task<LoanCardView> Get(int id);
        Task<SaveLoanCardRes> Save(SaveLoanCardReq request);
        Task<SaveLoanCardRes> ChangeStatus(StatusLoanCardReq request);
        Task<IEnumerable<BookView>> GetBookList(int loanCardId);
    }
}
