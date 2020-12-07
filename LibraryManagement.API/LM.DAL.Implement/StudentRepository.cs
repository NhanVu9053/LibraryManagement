using Dapper;
using LM.DAL.Interface;
using LM.Domain.Request.Student;
using LM.Domain.Response.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LM.DAL.Implement
{
    public class StudentRepository : BaseRepository, IStudentRepository
    {
        public async Task<SaveStudentRes> ChangeStatus(StatusStudentReq request)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@StudentId", request.StudentId);
                parameters.Add("@StatusId", request.StatusId);
                parameters.Add("@ModifiedBy", request.ModifiedBy);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<SaveStudentRes>(cnn: connection,
                                                                                sql: "sp_ChangeStatusStudent",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StudentView> Get(int id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@StudentId", id);
                var result = await SqlMapper.QueryFirstOrDefaultAsync<StudentView>(cnn: connection,
                                                                                sql: "sp_GetStudent",
                                                                                param: parameters,
                                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<StudentView>> Gets()
        {
            try
            {
                return await SqlMapper.QueryAsync<StudentView>(cnn: connection,
                                                        sql: "sp_GetStudents",
                                                        commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SaveStudentRes> Save(SaveStudentReq request)
        {
            var result = new SaveStudentRes()
            {
                StudentId = 0,
                Message = "Đã xảy ra sự cố, vui lòng liên hệ với administrator."
            };
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@StudentId", request.StudentId);
                parameters.Add("@StudentName", request.StudentName);
                parameters.Add("@CourseName", request.CourseName);
                parameters.Add("@Dob", request.Dob);
                parameters.Add("@Gender", request.Gender);
                parameters.Add("@Email", request.Email);
                parameters.Add("@PhoneNumber", request.PhoneNumber);
                //parameters.Add("@StatusId", request.StatusId);
                parameters.Add("@ProvinceId", request.ProvinceId);
                parameters.Add("@DistrictId", request.DistrictId);
                parameters.Add("@WardId", request.WardId);
                parameters.Add("@Address", request.Address);
                parameters.Add("@AvatarPath", request.AvatarPath);
                parameters.Add("@CreatedBy", "admin");
                parameters.Add("@ModifiedBy", "admin");

                result = await SqlMapper.QueryFirstOrDefaultAsync<SaveStudentRes>(cnn: connection,
                                                                    sql: "sp_SaveStudent",
                                                                    param: parameters,
                                                                    commandType: CommandType.StoredProcedure);
                return result;
            }
            catch
            {
                return result;
            }
        }
    }
}
