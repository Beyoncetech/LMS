using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AppBAL.Sevices.Login;
using AppModel;
using AppUtility.AppEncription;
using AppUtility.AppModels;
using BTWebAppFrameWorkCore.AppSecurity;
using BTWebAppFrameWorkCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BTWebAppFrameWorkCore.Controllers
{
    [Authorize(Policy = "AllowAuthUsers")]
    public class AccountController : BaseController
    {
        private readonly IEmailSender _EmailSender;
        private readonly ILoginService _LoginService;
        private readonly IAppCookiesAuthService _AppCookiesAuth;

        public AccountController(ILoginService LoginService, IEmailSender EmailSender, AppSettingsConfiguration AppSettingsConfig, IAppCookiesAuthService AppCookiesAuth)
        {
            _LoginService = LoginService;
            _EmailSender = EmailSender;
            _AppSettingsConfig = AppSettingsConfig;
            _AppCookiesAuth = AppCookiesAuth;
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

            return Json(new { stat = true, msg = "Successfully Signed out"});
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
    }
}