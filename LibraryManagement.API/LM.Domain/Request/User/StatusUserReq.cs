using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Request.User
{
    public class StatusUserReq : StatusReq
    {
        public string UserId { get; set; }
    }
}
