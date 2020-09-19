using System;
using System.Collections.Generic;
using System.Text;

namespace AppModel
{
    public class CommonResponce
    {
        public Boolean Stat { get; set; }        
        public string StatusMsg { get; set; }
        public dynamic StatusObj { get; set; }
    }

    public class SiteMapInfo
    {
        public string ID { get; set; }        
        public string ParentID { get; set; }
        public string AccessType { get; set; }
        public string  MenuItemInfo { get; set; }
        public string ActionUrl { get; set; }
        public string ActionPerm { get; set; }
        public string Name { get; set; }
        public string IconLeft { get; set; }
        public string IconRight { get; set; }
    }
}
