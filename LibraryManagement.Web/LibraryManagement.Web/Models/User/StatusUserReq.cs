using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.User
{
    public class StatusUserReq : StatusReq
    {
        public string UserId { get; set; }
    }
}
