namespace Sushi.Room.Application.Services.DataModels
{
    public class PublishedProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public bool HasDiscount => DiscountPercent.HasValue;

    }
}
