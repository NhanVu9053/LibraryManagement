using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Request.BookArchive
{
   public class SaveBookArchiveReq
    {
        public int BookArchiveId { get; set; }
       
        public int StatusId { get; set; }
        public int Value { get; set; }
        public bool IsPlus { get; set; }
        public string ModifiedBy { get; set; }
    }
}
