// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Sushi.Room.Infrastructure;

namespace Sushi.Room.Infrastructure.Migrations
{
    [DbContext(typeof(SushiRoomDbContext))]
    partial class SushiRoomDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Sushi.Room.Domain.AggregatesModel.CategoryAggregate.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CaptionEng")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("DateOfCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImageName")
                        .HasColumnType("text");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("boolean");

                    b.Property<int>("SortIndex")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Sushi.Room.Domain.AggregatesModel.ProductAggregate.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("DateOfCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("DescriptionEng")
                        .HasColumnType("text");

                    b.Property<decimal?>("DiscountPercent")
                        .HasColumnType("numeric");

                    b.Property<string>("ImageName")
                        .HasColumnType("text");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TitleEng")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Sushi.Room.Domain.AggregatesModel.ProductAggregate.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("DateOfCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("SortIndex")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("CategoryId", "ProductId")
                        .IsUnique();

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("Sushi.Room.Domain.AggregatesModel.ProductAggregate.ProductPriceChangeHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("DateOfCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductPriceChangeHistories");
                });

            modelBuilder.Entity("Sushi.Room.Domain.AggregatesModel.UserAggregate.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("DateOfCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Sushi.Room.Domain.AggregatesModel.ProductAggregate.Product", b =>
                {
                    b.HasOne("Sushi.Room.Domain.AggregatesModel.UserAggregate.User", "User")
                        .WithMany("Products")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sushi.Room.Domain.AggregatesModel.ProductAggregate.ProductCategory", b =>
                {
                    b.HasOne("Sushi.Room.Domain.AggregatesModel.CategoryAggregate.Category", "Category")
                        .WithMany("ProductCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sushi.Room.Domain.AggregatesModel.ProductAggregate.Product", "Product")
                        .WithMany("ProductCategories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Sushi.Room.Domain.AggregatesModel.ProductAggregate.ProductPriceChangeHistory", b =>
                {
                    b.HasOne("Sushi.Room.Domain.AggregatesModel.ProductAggregate.Product", "Product")
                        .WithMany("ProductPriceChangeHistories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sushi.Room.Domain.AggregatesModel.UserAggregate.User", "User")
                        .WithMany("ProductPriceChangeHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sushi.Room.Domain.AggregatesModel.CategoryAggregate.Category", b =>
                {
                    b.Navigation("ProductCategories");
                });

            modelBuilder.Entity("Sushi.Room.Domain.AggregatesModel.ProductAggregate.Product", b =>
                {
                    b.Navigation("ProductCategories");

                    b.Navigation("ProductPriceChangeHistories");
                });

            modelBuilder.Entity("Sushi.Room.Domain.AggregatesModel.UserAggregate.User", b =>
                {
                    b.Navigation("ProductPriceChangeHistories");

                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
