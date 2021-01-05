namespace LM.Domain.Request.BookArchive
{
    public class SaveBookArchiveReq : ReqModel
    {
        public int BookArchiveId { get; set; }
        public int Value { get; set; }
        public bool IsPlus { get; set; }
    }

}
