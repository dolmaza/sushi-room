using Sushi.Room.Domain.AggregatesModel.UserAggregate;

namespace Sushi.Room.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SushiRoomDbContext context) : base(context)
        {
        }
    }
}
