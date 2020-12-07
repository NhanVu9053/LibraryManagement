using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.LoanCard
{
    public class ExtendLoanCardReq
    {
        public int LoanCardId { get; set; }
        public int DayNumber { get; set; }
        public string ModifiedBy { get; set; }
    }
}
