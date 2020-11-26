using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.API.ModelDb
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        public DateTime HireDate { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public int ProvinceId { get; set; }
        [Required]
        public int DistrictId { get; set; }
        [Required]
        public int WardId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Address { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public string CreateBy { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
        [Required]
        public string ModifiedBy { get; set; }
        [Required]
        public int StatusId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
