namespace LM.Domain.Request.LoanCard
{
    public class ExtendLoanCardReq
    {
        public int LoanCardId { get; set; }
        public int DayNumber { get; set; }
        public string ModifiedBy { get; set; }
    }
}
