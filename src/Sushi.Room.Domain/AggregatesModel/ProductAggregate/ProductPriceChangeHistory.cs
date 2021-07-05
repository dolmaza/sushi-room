using Sushi.Room.Domain.AggregatesModel.UserAggregate;
using Sushi.Room.Domain.SeedWork;

namespace Sushi.Room.Domain.AggregatesModel.ProductAggregate
{
    public class ProductPriceChangeHistory : Entity
    {
        public ProductPriceChangeHistory(int userId, decimal price)
        {
            UserId = userId;
            Price = price;
        }

        public int ProductId { get; private set; }
        public int UserId { get; private set; }
        public decimal Price { get; private set; }

        public virtual Product Product { get; private set; }
        public virtual User User { get; private set; }
    }
}
