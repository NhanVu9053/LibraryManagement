namespace LibraryManagement.Web.Ultilities
{
    public static class Common
    {
        public static string apiUrl = @"https://localhost:44367/api";
        public enum Table
        {
            Book = 1,
            Student = 2,
            LoanCard = 3,
            LoanCardBook = 4,
            Category = 6,
            BookArchive = 7
        }
    }
}
