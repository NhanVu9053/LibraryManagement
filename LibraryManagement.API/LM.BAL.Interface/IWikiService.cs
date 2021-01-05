using LM.Domain.Response.Wiki;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LM.BAL.Interface
{
    public interface IWikiService
    {
        Task<IEnumerable<Status>> GetStatus(int tableId, bool isUpdate);
    }
}
