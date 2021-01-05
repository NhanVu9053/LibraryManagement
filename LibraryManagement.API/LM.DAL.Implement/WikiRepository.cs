using Dapper;
using LM.DAL.Interface;
using LM.Domain.Response.Wiki;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace LM.DAL.Implement
{
    public class WikiRepository : BaseRepository, IWikiRepository
    {
        public async Task<IEnumerable<Status>> GetStatus(int tableId, bool isUpdate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TableId", tableId);
            parameters.Add("@IsUpdate", isUpdate);
            return await SqlMapper.QueryAsync<Status>(cnn: connection,
                                                        sql: "sp_GetStatuses",
                                                        param: parameters,
                                                        commandType: CommandType.StoredProcedure);
        }
    }
}
