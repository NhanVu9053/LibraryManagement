using Dapper;
using LM.DAL.Interface;
using LM.Domain.Request.Book;
using LM.Domain.Response.Book;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LM.DAL.Implement
{
    public class BookRepository : BaseRepository, IBookRepository
    {
        public async Task<SaveBookRes> ChangeStatus(StatusBookReq request)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BookId", request.BookId);
                parameters.Add("@StatusId", request.StatusId);
                parameters.Add("@ModifiedBy", request.ModifiedBy);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<SaveBookRes>(cnn: connection,
                                                                                sql: "sp_ChangeStatusBook",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BookView> Get(int id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BookId", id);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<BookView>(cnn: connection,
                                                                                sql: "sp_GetBook",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BookView>> GetByCategoryId(int categoryId)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CategoryId", categoryId);
                var result = await SqlMapper.QueryAsync<BookView>(cnn: connection,
                                                                                sql: "sp_GetBookByCategoryId",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BookView>> GetRandomBook()
        {
            try
            {
                return await SqlMapper.QueryAsync<BookView>(cnn: connection,
                                                        sql: "sp_GetRandomBook",
                                                        commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BookView>> Gets()
        {
            try
            {
                return await SqlMapper.QueryAsync<BookView>(cnn: connection,
                                                        sql: "sp_GetBooks",
                                                        commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BookView>> GetTopLoanBook()
        {
            try
            {
                return await SqlMapper.QueryAsync<BookView>(cnn: connection,
                                                        sql: "sp_GetTopLoanBook",
                                                        commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BookView>> GetTopNewBook()
        {
            try
            {
                return await SqlMapper.QueryAsync<BookView>(cnn: connection,
                                                        sql: "sp_GetTopNew",
                                                        commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SaveBookRes> Save(SaveBookReq request)
        {
            var result = new SaveBookRes()
            {
                BookId = 0,
                Message = "Đã xảy ra sự cố, vui lòng liên hệ với administrator."
            };
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BookId", request.BookId);
                parameters.Add("@BookName", request.BookName);
                parameters.Add("@Dop", request.Dop);
                parameters.Add("@PublishCompany", request.PublishCompany);
                parameters.Add("@Author", request.Author);
                parameters.Add("@Page", request.Page);
                parameters.Add("@Description", request.Description);
                parameters.Add("@CategoryId", request.CategoryId);
                //parameters.Add("@StatusId", request.StatusId);
                parameters.Add("@ImagePath", request.ImagePath);
                parameters.Add("@Quantity", request.Quantity);
                parameters.Add("@CreatedBy", "admin");
                parameters.Add("@ModifiedBy", "admin");

                result = await SqlMapper.QueryFirstOrDefaultAsync<SaveBookRes>(cnn: connection,
                                                                    sql: "sp_SaveBook",
                                                                    param: parameters,
                                                                    commandType: CommandType.StoredProcedure);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public async Task<IEnumerable<BookView>> Search(string resultid)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Search", resultid);
                var result = await SqlMapper.QueryAsync<BookView>(cnn: connection,
                                                                                sql: "sp_SearchBook",
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
