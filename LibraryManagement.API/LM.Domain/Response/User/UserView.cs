using System;

namespace LM.Domain.Response.User
{
    public class UserView : ResView
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Dob { get; set; }
        public string DobStr { get; set; }
        public DateTime HireDate { get; set; }
        public string HireDateStr { get; set; }
        public bool Gender { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int WardId { get; set; }
        public string WardName { get; set; }
        public string Address { get; set; }
        public string AvatarPath { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }
    }
}
