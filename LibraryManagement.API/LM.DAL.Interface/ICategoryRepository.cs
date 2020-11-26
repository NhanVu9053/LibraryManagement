using LM.Domain.Request.Category;
using LM.Domain.Response.Category;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LM.DAL.Interface
{
   public interface ICategoryRepository
    {
        Task<CategoryView> Get(int categoryId);
        Task<IEnumerable<CategoryView>> Gets();
        Task<SaveCategoryRes> Save(SaveCategoryReq categoryId);
        Task<SaveCategoryRes> Delete(int categoryId);
    }
}
