using Sushi.Room.Application.Services.DataModels;
using System.Collections.Generic;

namespace Sushi.Room.Web.Models.Products
{
    public class PublishedProductViewModel
    {
        public string Culture { get; set; }
        public int CategoryId { get; set; }
        public string CategoryCaption { get; set; }
        public List<PublishedProductDto> Products { get; set; }
    }
}
