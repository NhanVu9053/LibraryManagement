using LM.Domain.Request.BookArchive;
using LM.Domain.Response.BookArchive;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LM.BAL.Interface
{
   public interface IBookArchiveService
    {
        Task<BookArchiveView> Get(int bookArchiveId);
        Task<IEnumerable<BookArchiveView>> Gets();
        Task<SaveBookArchiveRes> Save(SaveBookArchiveReq bookArchiveId);
        Task<SaveBookArchiveRes> Delete(int id);
    }
}
