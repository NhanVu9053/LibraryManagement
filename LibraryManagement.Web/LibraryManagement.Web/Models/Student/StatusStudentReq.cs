using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.Student
{
    public class StatusStudentReq : StatusReq
    {
        public int StudentId { get; set; }
    }
}
