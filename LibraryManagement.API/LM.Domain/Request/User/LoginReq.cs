using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Request.User
{
    public class LoginReq
    {
        public string Email { get; set; }
        public string Password { get; set; }
        //public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
