using LM.Domain.Request.Book;
using LM.Domain.Response.Book;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LM.DAL.Interface
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookView>> Gets();
        Task<BookView> Get(int id);
        Task<SaveBookRes> Save(SaveBookReq request);
        Task<SaveBookRes> ChangeStatus(StatusBookReq request);
        Task<IEnumerable<BookView>> GetByCategoryId(int categoryId);
        Task<IEnumerable<BookView>> Search(string resultid);
        Task<IEnumerable<BookView>> GetTopLoanBook();
        Task<IEnumerable<BookView>> GetTopNewBook();
        Task<IEnumerable<BookView>> GetRandomBook();
    }
}
