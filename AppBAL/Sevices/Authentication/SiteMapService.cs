using AppModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppBAL.Sevices.Authentication
{
    public interface ISiteMapService
    {
        List<SiteMapInfo> GetUserMenu(string UserPerm);
        string GetPageID(string ActionUrl);
        Boolean IsValidAction(string CurActionUrl, string CurUserPerm);
    }

    public class SiteMapService : ISiteMapService
    {
        private readonly List<SiteMapInfo> _AppSiteMap;
        public SiteMapService()
        {
            _AppSiteMap = new List<SiteMapInfo>();
            LoadSiteMap();
        }
        private void LoadSiteMap()
        {            
            _AppSiteMap.Add(new SiteMapInfo {  ID = "1", MenuOrder = 1, Group = "", Parent = "", Name = "Dashboard", Type = "LeftMenu", AccessType = "GA",  ActionUrl = "/Account/Dashboard", ActionPerm = "000", IconLeft = "nav-icon fas fa-tachometer-alt", IconRight = "" });

            _AppSiteMap.Add(new SiteMapInfo { ID = "2.1", MenuOrder = 2, Group = "", Parent = "Account", Name = "User", Type = "LeftMenu", AccessType = "R", ActionUrl = "/home/index", ActionPerm = "000", IconLeft = "nav-icon fas fa-th", IconRight = "" });

            _AppSiteMap.Add(new SiteMapInfo { ID = "3.1", MenuOrder = 3, Group = "", Parent = "Master", Name = "Create user", Type = "LeftMenu", AccessType = "R", ActionUrl = "/home/index", ActionPerm = "000", IconLeft = "nav-icon fas fa-th", IconRight = "" });
            _AppSiteMap.Add(new SiteMapInfo { ID = "3.2", MenuOrder = 4, Group = "", Parent = "Master", Name = "Delete user", Type = "LeftMenu", AccessType = "R", ActionUrl = "/home/index", ActionPerm = "000", IconLeft = "nav-icon fas fa-th", IconRight = "" });

            _AppSiteMap.Add(new SiteMapInfo { ID = "4.1", MenuOrder = 5, Group = "Report", Parent = "Master", Name = "User List Report", Type = "LeftMenu", AccessType = "R", ActionUrl = "/home/index", ActionPerm = "000", IconLeft = "nav-icon fas fa-th", IconRight = "" });
            _AppSiteMap.Add(new SiteMapInfo { ID = "4.2", MenuOrder = 6, Group = "Report", Parent = "Class", Name = "Class List Report", Type = "LeftMenu", AccessType = "R", ActionUrl = "/home/index", ActionPerm = "000", IconLeft = "nav-icon fas fa-th", IconRight = "" });
            _AppSiteMap.Add(new SiteMapInfo { ID = "4.2", MenuOrder = 6, Group = "Report", Parent = "Class", Name = "Student List Report", Type = "LeftMenu", AccessType = "R", ActionUrl = "/home/index", ActionPerm = "000", IconLeft = "nav-icon fas fa-th", IconRight = "" });
            
            _AppSiteMap.Add(new SiteMapInfo { ID = "50.1", MenuOrder = 50, Group = "App Logs", Parent = "", Name = "Activity Log", Type = "LeftMenu", AccessType = "R", ActionUrl = "/AppLog/ActivityLog", ActionPerm = "000", IconLeft = "nav-icon fas fa-th", IconRight = "" });

            _AppSiteMap.Add(new SiteMapInfo { ID = "100", MenuOrder = 100, Group = "", Parent = "", Name = "", Type = "ChildAction", AccessType = "GA", ActionUrl = "/Account/Logout", ActionPerm = "000", IconLeft = "nav-icon fas fa-tachometer-alt", IconRight = "" });
            _AppSiteMap.Add(new SiteMapInfo { ID = "101", MenuOrder = 101, Group = "", Parent = "", Name = "UserProfile", Type = "TopRightMenu", AccessType = "GA", ActionUrl = "/Account/UserProfile", ActionPerm = "000", IconLeft = "nav-icon fas fa-tachometer-alt", IconRight = "" });
            _AppSiteMap.Add(new SiteMapInfo { ID = "102", MenuOrder = 102, Group = "", Parent = "", Name = "", Type = "ChildAction", AccessType = "GA", ActionUrl = "/Account/MarkActivityAsRead", ActionPerm = "000", IconLeft = "nav-icon fas fa-tachometer-alt", IconRight = "" });
            _AppSiteMap.Add(new SiteMapInfo { ID = "103", MenuOrder = 103, Group = "", Parent = "", Name = "ChangePassword", Type = "TopRightMenu", AccessType = "GA", ActionUrl = "/Account/ChangeProfilePassword", ActionPerm = "000", IconLeft = "nav-icon fas fa-tachometer-alt", IconRight = "" });
            return;
        }

        public List<SiteMapInfo> GetUserMenu(string UserPerm)
        {
            return null;
        }

        public string GetPageID(string actionUrl)
        {
            var PageInfo = _AppSiteMap.FirstOrDefault(x => x.ActionUrl.Equals(actionUrl));
            if (PageInfo != null)
                return PageInfo.ID;
            else
                return "X.X";
        }

        public Boolean IsValidAction(string CurActionUrl, string CurUserPerm)
        {
            var CurPageInfo = _AppSiteMap.FirstOrDefault(x => x.ActionUrl.ToLower().Equals(CurActionUrl.ToLower()));

            if (CurPageInfo != null)
            {
                if (CurPageInfo.AccessType.Equals("GA")) // for globar access page
                    return true;
                else
                {
                    var UserPageIds = CurUserPerm.Split(',');
                    if (UserPageIds.Contains(CurPageInfo.ID))
                        return true;
                    else
                        return false;
                }
            }
            else
                return false;
        }
    }
}
