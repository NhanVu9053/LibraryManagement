using LM.BAL.Interface;
using LM.DAL.Interface;
using LM.Domain.Response.ContactInfo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LM.BAL.Implement
{
    public class ContactInfoService : IContactInfoService
    {
        private readonly IContactInfoRepository contactInfoRepository;

        public ContactInfoService(IContactInfoRepository contactInfoRepository)
        {
            this.contactInfoRepository = contactInfoRepository;
        }
        public async Task<IEnumerable<District>> GetDistricts(int provinceId)
        {
            return await contactInfoRepository.GetDistricts(provinceId);
        }

        public async Task<IEnumerable<Province>> GetProvinces()
        {
            return await contactInfoRepository.GetProvinces();
        }

        public async Task<IEnumerable<Ward>> GetWards(int districtId)
        {
            return await contactInfoRepository.GetWards(districtId);
        }
    }
}
