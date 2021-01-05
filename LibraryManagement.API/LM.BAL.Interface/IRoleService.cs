using LM.Domain.Request.Role;
using LM.Domain.Response.Role;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LM.BAL.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleView>> Gets();
        Task<RoleView> Get(string id);
        Task<SaveRoleRes> Create(SaveRoleReq request);
        Task<SaveRoleRes> Edit(SaveRoleReq request);
        Task<SaveRoleRes> Delete(string id);
    }
}
