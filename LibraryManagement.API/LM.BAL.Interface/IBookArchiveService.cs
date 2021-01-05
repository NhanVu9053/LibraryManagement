using LM.Domain.Request.BookArchive;
using LM.Domain.Response.BookArchive;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LM.BAL.Interface
{
    public interface IBookArchiveService
    {
        Task<BookArchiveView> Get(int bookArchiveId);
        Task<IEnumerable<BookArchiveView>> Gets();
        Task<SaveBookArchiveRes> Save(SaveBookArchiveReq request);
        Task<SaveBookArchiveRes> Delete(StatusBookArchiveReq request);
    }
}
