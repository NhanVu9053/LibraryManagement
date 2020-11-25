using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Request.Student
{
    public class StatusStudentReq : StatusReq
    {
        public int StudentId { get; set; }
    }
}
