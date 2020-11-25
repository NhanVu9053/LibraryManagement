using System;
using System.Collections.Generic;
using System.Text;

namespace LM.Domain.Request.LoanCard
{
    public class StatusLoanCardReq : StatusReq
    {
        public int LoanCardId { get; set; }
    }
}
