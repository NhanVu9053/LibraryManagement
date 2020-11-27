using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.Book
{
    public class StatusBookReq : StatusReq
    {
        public int BookId { get; set; }
    }
}
