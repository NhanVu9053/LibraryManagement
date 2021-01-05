namespace LibraryManagement.Web.Models.LoanCard
{
    public class ExtendLoanCardReq
    {
        public int LoanCardId { get; set; }
        public int DayNumber { get; set; }
        public string ModifiedBy { get; set; }
    }
}
