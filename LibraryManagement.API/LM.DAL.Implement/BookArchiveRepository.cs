using Dapper;
using LM.DAL.Interface;
using LM.Domain.Request.BookArchive;
using LM.Domain.Response.BookArchive;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LM.DAL.Implement
{
    public class BookArchiveRepository : BaseRepository, IBookArchiveRepository
    {
        public async Task<SaveBookArchiveRes> Delete(int id)
        {
            var result = new SaveBookArchiveRes()
            {
                BookArchiveId = 0,
                Message = "Something went wrong, please contact administrator."
            };
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@bookArchiveId", id);
                result = await SqlMapper.QueryFirstOrDefaultAsync<SaveBookArchiveRes>(cnn: connection,
                                                                    sql: "sp_IsDeleteBookArchive",
                                                                    param: parameters,
                                                                    commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public async Task<BookArchiveView> Get(int bookArchiveId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@BookArchiveId", bookArchiveId);
            return await SqlMapper.QueryFirstOrDefaultAsync<BookArchiveView>(cnn: connection,
                                                        sql: "sp_GetAllBookArchiveById",
                                                        parameters,
                                                        commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<BookArchiveView>> Gets()
        {
            return await SqlMapper.QueryAsync<BookArchiveView>(cnn: connection,
                                                       sql: "sp_GetAllBookArchive",
                                                       commandType: CommandType.StoredProcedure);
        }

        public async Task<SaveBookArchiveRes> Save(SaveBookArchiveReq bookArchiveId)
        {
            var resut = new SaveBookArchiveRes
            {
                BookArchiveId = 0,
                Message = ""
            };
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@BookArchiveId", bookArchiveId.BookArchiveId);        
            parameters.Add("@StatusId", bookArchiveId.StatusId);
            parameters.Add("@Value", bookArchiveId.Value);
            parameters.Add("@IsPlus", bookArchiveId.IsPlus);
            parameters.Add("@ModifiedBy", bookArchiveId.ModifiedBy);
            return await SqlMapper.QueryFirstOrDefaultAsync<SaveBookArchiveRes>(cnn: connection,
                                                        sql: "sp_UpdateBookArchive",
                                                        parameters,
                                                        commandType: CommandType.StoredProcedure);
        }
    }
}
