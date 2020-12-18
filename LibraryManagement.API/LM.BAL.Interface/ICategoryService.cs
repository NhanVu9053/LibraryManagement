using LM.Domain.Request.Category;
using LM.Domain.Response.Category;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LM.BAL.Interface
{
    public interface ICategoryService
    {
        Task<CategoryView> Get(int categoryId);
        Task<IEnumerable<CategoryView>> Gets();
        Task<SaveCategoryRes> Save(SaveCategoryReq request);
        Task<SaveCategoryRes> Delete(StatusCategoryReq request);
    }
}
