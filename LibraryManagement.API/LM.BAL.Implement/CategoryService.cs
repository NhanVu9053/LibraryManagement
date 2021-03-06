﻿using LM.BAL.Interface;
using LM.DAL.Interface;
using LM.Domain.Request.Category;
using LM.Domain.Response.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LM.BAL.Implement
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

       
        public async Task<CategoryView> Get(int categoryId)
        {
            return await categoryRepository.Get(categoryId);
        }

        public async Task<IEnumerable<CategoryView>> Gets()
        {
            return await categoryRepository.Gets();
        }

        public async Task<SaveCategoryRes> Save(SaveCategoryReq request)
        {
            return await categoryRepository.Save(request);
        }
        public async Task<SaveCategoryRes> Delete(StatusCategoryReq request)
        {
            return await categoryRepository.Delete(request);
        }
    }
}
