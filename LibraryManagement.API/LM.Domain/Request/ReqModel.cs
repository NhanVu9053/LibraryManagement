using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Request
{
    public class ReqModel
    {
        public int StatusId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
