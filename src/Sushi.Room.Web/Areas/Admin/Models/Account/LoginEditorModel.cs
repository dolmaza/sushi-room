namespace Sushi.Room.Web.Areas.Admin.Models.Account
{
    public class LoginEditorModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool HasErrors => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }
    }
}
