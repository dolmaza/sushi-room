using System.Collections.Generic;
using System.Linq;

namespace Sushi.Room.Web.Areas.Admin.Models
{
    public class SidebarMenuModel
    {
        public string Caption { get; set; }
        public string Url { get; set; }
        public string IconName { get; set; }
        public bool IsActive { get; set; }
        public bool IsMenuItemExpanded => SecondLevelMenuItems.Any(s => s.IsActive);
        public bool HasSecondLevelMenuItems => SecondLevelMenuItems.Count > 0;

        public List<SidebarMenuModel> SecondLevelMenuItems { get; set; }

        public SidebarMenuModel()
        {
            SecondLevelMenuItems = new List<SidebarMenuModel>();
        }
    }
}
