using Sushi.Room.Application.Services.DataModels;
using System.Collections.Generic;

namespace Sushi.Room.Web.Areas.Admin.Models.Users
{
    public class UserViewModel
    {
        public List<UserDto> Users { get; set; }
    }
}
