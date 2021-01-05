using Dapper;
using LM.DAL.Interface;
using LM.Domain.Request.BookArchive;
using LM.Domain.Response.BookArchive;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace LM.DAL.Implement
{
    public class BookArchiveRepository : BaseRepository, IBookArchiveRepository
    {
        public async Task<SaveBookArchiveRes> Delete(StatusBookArchiveReq request)
        {
            var result = new SaveBookArchiveRes()
            {
                BookArchiveId = 0,
                Message = "Đã xảy ra sự cố, vui lòng liên hệ với administrator."
            };
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BookArchiveId", request.BookArchiveId);
                parameters.Add("@ModifiedBy", request.ModifiedBy);
                result = await SqlMapper.QueryFirstOrDefaultAsync<SaveBookArchiveRes>(cnn: connection,
                                                                    sql: "sp_DeleteBookArchive",
                                                                    param: parameters,
                                                                    commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
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

        public async Task<SaveBookArchiveRes> Save(SaveBookArchiveReq request)
        {
            var resut = new SaveBookArchiveRes
            {
                BookArchiveId = 0,
                Message = "Đã xảy ra sự cố, vui lòng liên hệ với administrator."
            };

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BookArchiveId", request.BookArchiveId);
                parameters.Add("@Value", request.Value);
                parameters.Add("@IsPlus", request.IsPlus);
                parameters.Add("@ModifiedBy", request.ModifiedBy);

                return await SqlMapper.QueryFirstOrDefaultAsync<SaveBookArchiveRes>(cnn: connection,
                                                            sql: "sp_UpdateBookArchive",
                                                            parameters,
                                                            commandType: CommandType.StoredProcedure);
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
