using System;

namespace LibraryManagement.Web.Models.LoanCard
{
    public class LoanCardView : ResView
    {
        public int LoanCardId { get; set; }
        public DateTime LoanOfDate { get; set; }
        public string LoanOfDateStr { get; set; }
        public DateTime ReturnOfDate { get; set; }
        public string ReturnOfDateStr { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public int Books { get; set; }
    }
}
