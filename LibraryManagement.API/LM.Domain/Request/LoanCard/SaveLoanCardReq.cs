using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Request.LoanCard
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
