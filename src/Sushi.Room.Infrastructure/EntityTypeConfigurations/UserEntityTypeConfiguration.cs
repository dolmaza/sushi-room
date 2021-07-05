using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sushi.Room.Domain.AggregatesModel.UserAggregate;

namespace Sushi.Room.Infrastructure.EntityTypeConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(entity => entity.Id);

            builder.HasIndex(entity => entity.UserName)
                .IsUnique();

            builder.Property(entity => entity.UserName)
                .IsRequired();

            builder.Property(entity => entity.Password)
                .IsRequired();

            builder.Property(entity => entity.FirstName)
                .IsRequired();

            builder.Property(entity => entity.LastName)
                .IsRequired();
        }
    }
}
