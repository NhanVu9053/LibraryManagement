using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Request.BookArchive
{
    public class StatusBookArchiveReq : StatusReq
    {
        public int BookArchiveId { get; set; }
    }
}
