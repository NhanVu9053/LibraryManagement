using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.BookArchive
{
    public class SaveBookArchiveReq
    {
        public int BookArchiveId { get; set; }
        
        public int Value { get; set; }
        public bool IsPlus { get; set; }

        public int StatusId { get; set; }

        public string ModifiedBy { get; set; }
    }
}
