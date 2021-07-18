using Sushi.Room.Domain.AggregatesModel.CategoryAggregate;
using Sushi.Room.Domain.AggregatesModel.UserAggregate;
using Sushi.Room.Domain.SeedWork;
using System.Collections.Generic;

namespace Sushi.Room.Domain.AggregatesModel.ProductAggregate
{
    public class Product : Entity, IAggregateRoot
    {
        public Product()
        {

        }
        private Product(int categoryId, int userId, string title, string titleEng, string description, string descriptionEng, string imageName, decimal price)
        {
            CategoryId = categoryId;
            UserId = userId;
            Title = title;
            TitleEng = titleEng;
            Description = description;
            DescriptionEng = descriptionEng;
            ImageName = imageName;
            Price = price;
        }

        public int CategoryId { get; private set; }
        public int UserId { get; private set; }
        public string Title { get; private set; }
        public string TitleEng { get; private set; }
        public string Description { get; private set; }
        public string DescriptionEng { get; private set; }
        public string ImageName { get; private set; }
        public decimal Price { get; private set; }
        public bool IsPublished { get; private set; }

        public virtual Category Category { get; private set; }
        public virtual User User { get; private set; }
        public virtual ICollection<ProductPriceChangeHistory> ProductPriceChangeHistories { get; private set; }

        public static Product CreateNew(int categoryId, int userId, string title, string titleEng, string description, string descriptionEng, string imageName, decimal price, bool isPublished)
        {
            var product = new Product(categoryId, userId, title, titleEng, description, descriptionEng, imageName, price);

            product.SaveProductPriceChangeHistory(userId, price);

            if (isPublished)
            {
                product.MarkAsPublished();
            }
            else
            {
                product.MarkAsUnpublished();
            }

            return product;
        }

        public void UpdateMetaData(int categoryId, int userId, string title, string titleEng, string description, string descriptionEng, decimal price)
        {
            CategoryId = categoryId;
            UserId = userId;
            Title = title;
            TitleEng = titleEng;
            Description = description;
            DescriptionEng = descriptionEng;
            Price = price;
        }

        public void MarkAsPublished()
        {
            IsPublished = true;
        }

        public void MarkAsUnpublished()
        {
            IsPublished = false;
        }

        public void SaveProductPriceChangeHistory(int userId, decimal price)
        {
            var productPriceChangeHistory = new ProductPriceChangeHistory(userId, price);

            if (ProductPriceChangeHistories == null)
            {
                ProductPriceChangeHistories = new List<ProductPriceChangeHistory> { productPriceChangeHistory };
            }
            else
            {
                ProductPriceChangeHistories.Add(productPriceChangeHistory);
            }
        }

        public void SetImageName(string imageName)
        {
            ImageName = imageName;
        }
    }
}
