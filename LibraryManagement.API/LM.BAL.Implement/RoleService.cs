using LM.BAL.Interface;
using LM.DAL.Interface;
using LM.Domain.Request.Role;
using LM.Domain.Response.Role;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LM.BAL.Implement
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<SaveRoleRes> Create(SaveRoleReq request)
        {
            return await roleRepository.Create(request);
        }

        public async Task<SaveRoleRes> Delete(string id)
        {
            return await roleRepository.Delete(id);
        }

        public async Task<SaveRoleRes> Edit(SaveRoleReq request)
        {
            return await roleRepository.Edit(request);
        }

        public async Task<RoleView> Get(string id)
        {
            return await roleRepository.Get(id);
        }

        public async Task<IEnumerable<RoleView>> Gets()
        {
            return await roleRepository.Gets();
        }
    }
}
