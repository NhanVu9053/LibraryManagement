using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.User
{
    public class SaveUserRes
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatarpath { get; set; }
        //public string ReturnUrl { get; set; }
        public string Message { get; set; }
        public string RoleName { get; set; }
    }
}
