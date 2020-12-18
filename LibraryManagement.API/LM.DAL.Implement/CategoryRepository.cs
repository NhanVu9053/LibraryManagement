using Dapper;
using LM.DAL.Interface;
using LM.Domain.Request.Category;
using LM.Domain.Response.Category;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LM.DAL.Implement
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public async Task<SaveCategoryRes> Delete(int categoryId)
        {
            var result = new SaveCategoryRes()
            {
                CategoryId = 0,
                Message = "Đã xảy ra sự cố, vui lòng liên hệ với administrator."
            };
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CategoryId", categoryId);
                parameters.Add("@ModifiedBy", "admin");
                result = await SqlMapper.QueryFirstOrDefaultAsync<SaveCategoryRes>(cnn: connection,
                                                                    sql: "sp_DeleteCategory",
                                                                    param: parameters,
                                                                    commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CategoryView> Get(int categoryId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CategoryId", categoryId);
            return await SqlMapper.QueryFirstOrDefaultAsync<CategoryView>(cnn: connection,
                                                        sql: "sp_GetCategory",
                                                        parameters,
                                                        commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CategoryView>> Gets()
        {
            return await SqlMapper.QueryAsync<CategoryView>(cnn: connection,
                                                        sql: "sp_GetCategories",
                                                        commandType: CommandType.StoredProcedure);
        }

        public async Task<SaveCategoryRes> Save(SaveCategoryReq categoryId)
        {
            var result = new SaveCategoryRes()
            {
                CategoryId = 0,
                Message = "Đã xảy ra sự cố, vui lòng liên hệ với administrator."
            };
            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CategoryId", categoryId.CategoryId);
                parameters.Add("@CategoryName", categoryId.CategoryName);
                //parameters.Add("@StatusId", categoryId.StatusId);
                parameters.Add("@CreatedBy", "admin");
                parameters.Add("@ModifiedBy", "admin");
                result =  await SqlMapper.QueryFirstOrDefaultAsync<SaveCategoryRes>(cnn: connection,
                                                            sql: "sp_SaveCategory",
                                                            parameters,
                                                            commandType: CommandType.StoredProcedure);
                return result;
            }
            catch
            {
                return result;
            }
        }
    }
}
