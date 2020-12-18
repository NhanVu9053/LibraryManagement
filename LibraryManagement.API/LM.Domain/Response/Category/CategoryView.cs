using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Response.Category
{
  public  class CategoryView : ResView
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
