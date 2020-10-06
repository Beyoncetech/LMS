﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AppBAL.Sevices.Login;
using AppBAL.Sevices.Master;
using AppModel;
using AppModel.ViewModel;
using AppUtility.AppEncription;
using AppUtility.AppModels;
using BTWebAppFrameWorkCore.AppSecurity;
using BTWebAppFrameWorkCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BTWebAppFrameWorkCore.Controllers
{
    [Authorize(Policy = "AllowAuthUsers")]
    public class AccountController : BaseController
    {
        private readonly IEmailSender _EmailSender;
        private readonly ILoginService _LoginService;
        private readonly IAppUserService _AppUserService;
        private readonly IAppCookiesAuthService _AppCookiesAuth;
        private IWebHostEnvironment _HostingEnvironment;

        public AccountController(ILoginService LoginService, IEmailSender EmailSender, AppSettingsConfiguration AppSettingsConfig,
            IAppCookiesAuthService AppCookiesAuth, IAppUserService AppUserService, IWebHostEnvironment HostingEnvironment)
        {
            _LoginService = LoginService;
            _EmailSender = EmailSender;
            _AppSettingsConfig = AppSettingsConfig;
            _AppUserService = AppUserService;
            _AppCookiesAuth = AppCookiesAuth;
            _HostingEnvironment = HostingEnvironment;
        }
        #region App login
        [AllowAnonymous]
        public IActionResult Login()
        {
            var VModel = GetViewModel<LoginVM>();
            return View(VModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<JsonResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _LoginService.ValidateUser(model.UserName, model.Password);
                if (result.Stat)
                {
                    LoginUser UserInfo = (LoginUser)result.StatusObj;

                    var TempClaims = new List<Claim>
                    {
                        new Claim ("UserID", UserInfo.UserId),
                        new Claim ("UserName", UserInfo.Name),
                        new Claim ("UserType", UserInfo.UserType),
                        new Claim ("UserGender", string.IsNullOrEmpty(UserInfo.Gender) ? "M" : UserInfo.Gender),
                        new Claim ("UserPerm", string.IsNullOrEmpty(UserInfo.UserPerm) ? "" : UserInfo.UserPerm)
                    };


                    var claimsIdentity = new ClaimsIdentity(TempClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                        });


                    //return RedirectToAction("Index", "Home");
                    return Json(new { stat = true, msg = "Valid User", rtnUrl = "/Account/Dashboard" });
                }
                else
                    return Json(new { stat = false, msg = "Invalid UserId and Password" });
            }
            else
            {
                return Json(new { stat = false, msg = "Invalid UserId and Password" });
            }

        }
        [HttpPost]
        public async Task<JsonResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Json(new { stat = true, msg = "Successfully Signed out" });
        }
        #endregion

        #region Dashboard

        public IActionResult Dashboard()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Dashboard", ActionUrl = "/Account/Dashboard" } });
            var VModel = GetViewModel<DashboardVM>();
            return View(VModel);
        }

        #endregion

        #region User Profile        
        public async Task<IActionResult> UserProfile()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "User Profile", ActionUrl = "/Account/UserProfile" } });

            BaseViewModel VModel = null;
            var CurrentUserInfo = GetLoginUserInfo();
            //*****get user avtar************
            string UsrImgPath = string.Format("{0}\\{1}.{2}", Path.Combine(_HostingEnvironment.WebRootPath, "AppFileRepo\\UserAvatar\\"), CurrentUserInfo.UserID, "jpg");
            if (System.IO.File.Exists(UsrImgPath))
            {
                UsrImgPath = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}", CurrentUserInfo.UserID, "jpg");
            }
            else
            {
                if (CurrentUserInfo.UserGender.Equals("M"))
                    UsrImgPath = "~/img/avatar5.png";
                else
                    UsrImgPath = "~/img/avatar3.png";
            }

            //*******************************

            var result = await _AppUserService.GetUserProfile(CurrentUserInfo.UserID);
            if (result.Stat)
            {
                UserProfile UserInfo = (UserProfile)result.StatusObj;

                var TempVModel = new UserProfileVM
                {
                    Id = UserInfo.Id,
                    UserID = UserInfo.UserId,
                    UserName = UserInfo.Name,
                    Email = UserInfo.Email,
                    Mobile = UserInfo.Mobile,
                    Dob = UserInfo.Dob,
                    UserImgPath = UsrImgPath,
                    AttachUserImage = new FileUploadInfo()
                };

                VModel = GetViewModel(TempVModel);
            }
            else
            {
                var TempVModel = new UserProfileVM
                {
                    Id = 0,
                    UserID = CurrentUserInfo.UserID,                    
                    UserImgPath = UsrImgPath,
                    AttachUserImage = new FileUploadInfo()
                };

                VModel = GetViewModel(TempVModel);
            }


            return View(VModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UserProfile(UserProfileVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _AppUserService.UpdateAppUserProfileAsync(model).ConfigureAwait(false);                
                if (result.Stat)
                {
                    //save the user picture
                    //if (model.AttachUserImage.FileSize > 0)
                    //{
                    //    try
                    //    {
                    //        string UsrImgPath = AppPathResolver.GetTenantUserImageDirectoryPath(Server.MapPath("~\\images\\"), oTenantUser.TenantID);
                    //        if (!Directory.Exists(UsrImgPath))
                    //            Directory.CreateDirectory(UsrImgPath);
                    //        UsrImgPath = string.Format("{0}\\{1}.{2}", UsrImgPath, oTenantUser.UserID, "jpg");
                    //        string FileContentBase64 = Regex.Replace(model.AttachUserImage.FileContentsBase64, "^data:image/[a-zA-Z]+;base64,", string.Empty);
                    //        Byte[] bytes = Convert.FromBase64String(FileContentBase64);
                    //        System.IO.File.WriteAllBytes(UsrImgPath, bytes);
                    //        model.UserImgPath = string.Format("~/images/AppUser/{0}/{1}.{2}?r={3}", AppPathResolver.GetTenantFolderName(oTenantUser.TenantID), oTenantUser.UserID, "jpg", DateTime.Now.Ticks.ToString());
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        result.Stat = false;
                    //        result.Msg = ex.Message;
                    //    }
                    //}
                    return Json(new { stat = true, msg = "Successfully updated user profile" });
                }
                else
                    return Json(new { stat = false, msg = result.StatusMsg });
            }
            else
            {
                return Json(new { stat = false, msg = "Invalid User Profile data" });
            }
        }
        #endregion
    }
}