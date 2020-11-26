using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Request.Category
{
   public class SaveCategoryReq
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int StatusId { get; set; }
        
        public int CreatedBy { get; set; }
       

    }
}
