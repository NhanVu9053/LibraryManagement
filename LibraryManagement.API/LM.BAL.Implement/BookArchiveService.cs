using LM.BAL.Interface;
using LM.DAL.Interface;
using LM.Domain.Request.BookArchive;
using LM.Domain.Response.BookArchive;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LM.BAL.Implement
{
   public class BookArchiveService : IBookArchiveService
    {
        private readonly IBookArchiveRepository bookArchiveRepository;

        public BookArchiveService(IBookArchiveRepository bookArchiveRepository)
        {
            this.bookArchiveRepository = bookArchiveRepository;
        }

        public Task<SaveBookArchiveRes> Delete(int id)
        {
            return bookArchiveRepository.Delete(id);
        }

        public async Task<BookArchiveView> Get(int bookArchiveId)
        {
            return await bookArchiveRepository.Get(bookArchiveId);
        }

        public async Task<IEnumerable<BookArchiveView>> Gets()
        {
            return await bookArchiveRepository.Gets();
        }

        public async Task<SaveBookArchiveRes> Save(SaveBookArchiveReq bookArchiveId)
        {
            return await bookArchiveRepository.Save(bookArchiveId);
        }
    }
}
