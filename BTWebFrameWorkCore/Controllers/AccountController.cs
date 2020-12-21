using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AppBAL.Sevices.AppCore;
using AppBAL.Sevices.Login;
using AppBAL.Sevices.Master;
using AppModel;
using AppModel.ViewModel;
using AppUtility.AppEncription;
using AppUtility.AppIO;
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
        private readonly IAppJobService _AppJobService;
        private readonly ILoginService _LoginService;
        private readonly IAppUserService _AppUserService;
        private readonly IAppCookiesAuthService _AppCookiesAuth;
        private readonly IAppSettingService _AppSettingService;
        private readonly IAppJobService _JobService;

        public AccountController(ILoginService LoginService, IEmailSender EmailSender, AppSettingsConfiguration AppSettingsConfig,
            IAppCookiesAuthService AppCookiesAuth, IAppUserService AppUserService, IAppSettingService ObjAppSettingService,
            IAppJobService objAppJobService, IAppJobService JobService)
        {
            _LoginService = LoginService;
            _EmailSender = EmailSender;
            _AppSettingsConfig = AppSettingsConfig;
            _AppUserService = AppUserService;
            _AppCookiesAuth = AppCookiesAuth;
            _AppSettingService = ObjAppSettingService;
            _AppJobService = objAppJobService;
            _JobService = JobService;
        }
        #region App login
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            var VModel = await GetViewModel<LoginVM>();
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
                        new Claim ("ID", UserInfo.Id.ToString()),
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

        #region App Message update
        [HttpPost]
        public async Task<IActionResult> MarkActivityAsRead()
        {
            UserActivityInfo TempActivity;
            var Result = await GetBaseService().MarkAllUnreadActivityAsRead().ConfigureAwait(false);
            if (Result)
            {
                TempActivity = new UserActivityInfo();
                TempActivity.TotalActivity = "0";
                TempActivity.MsgItems = new List<UserMessageInfo>();
            }
            else
            {
                TempActivity = await GetBaseService().GetUnreadUserActivity(5);
            }

            return PartialView("_UserMessage", TempActivity);

        }
        #endregion

        #region Dashboard

        public async Task<IActionResult> Dashboard()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Dashboard", ActionUrl = "/Account/Dashboard" } });
            var VModel = await GetViewModel<DashboardVM>();
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
            string UsrImgPath = string.Format("{0}\\{1}.{2}", Path.Combine(GetBaseService().GetAppRootPath(), "AppFileRepo\\UserAvatar"), CurrentUserInfo.UserID, "jpg");
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

                VModel = await GetViewModel(TempVModel);
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

                VModel = await GetViewModel(TempVModel);
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
                    if (model.AttachUserImage.FileSize > 0)
                    {
                        string UsrImgPath = Path.Combine(GetBaseService().GetAppRootPath(), "AppFileRepo\\UserAvatar");
                        if (GetBaseService().DirectoryFileService.CreateDirectoryIfNotExist(UsrImgPath))
                        {
                            UsrImgPath = string.Format("{0}\\{1}.{2}", UsrImgPath, model.UserID, "jpg");
                            if (GetBaseService().DirectoryFileService.CreateFileFromBase64String(model.AttachUserImage.FileContentsBase64, UsrImgPath))
                            {
                                model.UserImgPath = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", model.UserID, "jpg", DateTime.Now.Ticks.ToString());
                                model.BUserImgPath = model.UserImgPath; // update the user avatar
                            }
                        }

                    }
                    await GetBaseService().AddActivity(ActivityType.Update, model.UserID, model.UserName, "User Profile", "Updated user profile");
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

        #region Change Password        
        public async Task<IActionResult> ChangeProfilePassword()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Change Password", ActionUrl = "/Account/ChangeProfilePassword" } });

            BaseViewModel VModel = null;
            var CurrentUserInfo = GetLoginUserInfo();

            var result = await _AppUserService.GetUserProfile(CurrentUserInfo.UserID);
            if (result.Stat)
            {
                UserProfile UserInfo = (UserProfile)result.StatusObj;

                var TempVModel = new ChangeProfilePasswordVM
                {
                    Id = UserInfo.Id,
                    UserID = UserInfo.UserId,
                    OldPassword = "",
                    NewPassword = "",
                    ConfirmNewPassword = ""
                };
                VModel = await GetViewModel(TempVModel);
            }
            else
            {
                var TempVModel = new ChangeProfilePasswordVM
                {
                    Id = 0,
                    UserID = CurrentUserInfo.UserID,
                    OldPassword = "",
                    NewPassword = "",
                    ConfirmNewPassword = ""
                };
                VModel = await GetViewModel(TempVModel);
            }

            return View(VModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ChangeProfilePassword(ChangeProfilePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _AppUserService.ChangeProfilePasswordAsync(model).ConfigureAwait(false);
                if (result.Stat)
                {
                    await GetBaseService().AddActivity(ActivityType.Update, model.UserID, model.BUserName, "Change Password", "Changed user password");
                    return Json(new { stat = true, msg = "Successfully Changed user password" });
                }
                else
                    return Json(new { stat = false, msg = result.StatusMsg });
            }
            else
            {
                return Json(new { stat = false, msg = "Invalid User change password data" });
            }
        }
        #endregion

        #region App Settings        
        public async Task<IActionResult> AppSettings()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Settings", ActionUrl = "/Account/AppSettings" } });

            BaseViewModel VModel = null;

            var TempVModel = new SettingsVM
            {
                MailSettings = new MailSettingBM(),
                AppGeneralSettings = new GeneralSettingBM()
            };

            var oMailSetup = await _AppSettingService.GetMailSetting().ConfigureAwait(false);
            TempVModel.MailSettings = oMailSetup;

            var oGeneralSetup = await _AppSettingService.GetAppGeneralSetting().ConfigureAwait(false);
            TempVModel.AppGeneralSettings = oGeneralSetup;

            VModel = await GetViewModel(TempVModel);
            return View(VModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AppSettings(SettingsVM model)
        {
            if (ModelState.IsValid)
            {
                CommonResponce result = null;
                string ActivityMsg = "";
                if (model.Flag.Equals("GENERALSetting"))
                {
                    result = await _AppSettingService.SaveGeneralSetting(model.AppGeneralSettings).ConfigureAwait(false);
                    ActivityMsg = "Changed App Settings";
                }
                else
                {
                    result = await _AppSettingService.SaveMailSetting(model.MailSettings).ConfigureAwait(false);
                    ActivityMsg = "Changed Email Settings";
                }

                if (result.Stat)
                {
                    await GetBaseService().AddActivity(ActivityType.Update, model.BUserID, model.BUserName, "Settings", ActivityMsg);
                    return Json(new { stat = true, msg = "Successfully Changed settings" });
                }
                else
                    return Json(new { stat = false, msg = result.StatusMsg });
            }
            else
            {
                return Json(new { stat = false, msg = "Invalid application settings" });
            }
        }
        #endregion

        #region App Users        
        public async Task<IActionResult> AppUsers()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "App Users", ActionUrl = "/Account/AppUsers" } });

            BaseViewModel VModel = null;

            var result = await _AppUserService.GetAllAppUsers(500, GetBaseService().GetAppRootPath());

            var TempVModel = new AppUsersVM();
            TempVModel.AppUsersInfo = new AppGridModel<AppUserBM>();
            TempVModel.AppUsersInfo.Rows = result;

            VModel = await GetViewModel(TempVModel);

            return View(VModel);
        }

        public async Task<IActionResult> AppUser(int id = 0)
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "App Users", ActionUrl = "/Account/AppUsers" },
                                    new { Name = "User Profile", ActionUrl = "/Account/AppUser" } });

            BaseViewModel VModel = null;
            AppUserVM TempVModel = null;
            if (id != 0)
            {
                TempVModel = await _AppUserService.GetAppUserByID(id);
                if (TempVModel != null)
                {
                    TempVModel.UserImgPath = GetBaseService().GetUserAvatarPath(string.Format("{0}.{1}", TempVModel.UserId, "jpg"));
                    TempVModel.AttachUserImage = new FileUploadInfo();
                }
                else
                {
                    throw new Exception("Not a valid application User");
                }

            }
            else
            {
                TempVModel = new AppUserVM();
                TempVModel.UserImgPath = "~/assets/img/AppUser/BlankUser.jpg";
                TempVModel.AttachUserImage = new FileUploadInfo();
            }

            VModel = await GetViewModel(TempVModel);

            return View(VModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AppUser(AppUserVM model)
        {
            if (ModelState.IsValid)
            {
                string ResetContext = Guid.NewGuid().ToString().Replace("-", "RP");
                DateTime PassValidity = DateTime.Now.AddDays(1); //validity for 1 day
                var result = await _AppUserService.SaveAppUserAsync(model, ResetContext, PassValidity).ConfigureAwait(false);
                if (result.Stat)
                {
                    //save the user picture
                    if (model.AttachUserImage.FileSize > 0)
                    {
                        string UsrImgPath = Path.Combine(GetBaseService().GetAppRootPath(), "AppFileRepo\\UserAvatar");
                        if (GetBaseService().DirectoryFileService.CreateDirectoryIfNotExist(UsrImgPath))
                        {
                            UsrImgPath = string.Format("{0}\\{1}.{2}", UsrImgPath, model.UserId, "jpg");
                            if (GetBaseService().DirectoryFileService.CreateFileFromBase64String(model.AttachUserImage.FileContentsBase64, UsrImgPath))
                            {
                                model.UserImgPath = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", model.UserId, "jpg", DateTime.Now.Ticks.ToString());
                            }
                        }

                    }
                    if (model.Id != 0)
                    {
                        await GetBaseService().AddActivity(ActivityType.Update, model.BUserID, model.BUserName, "Update User", string.Format("Updated user : {0} info.", model.Name));
                        return Json(new { stat = true, msg = "Successfully updated user" });
                    }
                    else
                    {
                        var oLoginUser = GetLoginUserInfo();
                        await _JobService.AddNewUserCreateEmailJob(Convert.ToInt64(oLoginUser.ID), model.Name, model.Email, GetAppRootUrl());
                        await GetBaseService().AddActivity(ActivityType.Create, model.BUserID, model.BUserName, "Save User", string.Format("Save new user : {0} info.", model.Name));
                        return Json(new { stat = true, msg = "Successfully saved user" });
                    }
                }
                else
                    return Json(new { stat = false, msg = result.StatusMsg });
            }
            else
            {
                return Json(new { stat = false, msg = "Invalid User data" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteAppUser(long id)
        {
            var oUser = await _AppUserService.GetAppUserByID((int)id).ConfigureAwait(false);
            if (oUser != null)
            {
                var result = await _AppUserService.DeleteAppUser(id).ConfigureAwait(false);

                if (result.Stat)
                {
                    var CurrentUserInfo = GetLoginUserInfo();
                    await GetBaseService().AddActivity(ActivityType.Create, CurrentUserInfo.UserID, CurrentUserInfo.UserName, "Delete User", string.Format("Deleted user : {0}.", oUser.Name));
                }
                return Json(new { stat = result.Stat, msg = result.StatusMsg });
            }
            else
                return Json(new { stat = false, msg = "Not a valid User." });
        }

        [HttpPost]
        public async Task<IActionResult> ReloadAppUsers()
        {
            var result = await _AppUserService.GetAllAppUsers(500, GetBaseService().GetAppRootPath());

            var TempModel = new AppGridModel<AppUserBM>();
            TempModel.Rows = result;
                       
            return PartialView("_HTMLTable", TempModel);

        }
        #endregion

        #region App user reset section
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public async Task<IActionResult> ResetUserPass(string id)
        {
            BaseViewModel VModel = null;
            var model = new UserResetVM
            {
                UserResetContext = id,
                Password = "",
                ConfirmPassword = ""
            };
            VModel = await GetViewModel(model);            
            return View(VModel);            
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ResetUserPass(UserResetVM model)
        {            
            if (ModelState.IsValid)
            {
                var result = await _AppUserService.ResetUserPassAsync(model).ConfigureAwait(false);

                return Json(new { stat = result.Stat, msg = result.StatusMsg });
            }
            else
            {
                return Json(new { stat = false, msg = "Invalid Password reset data" });
            }
        }
        #endregion
    }
}