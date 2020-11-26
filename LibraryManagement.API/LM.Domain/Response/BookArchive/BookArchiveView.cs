using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Response.BookArchive
{
   public class BookArchiveView
    {
        public int BookArchiveId { get; set; }
        public int BookId { get; set; }
        public int Quanity { get; set; }
        public int QuanityRemain { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateStr { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedDateStr { get; set; }
        public string Message { get; set; }
    }
}
