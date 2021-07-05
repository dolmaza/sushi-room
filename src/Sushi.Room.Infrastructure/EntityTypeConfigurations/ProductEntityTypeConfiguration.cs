using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sushi.Room.Domain.AggregatesModel.ProductAggregate;

namespace Sushi.Room.Infrastructure.EntityTypeConfigurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Title)
                .IsRequired();

            builder.HasOne(product => product.User)
                .WithMany(user => user.Products)
                .HasForeignKey(product => product.UserId)
                .IsRequired();

            builder.HasOne(product => product.Category)
                .WithMany(user => user.Products)
                .HasForeignKey(product => product.CategoryId)
                .IsRequired();
        }
    }
}
