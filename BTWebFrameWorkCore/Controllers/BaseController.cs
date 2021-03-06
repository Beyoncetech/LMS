﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using AppModel;
using AppModel.ViewModel;
using BTWebAppFrameWorkCore.Models;
using BTWebAppFrameWorkCore.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BTWebAppFrameWorkCore.Controllers
{
    public abstract class BaseController : Controller
    {        
        private IBaseControllerService _BaseService;
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
            
        }

        public void CreateBreadCrumb(dynamic BreadCrumbItems)
        {
            _BaseViewModel.BreadCrumbItems = new List<AppBreadCrumb>();
            foreach (var item in BreadCrumbItems)
            {
                _BaseViewModel.BreadCrumbItems.Add(new AppBreadCrumb { Name = item.Name, ActionUrl = item.ActionUrl });
            }
        }

        public async Task<BaseViewModel> GetViewModel<T>()
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
            UpdateBaseModelConfig();
            await GetUserMessage(); // get the unread message and notification from db
            TempBaseViewModel.CopyToBase(_BaseViewModel);
            return TempBaseViewModel;
        }
               
        public async Task<BaseViewModel> GetViewModel(BaseViewModel objBaseViewModel)
        {            
            var tempLoginUser = GetLoginUserInfo();
            if (tempLoginUser != null)
            {
                _BaseViewModel.BUserID = tempLoginUser.UserID;
                _BaseViewModel.BUserName = tempLoginUser.UserName;
                _BaseViewModel.BUserType = tempLoginUser.UserType;
                _BaseViewModel.BUserGender = tempLoginUser.UserGender;
            }
            UpdateBaseModelConfig();
            await GetUserMessage(); // get the unread message and notification from db
            objBaseViewModel.CopyToBase(_BaseViewModel);
            return objBaseViewModel;
        }

        public void UpdateBaseModelConfig()
        {            
            _BaseViewModel.ProjectName = GetBaseService().AppSettingConfigaration.ProjectName;
            _BaseViewModel.BUserImgPath = GetBaseService().GetUserAvatarPath(string.Format("{0}.{1}", _BaseViewModel.BUserID, "jpg"));
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
                        case "ID":
                            result.ID = claim.Value;
                            break;
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

        public IBaseControllerService GetBaseService()
        {
            if(_BaseService == null)
                _BaseService = (IBaseControllerService) HttpContext.RequestServices.GetService(typeof(IBaseControllerService));

            return _BaseService;
        }

        public string GetAppRootUrl()
        {
            return HttpContext.Request.Host.Value;
        }

        private async Task GetUserMessage()
        {
            if (Request.HttpContext.User.Identity.IsAuthenticated)
            {
                // get top 5 unread activity from db
                _BaseViewModel.UserActivityMsg = await GetBaseService().GetUnreadUserActivity(5);
                
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
}