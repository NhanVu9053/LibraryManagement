using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace LM.DAL.Implement
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        public DateTime Dob { get; set; }
        [Required]
        public DateTime HireDate { get; set; }
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
        public DateTime CreatedDate { get; set; }
        [Required]
        [MaxLength(450)]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
        [Required]
        [MaxLength(450)]
        public string ModifiedBy { get; set; }
        [Required]
        [MaxLength(200)]
        public string AvatarPath { get; set; }
        [Required]
        public int StatusId { get; set; }
    }
}
