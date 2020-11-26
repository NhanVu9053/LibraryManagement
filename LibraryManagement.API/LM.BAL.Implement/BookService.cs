using LM.BAL.Interface;
using LM.DAL.Interface;
using LM.Domain.Request.Book;
using LM.Domain.Response.Book;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LM.BAL.Implement
{
    public class BookService : IBookService
    {
        private readonly IBookRepository bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }
        public async Task<SaveBookRes> ChangeStatus(StatusBookReq request)
        {
            return await bookRepository.ChangeStatus(request);
        }

        public async Task<BookView> Get(int id)
        {
            return await bookRepository.Get(id);
        }

        public async Task<IEnumerable<BookView>> Gets()
        {
            return await bookRepository.Gets();
        }

        public async Task<SaveBookRes> Save(SaveBookReq request)
        {
            return await bookRepository.Save(request);
        }
    }
}
