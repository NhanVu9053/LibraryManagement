using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.Book
{
    public class SaveBookReq : ReqModel
    {
        public int BookId { get; set; }
        [Required(ErrorMessage = "Tên sách không được để trống")]
        [Display(Name = "Tên sách")]
        [MaxLength(200, ErrorMessage ="Tên sách quá dài so với quy định")]
        public string BookName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày xuất bản")]
        public DateTime Dop { get; set; }
        [Required(ErrorMessage = "Nhà xuất bản không được để trống")]
        [Display(Name = "Nhà xuất bản")]
        [MaxLength(200, ErrorMessage = "Nhà xuất bản quá dài so với quy định")]
        public string PublishCompany { get; set; }
        [Required(ErrorMessage = "Tác giả không được để trống")]
        [Display(Name = "Tác giả")]
        [MaxLength(200, ErrorMessage = "Tác giả quá dài so với quy định")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Số trang sách không được để trống")]
        [Display(Name = "Số trang")]
        public int Page { get; set; }
        [Required(ErrorMessage = "Mô tả sách không được để trống")]
        [Display(Name = "Mô tả")]
        [MaxLength(200, ErrorMessage = "Mô tả sách quá dài so với quy định")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Thể loại không được để trống")]
        [Display(Name = "Thể loại")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Hình ảnh sách không được để trống")]
        [Display(Name = "Hình ảnh")]
        [MaxLength(200, ErrorMessage = "Dộ dài tên file Hình ảnh sách quá dài")]
        public string ImagePath { get; set; }
        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Display(Name = "Số lượng")]
        public int Quanity { get; set; }
    }
}
