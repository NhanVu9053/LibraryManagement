using Dapper;
using LM.DAL.Interface;
using LM.Domain.Request.Role;
using LM.Domain.Response.Role;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LM.DAL.Implement
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager,
                              UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<SaveRoleRes> Create(SaveRoleReq request)
        {
            var result = new SaveRoleRes()
            {
                Message = "Đã xảy ra sự cố, vui lòng liên hệ với administrator."
            };
            try
            {
                var check = await CheckSave(request);
                if(check.RoleName != null)
                {
                    var resRole = await roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = request.RoleName
                    });
                    if (resRole.Succeeded)
                    {
                        result.Message = "Thao tác tạo mới Role thành công";
                        result.RoleName = request.RoleName;
                    }
                    else
                    {
                        result.Message = "Thao tác tạo mới Role không thành công";
                    }
                }
                else
                {
                    result.Message = check.Message;
                }
                
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SaveRoleRes> Edit(SaveRoleReq request)
        {
            var result = new SaveRoleRes()
            {
                Message = "Đã xảy ra sự cố, vui lòng liên hệ với administrator."
            };
            try
            {
                var role = await roleManager.FindByIdAsync(request.RoleId);
                if (role != null)
                {
                    var check = await CheckSave(request);
                    if(check.RoleName != null)
                    {
                        role.Name = request.RoleName;
                        var resRolet = await roleManager.UpdateAsync(role);
                        if (resRolet.Succeeded)
                        {
                            result.Message = "Cập nhật thành công Role";
                            result.RoleName = role.Name;
                        }
                        else
                        {
                            result.Message = "Thao tác cập nhật không thành công Role";
                        }
                    }
                    else
                    {
                        result.Message = check.Message;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RoleView> Get(string id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@RoleId", id);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<RoleView>(cnn: connection,
                                                                                sql: "sp_GetRole",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<RoleView>> Gets()
        {
            try
            {
                return await SqlMapper.QueryAsync<RoleView>(cnn: connection,
                                                        sql: "sp_GetRoles",
                                                        commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<SaveRoleRes> CheckSave(SaveRoleReq request)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@RoleId", request.RoleId);
                parameters.Add("@RoleName", request.RoleName);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<SaveRoleRes>(cnn: connection,
                                                                                sql: "sp_CheckSaveRole",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SaveRoleRes> Delete(string id)
        {
            var result = new SaveRoleRes()
            {
                Message = "Đã xảy ra sự cố, vui lòng liên hệ với administrator."
            };
            try
            {
                var deleteRole = await roleManager.FindByIdAsync(id);
                if (deleteRole != null)
                {
                    var userInRole = await userManager.GetUsersInRoleAsync(deleteRole.Name);
                    if (userInRole.Count > 0)
                    {
                        result.Message = $"Thao tác xóa Role name: * {deleteRole.Name} * không thành công. Phải xóa tài khoản User dùng Role name: * {deleteRole.Name} * trước.";
                    }
                    else
                    {
                        var resDel = await roleManager.DeleteAsync(deleteRole);
                        if (resDel.Succeeded)
                        {
                            result.Message = $"Bạn đã xóa thành công Role name: * {deleteRole.Name} *";
                            result.RoleName = deleteRole.Name;
                        }
                        else
                        {
                            result.Message = $"Thao tác xóa Role name: * {deleteRole.Name} * không thành công";
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
