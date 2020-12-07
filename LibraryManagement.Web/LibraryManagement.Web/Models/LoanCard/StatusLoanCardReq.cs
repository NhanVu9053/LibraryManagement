using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.LoanCard
{
    public class StatusLoanCardReq : StatusReq
    {
        public int LoanCardId { get; set; }
    }
}
