using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Request.Book
{
    public class StatusBookReq : StatusReq
    {
        public int BookId { get; set; }
    }
}
