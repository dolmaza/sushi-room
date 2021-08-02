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
        private Product(int userId, string title, string titleEng, string description, string descriptionEng, string imageName, decimal price, decimal? discountPercent)
        {
            UserId = userId;
            Title = title;
            TitleEng = titleEng;
            Description = description;
            DescriptionEng = descriptionEng;
            ImageName = imageName;
            Price = price;
            DiscountPercent = discountPercent;
        }
        
        public int UserId { get; private set; }
        public string Title { get; private set; }
        public string TitleEng { get; private set; }
        public string Description { get; private set; }
        public string DescriptionEng { get; private set; }
        public string ImageName { get; private set; }
        public decimal Price { get; private set; }
        public decimal? DiscountPercent { get; private set; }
        public bool IsPublished { get; private set; }

        public virtual User User { get; private set; }
        public virtual ICollection<ProductPriceChangeHistory> ProductPriceChangeHistories { get; private set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; private set; }

        public static Product CreateNew(int userId, string title, string titleEng, string description, string descriptionEng, string imageName, decimal price, decimal? discountPercent, bool isPublished)
        {
            var product = new Product(userId, title, titleEng, description, descriptionEng, imageName, price, discountPercent);

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

        public void UpdateMetaData(int userId, string title, string titleEng, string description, string descriptionEng, decimal price, decimal? discountPercent)
        {
            UserId = userId;
            Title = title;
            TitleEng = titleEng;
            Description = description;
            DescriptionEng = descriptionEng;
            Price = price;
            DiscountPercent = discountPercent;
        }

        public void MarkAsPublished()
        {
            IsPublished = true;
        }

        public void MarkAsUnpublished()
        {
            IsPublished = false;
        }

        public void SetCategories(List<ProductCategory> productCategories)
        {
            ProductCategories ??= new List<ProductCategory>();
            
            ProductCategories.Clear();
            foreach (var productCategory in productCategories)
            {
                ProductCategories.Add(productCategory);
            }
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

        public decimal? CalculateDiscountedPrice()
        {
            return Price - Price * DiscountPercent / 100;
        }
    }
}
