using System.Collections.Generic;

namespace Sushi.Room.Web.Models
{
    public class HeaderComponentModel
    {
        public string Culture { get; set; }
        public LanguagePickerDropDownItemModel SelectedLanguagePickerDropDownItem { get; set; }
        public List<LanguagePickerDropDownItemModel> LanguagePickerDropDownItems { get; set; }
    }

    public class LanguagePickerDropDownItemModel
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}