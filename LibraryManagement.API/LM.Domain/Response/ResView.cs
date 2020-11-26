using LM.Domain.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Response
{
    public class ResView : ReqModel
    {
        public string StatusName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateStr { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedDateStr { get; set; }
    }
}
