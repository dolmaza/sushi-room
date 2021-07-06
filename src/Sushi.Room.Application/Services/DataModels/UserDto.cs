using System.ComponentModel.DataAnnotations;

namespace Sushi.Room.Application.Services.DataModels
{
    public class UserDto
    {
        public int? Id { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "პაროლი")]
        public string Password { get; set; }

        [Display(Name = "სახელი")]
        public string FirstName { get; set; }

        [Display(Name = "გვარი")]
        public string LastName { get; set; }

        [Display(Name = "აქტიურია?")]
        public bool IsActive { get; set; }
    }
}
