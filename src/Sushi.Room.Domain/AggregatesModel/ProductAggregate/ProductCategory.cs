using Sushi.Room.Domain.AggregatesModel.CategoryAggregate;
using Sushi.Room.Domain.SeedWork;

namespace Sushi.Room.Domain.AggregatesModel.ProductAggregate
{
    public class ProductCategory : Entity, IAggregateRoot
    {
        public ProductCategory(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int ProductId { get; private set; }
        public int CategoryId { get; private set; }
        public int SortIndex { get; private set; }

        public virtual Category Category { get; private set; }
        public virtual Product Product { get; private set; }

        public void UpdateMetaData(int sortIndex)
        {
            SortIndex = sortIndex;
        }
    }
}