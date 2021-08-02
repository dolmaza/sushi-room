using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sushi.Room.Domain.AggregatesModel.ProductAggregate;

namespace Sushi.Room.Infrastructure.EntityTypeConfigurations
{
    public class ProductCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategories");

            builder.HasKey(entity => entity.Id);

            builder.HasIndex(entity => new {entity.CategoryId, entity.ProductId}).IsUnique();

            builder.HasOne(productCategory => productCategory.Product)
                .WithMany(product => product.ProductCategories)
                .HasForeignKey(productCategory => productCategory.ProductId)
                .IsRequired();
            
            builder.HasOne(productCategory => productCategory.Category)
                .WithMany(product => product.ProductCategories)
                .HasForeignKey(productCategory => productCategory.CategoryId)
                .IsRequired();
        }
    }
}