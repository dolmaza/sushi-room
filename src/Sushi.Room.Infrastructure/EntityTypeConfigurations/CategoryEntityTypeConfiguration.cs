using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sushi.Room.Domain.AggregatesModel.CategoryAggregate;

namespace Sushi.Room.Infrastructure.EntityTypeConfigurations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Caption)
                .IsRequired();
        }
    }
}
