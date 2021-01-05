namespace LM.Domain.Response.User
{
    public class SaveUserRes : ResultRes
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatarpath { get; set; }
        public string RoleName { get; set; }
    }
}
