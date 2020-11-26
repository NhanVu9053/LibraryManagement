using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Response.Category
{
  public  class CategoryView
    {
        public  int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }       
        public string CreatedDateStr { get; set; }
        public string ModifiedDateStr { get; set; }
    }
}
