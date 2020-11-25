using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.API.ModelDb
{
    public class Wiki
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string TableName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public bool IsDelete { get; set; }
    }
}
