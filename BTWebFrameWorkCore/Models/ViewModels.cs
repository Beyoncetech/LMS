using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BTWebAppFrameWorkCore.Models
{
    public class AppBreadCrumb
    {
        public string ActionUrl { get; set; }
        public string Name { get; set; }
    }
    public class BaseViewModel
    {
        public string ProjectName { get; set; }
        public string PageTitle { get; set; }
        public string BUserName { get; set; }
        public string BUserImgPath { get; set; }
        public List<AppBreadCrumb> BreadCrumbItems { get; set; }
        public List<UserMessageInfo> UserMsgItems { get; set; }
        public UserNotificationInfo UserNotification { get; set; }

        public void CopyToBase(BaseViewModel BaseObj)
        {
            ProjectName = BaseObj.ProjectName;
            PageTitle = BaseObj.PageTitle;
            BUserName = BaseObj.BUserName;
            BUserImgPath = BaseObj.BUserImgPath;
            BreadCrumbItems = BaseObj.BreadCrumbItems;
            UserMsgItems = BaseObj.UserMsgItems;
            UserNotification = BaseObj.UserNotification;
        }
    }

    public class LoginVM : BaseViewModel
    {
        [Required(ErrorMessage = "UserID is required")]
        public string UserName { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Password cannot be less than 3 char")]
        public string Password { get; set; }
        public Boolean RememberMe { get; set; }
    }

    public class UserContactInfo : BaseViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
