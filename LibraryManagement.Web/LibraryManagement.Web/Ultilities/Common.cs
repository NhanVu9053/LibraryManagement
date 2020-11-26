using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Ultilities
{
    public static class Common
    {
        public static string apiUrl = @"https://localhost:44367/api";
        public enum Table
        {
            Category = 1,
            //Module = 2,
            //Teacher = 3,
            LoanCard = 4,
            BookArchive =5
        }
    }
}
