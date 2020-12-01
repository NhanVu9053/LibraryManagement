using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Web.Ultilities;
using LibraryManagement.Web.Views.ContactInfo;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers
{
    public class ContactInfoController : Controller
    {
        [HttpGet]
        [Route("/contactInfo/getProvinces")]
        public JsonResult GetProvinces()
        {
            var status = ApiHelper<List<Province>>.HttpGetAsync(@$"contactInfo/getProvinces");
            return Json(new { data = status });
        }
        [HttpGet]
        [Route("/contactInfo/getDistricts/{provinceId}")]
        public JsonResult GetDistricts(int provinceId)
        {
            var status = ApiHelper<List<District>>.HttpGetAsync(@$"contactInfo/getDistricts/{provinceId}");
            return Json(new { data = status });
        }
        [HttpGet]
        [Route("/contactInfo/getWards/{districtId}")]
        public JsonResult GetWards(int districtId)
        {
            var status = ApiHelper<List<Ward>>.HttpGetAsync(@$"contactInfo/getDistricts/{districtId}");
            return Json(new { data = status });
        }
    }
}
