namespace LibraryManagement.Web.Models.BookArchive
{
    public class BookArchiveView : ResView
    {
        public int BookArchiveId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public int QuantityRemain { get; set; }
        public string ImagePath { get; set; }
    }
}
