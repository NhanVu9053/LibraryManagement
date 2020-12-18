using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Request.User
{
    public class SaveUserReq
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Dob { get; set; }
        public DateTime HireDate { get; set; }
        public bool Gender { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public string Address { get; set; }
        public string ModifiedBy { get; set; }
        public string AvatarPath { get; set; }
        public string RoleId { get; set; }
    }
}
