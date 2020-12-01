using LM.Domain.Response.ContactInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LM.BAL.Interface
{
    public interface IContactInfoService
    {
        Task<IEnumerable<Province>> GetProvinces();
        Task<IEnumerable<District>> GetDistricts(int provinceId);
        Task<IEnumerable<Ward>> GetWards(int districtId);
    }
}
