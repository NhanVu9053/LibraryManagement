using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Response.Student
{
    public class StudentView : ResView
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public DateTime Dob { get; set; }
        public string DobStr { get; set; }
        public bool Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int WardId { get; set; }
        public string WardName { get; set; }
        public string Address { get; set; }
        public string AvatarPath { get; set; }
    }
}
