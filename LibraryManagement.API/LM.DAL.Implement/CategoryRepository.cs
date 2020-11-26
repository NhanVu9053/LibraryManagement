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
                Message = "Có gì đó sai sót, xin vui lòng liên hệ Admin"
            };
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CategoryId", categoryId);
                result = await SqlMapper.QueryFirstOrDefaultAsync<SaveCategoryRes>(cnn: connection,
                                                                    sql: "sp_DeleteCategory",
                                                                    param: parameters,
                                                                    commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                return result;
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
                Message = "Something went wrong, please contact administrator."
            };
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CategoryId", categoryId.CategoryId);
            parameters.Add("@CategoryName", categoryId.CategoryName);
            parameters.Add("@StatusId", categoryId.StatusId);
            
            parameters.Add("@CreatedBy", categoryId.CreatedBy);
            return await SqlMapper.QueryFirstOrDefaultAsync<SaveCategoryRes>(cnn: connection,
                                                        sql: "sp_SaveCategory",
                                                        parameters,
                                                        commandType: CommandType.StoredProcedure);
        }

    }
}
