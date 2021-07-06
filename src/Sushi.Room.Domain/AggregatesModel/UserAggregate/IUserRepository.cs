using Sushi.Room.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sushi.Room.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> GetUsersAsync();

        Task<bool> IsUserUsernameNotUnique(string username, int? id = null);

        Task<User> GetAuthorizedUser(string username, string password);
    }
}
