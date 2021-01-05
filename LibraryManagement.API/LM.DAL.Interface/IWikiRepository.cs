using LM.Domain.Response.Wiki;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LM.DAL.Interface
{
    public interface IWikiRepository
    {
        Task<IEnumerable<Status>> GetStatus(int tableId, bool isUpdate);
    }
}
