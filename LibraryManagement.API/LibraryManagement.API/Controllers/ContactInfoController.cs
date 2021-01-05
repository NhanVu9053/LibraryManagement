using LM.BAL.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    public class ContactInfoController : ControllerBase
    {
        private readonly IContactInfoService contactInfoService;

        public ContactInfoController(IContactInfoService contactInfoService)
        {
            this.contactInfoService = contactInfoService;
        }
        [HttpGet("api/contactInfo/getProvinces")]
        public async Task<OkObjectResult> GetProvinces()
        {
            var books = await contactInfoService.GetProvinces();
            return Ok(books);
        }
        [HttpGet("api/contactInfo/getDistricts/{provinceId}")]
        public async Task<OkObjectResult> GetDistricts(int provinceId)
        {
            var books = await contactInfoService.GetDistricts(provinceId);
            return Ok(books);
        }
        [HttpGet("api/contactInfo/getWards/{districtId}")]
        public async Task<OkObjectResult> GetWards(int districtId)
        {
            var books = await contactInfoService.GetWards(districtId);
            return Ok(books);
        }
    }
}
