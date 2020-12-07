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
            var provinces = ApiHelper<List<Province>>.HttpGetAsync(@$"contactInfo/getProvinces");
            return Json(new { data = provinces });
        }
        [HttpGet]
        [Route("/contactInfo/getDistricts/{provinceId}")]
        public JsonResult GetDistricts(int provinceId)
        {
            var districts = ApiHelper<List<District>>.HttpGetAsync(@$"contactInfo/getDistricts/{provinceId}");
            return Json(new { data = districts });
        }
        [HttpGet]
        [Route("/contactInfo/getWards/{districtId}")]
        public JsonResult GetWards(int districtId)
        {
            var wards = ApiHelper<List<Ward>>.HttpGetAsync(@$"contactInfo/getWards/{districtId}");
            return Json(new { data = wards });
        }
    }
}
