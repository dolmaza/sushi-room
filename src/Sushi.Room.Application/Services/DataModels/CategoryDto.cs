using System.ComponentModel.DataAnnotations;

namespace Sushi.Room.Application.Services.DataModels
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Display(Name = "დასახელება")]
        public string Caption { get; set; }

        [Display(Name = "დასახელება ინგ.")]
        public string CaptionEng { get; set; }

        [Display(Name = "სურათი")]
        public string ImageName { get; set; }

        public string ImageUrl { get; set; }
        public string ImageBase64 { get; set; }

        [Display(Name = "გამოქვეყნებულია?")]
        public bool IsPublished { get; set; }

        [Display(Name = "სორტირების ინდექსი")]
        public int SortIndex { get; set; }
    }
}
