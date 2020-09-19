using AppModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppBAL.Sevices.Authentication
{
    public interface ISiteMapService
    {
        List<SiteMapInfo> GetUserMenu(string UserPerm);
    }

    public class SiteMapService : ISiteMapService
    {
        private List<SiteMapInfo> _AppSiteMap;
        public SiteMapService()
        {
            LoadSiteMap();
        }
        private void LoadSiteMap()
        {
            _AppSiteMap = new List<SiteMapInfo>();
            _AppSiteMap.Add(new SiteMapInfo {  ID = "1", ParentID = "0", Name = "Dashboard", AccessType = "GA", MenuItemInfo = "nav-item",  ActionUrl = "home/index", IconLeft = "nav-icon fas fa-tachometer-alt", IconRight = "" });
            
            _AppSiteMap.Add(new SiteMapInfo { ID = "2", ParentID = "0", Name = "Account", AccessType = "R", MenuItemInfo = "nav-header", ActionUrl = "", IconLeft = "", IconRight = "" });
            _AppSiteMap.Add(new SiteMapInfo { ID = "2.1", ParentID = "2", Name = "User", AccessType = "R", MenuItemInfo = "nav-item", ActionUrl = "home/index", IconLeft = "nav-icon fas fa-th", IconRight = "" });

            _AppSiteMap.Add(new SiteMapInfo { ID = "3", ParentID = "0", Name = "Master", AccessType = "R", MenuItemInfo = "nav-header", ActionUrl = "", IconLeft = "", IconRight = "" });
            _AppSiteMap.Add(new SiteMapInfo { ID = "3.1", ParentID = "3", Name = "Create user", AccessType = "R", MenuItemInfo = "nav-item", ActionUrl = "home/index", IconLeft = "nav-icon fas fa-th", IconRight = "" });
            _AppSiteMap.Add(new SiteMapInfo { ID = "3.2", ParentID = "3", Name = "Delete user", AccessType = "R", MenuItemInfo = "nav-item", ActionUrl = "home/index", IconLeft = "nav-icon fas fa-th", IconRight = "" });

            _AppSiteMap.Add(new SiteMapInfo { ID = "4", ParentID = "0", Name = "Report", AccessType = "R", MenuItemInfo = "nav-header", ActionUrl = "", IconLeft = "", IconRight = "" });
            _AppSiteMap.Add(new SiteMapInfo { ID = "4.1", ParentID = "4", Name = "Report 1", AccessType = "R", MenuItemInfo = "nav-item", ActionUrl = "home/index", IconLeft = "nav-icon fas fa-th", IconRight = "" });
            _AppSiteMap.Add(new SiteMapInfo { ID = "4.2", ParentID = "4", Name = "Report 2", AccessType = "R", MenuItemInfo = "nav-item", ActionUrl = "home/index", IconLeft = "nav-icon fas fa-th", IconRight = "" });
            return;
        }

        public List<SiteMapInfo> GetUserMenu(string UserPerm)
        {
            return null;
        }
    }
}
