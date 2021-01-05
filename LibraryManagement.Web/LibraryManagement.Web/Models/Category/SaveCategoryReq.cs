namespace LibraryManagement.Web.Models.Category
{
    public class SaveCategoryReq : ReqModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }    
    }
}
