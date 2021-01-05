using System;

namespace LM.Domain.Request.Student
{
    public class SaveStudentReq : ReqModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public DateTime Dob { get; set; }
        public bool Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public string Address { get; set; }
        public string AvatarPath { get; set; }
    }
}
