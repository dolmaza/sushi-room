using Sushi.Room.Domain.AggregatesModel.ProductAggregate;
using Sushi.Room.Domain.SeedWork;
using System.Collections.Generic;

namespace Sushi.Room.Domain.AggregatesModel.CategoryAggregate
{
    public class Category : Entity, IAggregateRoot
    {
        public Category()
        {
        }

        private Category(string caption, string captionEng, string imageName)
        {
            Caption = caption;
            CaptionEng = captionEng;
            ImageName = imageName;
        }

        public string Caption { get; private set; }
        public string CaptionEng { get; private set; }
        public string ImageName { get; private set; }
        public bool IsPublished { get; private set; }
        public int SortIndex { get; private set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; private set; }

        public static Category CreateNew(string caption, string captionEng, string imageName, bool isPublished)
        {
            var category = new Category(caption, captionEng, imageName);

            if (isPublished)
            {
                category.MarkAsPublished();
            }
            else
            {
                category.MarkAsUnpublished();
            }

            return category;
        }

        public void SetSortIndex(int sortIndex)
        {
            SortIndex = sortIndex;
        }

        public void UpdateMetaData(string caption, string captionEng)
        {
            Caption = caption;
            CaptionEng = captionEng;
        }

        public void MarkAsPublished()
        {
            IsPublished = true;
        }

        public void MarkAsUnpublished()
        {
            IsPublished = false;
        }

        public void SetImageName(string imageName)
        {
            ImageName = imageName;
        }
    }
}
