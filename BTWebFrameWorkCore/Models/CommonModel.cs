using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTWebAppFrameWorkCore.Models
{
    public class LoginUserInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }              
        public string UserType { get; set; }
        public string UserPerm { get; set; }
        public string UserGender { get; set; }
    }

    public class UserMessageInfo
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string TimeRef { get; set; }
        public string UserAvatar { get; set; }
    }

    public class UserNotificationItem
    {        
        public string NotifyType { get; set; }
        public string Message { get; set; }
        public string TimeRef { get; set; }        
    }

    public class UserNotificationInfo
    {
        public string TotalNotification { get; set; }
        public List<UserNotificationItem> MsgItems { get; set; }        
    }
        
}
