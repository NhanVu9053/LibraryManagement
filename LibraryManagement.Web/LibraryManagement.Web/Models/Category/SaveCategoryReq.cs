using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.Category
{
    public class SaveCategoryReq
    {
        public int CategoryId { get; set; }
        [Required]
        [Display(Name =" Tên Danh Mục")]
        public string CategoryName { get; set; }    
        [Required]
        [Display(Name ="Chọn trạng thái")]
        public int StatusId { get; set; }
    }
}
