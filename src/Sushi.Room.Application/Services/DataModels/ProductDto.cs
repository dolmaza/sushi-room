using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sushi.Room.Application.Services.DataModels
{
    public class ProductDto
    {
        public int Id { get; set; }
        
        [Display(Name = "კატეგორია")]
        public int CategoryId { get; set; }
        public string CategoryCaption { get; set; }
        
        public int UserId { get; set; }
        
        [Display(Name = "სათაური")]
        public string Title { get; set; }
        
        [Display(Name = "სათაური ინგ.")]
        public string TitleEng { get; set; }
        
        [Display(Name = "აღწერა")]
        public string Description { get; set; }
        
        [Display(Name = "აღწერა ინგ.")]
        public string DescriptionEng { get; set; }
        
        [Display(Name = "ფასი")]
        public decimal? Price { get; set; }
        
        [Display(Name = "გამოქვეყნებულია?")]
        public bool IsPublished { get; set; }
        
        [Display(Name = "სურათი")]
        public string ImageName { get; set; }

        public string ImageUrl { get; set; }
        public string ImageBase64 { get; set; }

        public List<KeyValuePair<int, string>> Categories { get; set; }
    }
}