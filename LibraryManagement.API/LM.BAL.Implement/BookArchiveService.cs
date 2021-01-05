using LM.BAL.Interface;
using LM.DAL.Interface;
using LM.Domain.Request.BookArchive;
using LM.Domain.Response.BookArchive;
using System.Collections.Generic;
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

        public Task<SaveBookArchiveRes> Delete(StatusBookArchiveReq request)
        {
            return bookArchiveRepository.Delete(request);
        }

        public async Task<BookArchiveView> Get(int bookArchiveId)
        {
            return await bookArchiveRepository.Get(bookArchiveId);
        }

        public async Task<IEnumerable<BookArchiveView>> Gets()
        {
            return await bookArchiveRepository.Gets();
        }

        public async Task<SaveBookArchiveRes> Save(SaveBookArchiveReq request)
        {
            return await bookArchiveRepository.Save(request);
        }
    }
}
