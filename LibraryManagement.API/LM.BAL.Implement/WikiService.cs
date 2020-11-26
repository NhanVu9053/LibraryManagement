using LM.BAL.Interface;
using LM.DAL.Interface;
using LM.Domain.Response.Wiki;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LM.BAL.Implement
{
   public class WikiService : IWikiService
    {
        private readonly IWikiRepository wikiRepository;

        public WikiService(IWikiRepository wikiRepository)
        {
            this.wikiRepository = wikiRepository;
        }

        public async Task<IEnumerable<Status>> GetStatus(int tableId)
        {
            return await wikiRepository.GetStatus(tableId);
        }
    }
}
