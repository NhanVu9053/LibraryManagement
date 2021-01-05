using LM.Domain.Response.ContactInfo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LM.DAL.Interface
{
    public interface IContactInfoRepository
    {
        Task<IEnumerable<Province>> GetProvinces();
        Task<IEnumerable<District>> GetDistricts(int provinceId);
        Task<IEnumerable<Ward>> GetWards(int districtId);
    }
}
