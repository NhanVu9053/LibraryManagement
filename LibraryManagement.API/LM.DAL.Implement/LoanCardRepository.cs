using Dapper;
using LM.DAL.Interface;
using LM.Domain.Request.LoanCard;
using LM.Domain.Response.Book;
using LM.Domain.Response.LoanCard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LM.DAL.Implement
{
    public class LoanCardRepository : BaseRepository, ILoanCardRepository
    {
        public async Task<SaveLoanCardRes> ChangeStatus(StatusLoanCardReq request)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@LoanCardId", request.LoanCardId);
                parameters.Add("@StatusId", request.StatusId);
                parameters.Add("@ModifiedBy", request.ModifiedBy);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<SaveLoanCardRes>(cnn: connection,
                                                                                sql: "sp_ChangeStatusLoanCard",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SaveLoanCardRes> ExtendLoanCard(ExtendLoanCardReq request)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@LoanCardId", request.LoanCardId);
                parameters.Add("@dayNumber", request.DayNumber);
                parameters.Add("@ModifiedBy", request.ModifiedBy);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<SaveLoanCardRes>(cnn: connection,
                                                                                sql: "sp_ExtendLoanCard",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<LoanCardView> Get(int id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@LoanCardId", id);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<LoanCardView>(cnn: connection,
                                                                                sql: "sp_GetLoanCard",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BookView>> GetBookList(int loanCardId)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@LoanCardId", loanCardId);
                var result = await SqlMapper.QueryAsync<BookView>(cnn: connection,
                                                                  sql: "sp_GetBookList",
                                                                  param: parameters,
                                                                  commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<LoanCardView>> Gets()
        {
            try
            {
                return await SqlMapper.QueryAsync<LoanCardView>(cnn: connection,
                                                        sql: "sp_GetLoanCards",
                                                        commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SaveLoanCardRes> Save(SaveLoanCardReq request)
        {
            var result = new SaveLoanCardRes()
            {
                LoanCardId = 0,
                Message = "Đã xảy ra sự cố, vui lòng liên hệ với administrator."
            };
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@LoanCardId", request.LoanCardId);
                parameters.Add("@StudentId", request.StudentId);
                parameters.Add("@LoanOfDate", request.LoanOfDate);
                parameters.Add("@ReturnOfDate", request.ReturnOfDate);
                //parameters.Add("@StatusId", request.StatusId);
                parameters.Add("@CreatedBy", request.CreatedBy);
                parameters.Add("@ModifiedBy", request.ModifiedBy);
                parameters.Add("@BookIds", request.BookIds);

                result = await SqlMapper.QueryFirstOrDefaultAsync<SaveLoanCardRes>(cnn: connection,
                                                                    sql: "sp_SaveLoanCard",
                                                                    param: parameters,
                                                                    commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
