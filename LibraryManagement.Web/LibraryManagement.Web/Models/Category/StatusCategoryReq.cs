using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.Category
{
    public class StatusCategoryReq : StatusReq
    {
        public int CategoryId { get; set; }
    }
}
