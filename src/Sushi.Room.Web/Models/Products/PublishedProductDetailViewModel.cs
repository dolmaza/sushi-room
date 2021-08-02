using Sushi.Room.Application.Services.DataModels;

namespace Sushi.Room.Web.Models.Products
{
    public class PublishedProductDetailViewModel
    {
        public string Culture { get; set; }
        public int CategoryId { get; set; }
        public string CategoryCaption { get; set; }
        public PublishedProductDto Product { get; set; }
    }
}