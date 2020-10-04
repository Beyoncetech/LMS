using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using AppModel;
using BTWebAppFrameWorkCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BTWebAppFrameWorkCore.Controllers
{
    public abstract class BaseController : Controller
    {
        public AppSettingsConfiguration _AppSettingsConfig;
        public BaseViewModel _BaseViewModel;

        public BaseController()
        {
            _BaseViewModel = new BaseViewModel
            {
                ProjectName = "BT Project",
                PageTitle = "Home",
                BUserID = "",
                BUserName = "Unknown",
                BUserType = "A",
                BUserGender = "M",
                BUserImgPath = "~/assets/img/AppUser/BlankUser.jpg",
                BreadCrumbItems = new List<AppBreadCrumb>()
            };
            if (AppConfigSingletonRepository.AppConfig != null)
                _BaseViewModel.ProjectName = AppConfigSingletonRepository.AppConfig.ProjectName;
                       

            GetUserMessage(); // get the unread message and notification from db
        }

        public void CreateBreadCrumb(dynamic BreadCrumbItems)
        {
            _BaseViewModel.BreadCrumbItems = new List<AppBreadCrumb>();
            foreach (var item in BreadCrumbItems)
            {
                _BaseViewModel.BreadCrumbItems.Add(new AppBreadCrumb { Name = item.Name, ActionUrl = item.ActionUrl });
            }
        }

        public BaseViewModel GetViewModel<T>()
        {
            BaseViewModel TempBaseViewModel = (BaseViewModel)Activator.CreateInstance(typeof(T));
            var tempLoginUser = GetLoginUserInfo();
            if (tempLoginUser != null)
            {
                _BaseViewModel.BUserID = tempLoginUser.UserID;
                _BaseViewModel.BUserName = tempLoginUser.UserName;
                _BaseViewModel.BUserType = tempLoginUser.UserType;
                _BaseViewModel.BUserGender = tempLoginUser.UserGender;
            }
            TempBaseViewModel.CopyToBase(_BaseViewModel);
            return TempBaseViewModel;
        }

        public LoginUserInfo GetLoginUserInfo()
        {
            LoginUserInfo result = null;            
            if (Request.HttpContext.User.Identity.IsAuthenticated)
            {
                var claimsIdentity = Request.HttpContext.User.Identity as ClaimsIdentity;

                result = new LoginUserInfo();
                foreach (var claim in claimsIdentity.Claims)
                {
                    switch (claim.Type)
                    {
                        case "UserID":
                            result.UserID = claim.Value;
                            break;
                        case "UserName":
                            result.UserName = claim.Value;
                            break;
                        case "UserType":
                            result.UserType = claim.Value;
                            break;
                        case "UserPerm":
                            result.UserPerm = claim.Value;
                            break;
                        case "UserGender":
                            result.UserGender = claim.Value;
                            break;
                        default:
                            break;
                    }
                    //System.Console.WriteLine(claim.Type + ":" + claim.Value);
                }

                return result;
            }
            else
                return result;            
        }

        private void GetUserMessage()
        {
            // get top 3 unread meggage from db
            _BaseViewModel.UserMsgItems = new List<UserMessageInfo>();
            _BaseViewModel.UserMsgItems.Add(new UserMessageInfo
            {
                ID = "A001",
                Name = "Brad Diesel",
                Message = "Call me whenever you can...",
                TimeRef = "4 Hours Ago",
                UserAvatar = "/img/user1-128x128.jpg"
            });
            _BaseViewModel.UserMsgItems.Add(new UserMessageInfo
            {
                ID = "A002",
                Name = "John Pierce",
                Message = "I got your message bro",
                TimeRef = "6 Hours Ago",
                UserAvatar = "/img/user3-128x128.jpg"
            });
            _BaseViewModel.UserMsgItems.Add(new UserMessageInfo
            {
                ID = "A003",
                Name = "Nora Silvester",
                Message = "The subject goes here",
                TimeRef = "9 Hours Ago",
                UserAvatar = "/img/user8-128x128.jpg"
            });

            // get top unread notification from db
            _BaseViewModel.UserNotification = new UserNotificationInfo();
            _BaseViewModel.UserNotification.TotalNotification = "15";
            _BaseViewModel.UserNotification.MsgItems = new List<UserNotificationItem>();
            _BaseViewModel.UserNotification.MsgItems.Add(new UserNotificationItem
            {
                NotifyType = "M",
                Message = "4 new messages",
                TimeRef = "3 mins"
            });
            _BaseViewModel.UserNotification.MsgItems.Add(new UserNotificationItem
            {
                NotifyType = "R",
                Message = "8 friend requests",
                TimeRef = "12 hours"
            });
            _BaseViewModel.UserNotification.MsgItems.Add(new UserNotificationItem
            {
                NotifyType = "RPT",
                Message = "3 new reports",
                TimeRef = "2 days"
            });
        }

    }
}