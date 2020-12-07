using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.LoanCard
{
    public class SaveLoanCardReq : ReqModel
    {
        public int LoanCardId { get; set; }
        public DateTime LoanOfDate { get; set; }
        public DateTime ReturnOfDate { get; set; }
        public int StudentId { get; set; }
        public string BookIds { get; set; }
    }
}
