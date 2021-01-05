using LM.BAL.Interface;
using LM.Domain.Request.Role;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }
        [HttpGet("api/role/gets")]
        public async Task<OkObjectResult> Gets()
        {
            var roles = await roleService.Gets();
            return Ok(roles);
        }
        [HttpGet("api/role/get/{id}")]
        public async Task<OkObjectResult> Get(string id)
        {
            var role = await roleService.Get(id);
            return Ok(role);
        }
        [HttpPost]
        [Route("/api/role/create")]
        public async Task<OkObjectResult> Create(SaveRoleReq request)
        {
            var result = await roleService.Create(request);
            return Ok(result);
        }
        [HttpPatch]
        [Route("/api/role/edit")]
        public async Task<OkObjectResult> Edit(SaveRoleReq request)
        {
            var result = await roleService.Edit(request);
            return Ok(result);
        }
        [HttpDelete]
        [Route("/api/role/delete/{id}")]
        public async Task<OkObjectResult> Delete(string id)
        {
            var result = await roleService.Delete(id);
            return Ok(result);
        }
    }
}
