using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models
{
    public class StatusReq
    {
        public int StatusId { get; set; }
        public string ModifiedBy { get; set; }
    }
}
