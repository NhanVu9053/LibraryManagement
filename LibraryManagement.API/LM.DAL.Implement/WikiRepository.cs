﻿using Dapper;
using LM.DAL.Interface;
using LM.Domain.Response.Wiki;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LM.DAL.Implement
{
    public class WikiRepository : BaseRepository, IWikiRepository
    {
        public async Task<IEnumerable<Status>> GetStatus(int tableId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TableId", tableId);
            return await SqlMapper.QueryAsync<Status>(cnn: connection,
                                                        sql: "sp_GetStatus",
                                                        param: parameters,
                                                        commandType: CommandType.StoredProcedure);
        }
    }
}