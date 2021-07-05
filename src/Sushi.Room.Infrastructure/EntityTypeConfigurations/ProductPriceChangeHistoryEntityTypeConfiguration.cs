using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sushi.Room.Domain.AggregatesModel.ProductAggregate;

namespace Sushi.Room.Infrastructure.EntityTypeConfigurations
{
    public class ProductPriceChangeHistoryEntityTypeConfiguration : IEntityTypeConfiguration<ProductPriceChangeHistory>
    {
        public void Configure(EntityTypeBuilder<ProductPriceChangeHistory> builder)
        {
            builder.ToTable("ProductPriceChangeHistories");

            builder.HasKey(entity => entity.Id);

            builder.HasOne(product => product.User)
                .WithMany(user => user.ProductPriceChangeHistories)
                .HasForeignKey(product => product.UserId)
                .IsRequired();

            builder.HasOne(product => product.Product)
                .WithMany(user => user.ProductPriceChangeHistories)
                .HasForeignKey(product => product.ProductId)
                .IsRequired();
        }
    }
}
