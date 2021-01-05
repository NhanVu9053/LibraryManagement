using Dapper;
using LM.DAL.Interface;
using LM.Domain.Request.User;
using LM.Domain.Response.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace LM.DAL.Implement
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserRepository(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public async Task<SaveUserRes> Login(LoginReq request)
        {
            var result = new SaveUserRes()
            {
                Message = "Đã xảy ra sự cố, vui lòng liên hệ với administrator."
            };
            try
            {
                var check = await CheckStatusUserIsActive(request.Email);
                if(check.Email == null)
                {
                    result.Message = check.Message;
                    return result;
                }
                var siginResult = await signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
                if (siginResult.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(request.Email);
                    var roles = await userManager.GetRolesAsync(user);
                    if (user != null)
                    {
                        result.Message = "Đăng nhập thành công";
                        result.UserId = user.Id;
                        result.Avatarpath = user.AvatarPath;
                        result.Email = user.Email;
                        result.FullName = user.FullName;
                        result.RoleName = roles.AsList<string>()[0];
                    }
                    else
                    {
                        result.Message = "Mật khẩu không đúng";
                    }
                }
                else
                {
                    result.Message = "Mật khẩu không đúng";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public async Task<UserView> Get(string id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", id);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<UserView>(cnn: connection,
                                                                                sql: "sp_GetUser",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<UserView>> Gets()
        {
            try
            {
                return await SqlMapper.QueryAsync<UserView>(cnn: connection,
                                                        sql: "sp_GetUsers",
                                                        commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SaveUserRes> Create(SaveUserReq request)
        {
            var result = new SaveUserRes()
            {
                Message = "Đã xảy ra sự cố, vui lòng liên hệ với administrator."
            };
            try
            {
                var user = ConvertObject(request);
                user.StatusId = 1;
                user.CreatedDate = DateTime.Now;
                user.CreatedBy = request.ModifiedBy;
                var check = await CheckSave(request);
                if (check.Email != null)
                {
                    var resSave = await userManager.CreateAsync(user, request.Password);
                    if (resSave.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(request.RoleId))
                        {
                            var role = await roleManager.FindByIdAsync(request.RoleId);
                            var addRoleResult = await userManager.AddToRoleAsync(user, role.Name);
                            if (addRoleResult.Succeeded)
                            {
                                result.Message = "Thao tác tạo mới Tài khoản thành công";
                                result.UserId = user.Id;
                                result.Email = user.Email;
                            }
                        }
                    }
                    else
                    {
                        result.Message = "Thao tác tạo mới Tài khoản không thành công, có thể do email hoặc password không hợp lệ";
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

        public async Task<SaveUserRes> Edit(SaveUserReq request)
        {
            var result = new SaveUserRes()
            {
                Message = "Đã xảy ra sự cố, vui lòng liên hệ với administrator."
            };
            try
            {
                var check = await CheckSave(request);
                if(check.Email != null)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserId", request.UserId);
                    parameters.Add("@Email", request.Email);
                    parameters.Add("@FullName", request.FullName);
                    parameters.Add("@Dob", request.Dob);
                    parameters.Add("@Gender", request.Gender);
                    parameters.Add("@PhoneNumber", request.PhoneNumber);
                    parameters.Add("@HireDate", request.HireDate);
                    parameters.Add("@ProvinceId", request.ProvinceId);
                    parameters.Add("@DistrictId", request.DistrictId);
                    parameters.Add("@WardId", request.WardId);
                    parameters.Add("@Address", request.Address);
                    parameters.Add("@AvatarPath", request.AvatarPath);
                    parameters.Add("@ModifiedBy", request.ModifiedBy);
                    parameters.Add("@RoleId", request.RoleId);
                    result = await SqlMapper.QueryFirstOrDefaultAsync<SaveUserRes>(cnn: connection,
                                                                        sql: "sp_UpdateUser",
                                                                        param: parameters,
                                                                        commandType: CommandType.StoredProcedure);
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

        public async Task<SaveUserRes> ChangeStatus(StatusUserReq request)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", request.UserId);
                parameters.Add("@StatusId", request.StatusId);
                parameters.Add("@ModifiedBy", request.ModifiedBy);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<SaveUserRes>(cnn: connection,
                                                                                sql: "sp_ChangeStatusUser",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ApplicationUser ConvertObject(SaveUserReq request)
        {
            var user = new ApplicationUser()
            {
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                FullName = request.FullName,
                Dob = request.Dob,
                HireDate = request.HireDate,
                Gender = request.Gender,
                ProvinceId = request.ProvinceId,
                DistrictId = request.DistrictId,
                WardId = request.WardId,
                Address = request.Address,
                AvatarPath = request.AvatarPath,
                ModifiedDate = DateTime.Now,
                ModifiedBy = request.ModifiedBy
            };
            return user;
        }
        public async Task<SaveUserRes> CheckSave(SaveUserReq request)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", request.UserId);
                parameters.Add("@Email", request.Email);
                parameters.Add("@FullName", request.FullName);
                parameters.Add("@Dob", request.Dob);
                parameters.Add("@PhoneNumber", request.PhoneNumber);
                parameters.Add("@HireDate", request.HireDate);
                parameters.Add("@ProvinceId", request.ProvinceId);
                parameters.Add("@DistrictId", request.DistrictId);
                parameters.Add("@WardId", request.WardId);
                parameters.Add("@Address", request.Address);
                parameters.Add("@AvatarPath", request.AvatarPath);
                parameters.Add("@ModifiedBy", request.ModifiedBy);
                parameters.Add("@RoleId", request.RoleId);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<SaveUserRes>(cnn: connection,
                                                                                sql: "sp_CheckSaveUser",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<SaveUserRes> CheckStatusUserIsActive(string email)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Email", email);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<SaveUserRes>(cnn: connection,
                                                                                sql: "sp_CheckStatusUserIsActive",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

