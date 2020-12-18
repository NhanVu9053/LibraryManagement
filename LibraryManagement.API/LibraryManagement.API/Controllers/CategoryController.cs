using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LM.BAL.Implement;
using LM.BAL.Interface;
using LM.Domain.Request.Category;
using LM.Domain.Response.Category;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        [HttpGet("api/Category/get/{categoryId}")]
        public async Task<OkObjectResult> Get(int categoryId)
        {
            var category = await categoryService.Get(categoryId);
            return Ok(category);
        }
        [HttpGet("api/category/gets")]
        public async Task<OkObjectResult> Gets()
        {
            var category = await categoryService.Gets();
            return Ok(category);
        }
        [HttpPost, HttpPatch]
        [Route("api/category/save")]
        public async Task<OkObjectResult> Save(SaveCategoryReq request)
        {
            var result = await categoryService.Save(request);
            return Ok(result);
        }

        [HttpPatch("api/category/Delete/{id}")]
        public async Task<SaveCategoryRes> IsDelete(int id)
        {
            return await categoryService.Delete(id);
        }
    }
}
