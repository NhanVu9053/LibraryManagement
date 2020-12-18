using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.Category
{
    public class SaveCategoryReq : ReqModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }    
    }
}
