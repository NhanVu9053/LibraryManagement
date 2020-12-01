using Dapper;
using LM.DAL.Interface;
using LM.Domain.Response.ContactInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LM.DAL.Implement
{
    public class ContactInfoRepository : BaseRepository, IContactInfoRepository
    {

        public async Task<IEnumerable<District>> GetDistricts(int provinceId)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProvinceId", provinceId);
                var result = await SqlMapper.QueryAsync<District>(cnn: connection,
                                                                sql: "sp_GetDistricts",
                                                                param: parameters,
                                                                commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Province>> GetProvinces()
        {
            try
            {
                return await SqlMapper.QueryAsync<Province>(cnn: connection,
                                                        sql: "sp_GetProvinces",
                                                        commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public async Task<IEnumerable<Ward>> GetWards(int districtId)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DistrictId", districtId);
                var result = await SqlMapper.QueryAsync<Ward>(cnn: connection,
                                                                sql: "sp_GetWards",
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
