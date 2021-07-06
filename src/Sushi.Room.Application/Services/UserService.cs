using Sushi.Room.Application.Services.DataModels;
using Sushi.Room.Domain.AggregatesModel.UserAggregate;
using Sushi.Room.Domain.Exceptions;
using Sushi.Room.Domain.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sushi.Room.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsUserUsernameNotUnique(string username, int? id = null)
        {
            return await _repository.IsUserUsernameNotUnique(username, id);
        }

        public async Task<UserDto> GetAuthorizedUserAsync(string username, string password)
        {
            var user = await _repository.GetAuthorizedUser(username, password.ToSha256());

            if (user == default)
            {
                return default;
            }

            return GetUserToUserDto(user);
        }

        public async Task<int> AddNewUserAsync(UserDto userDto)
        {
            var user = User.CreateNew(userDto.UserName, userDto.Password.ToSha256(), userDto.FirstName, userDto.LastName, userDto.IsActive);

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            return user.Id;
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            var user = await _repository.FindByIdAsync(userDto.Id ?? 0);

            if (user == default)
            {
                throw new SushiRoomDomainException("მომხმარებელი ვერ მოიძებნა");
            }

            user.UpdateMetaData(userDto.UserName, userDto.Password.ToSha256(), userDto.FirstName, userDto.LastName);

            if (userDto.IsActive)
            {
                user.MarkAsActive();
            }
            else
            {
                user.MarkAsNotActive();
            }

            _repository.Update(user);

            await _repository.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _repository.FindByIdAsync(id);

            if (user == default)
            {
                throw new SushiRoomDomainException("მომხმარებელი ვერ მოიძებნა");
            }

            _repository.Remove(user);

            await _repository.SaveChangesAsync();
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var users = await _repository.GetUsersAsync();

            return users.Select(GetUserToUserDto).ToList();
        }

        public async Task<UserDto> GetSingleUserByIdAsync(int id)
        {
            var user = await _repository.FindByIdAsync(id);

            if (user == default)
            {
                return default;
            }

            return GetUserToUserDto(user);
        }

        private UserDto GetUserToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                UserName = user.UserName
            };
        }

    }
}