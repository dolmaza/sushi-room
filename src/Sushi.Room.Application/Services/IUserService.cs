using Sushi.Room.Application.Services.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sushi.Room.Application.Services
{
    public interface IUserService
    {
        Task<bool> IsUserUsernameNotUnique(string username, int? id = null);

        Task<UserDto> GetAuthorizedUserAsync(string username, string password);

        Task<int> AddNewUserAsync(UserDto userDto);

        Task UpdateUserAsync(UserDto userDto);

        Task<List<UserDto>> GetUsersAsync();

        Task<UserDto> GetSingleUserByIdAsync(int id);

        Task DeleteUserAsync(int id);

    }
}
