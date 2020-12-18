using LM.BAL.Interface;
using LM.Domain.Request.Category;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpPatch("api/category/delete")]
        public async Task<OkObjectResult> Delete(StatusCategoryReq request)
        {
            var result = await categoryService.Delete(request);
            return Ok(result);
        }
    }
}
