using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AppBAL.Sevices.Login;
using AppModel;
using AppUtility.AppModels;
using BTWebAppFrameWorkCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BTWebAppFrameWorkCore.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IEmailSender _EmailSender;
        private readonly ILoginService _LoginService;        

        public AccountController(ILoginService LoginService, IEmailSender EmailSender, AppSettingsConfiguration AppSettingsConfig)
        {
            _LoginService = LoginService;
            _EmailSender = EmailSender;
            _AppSettingsConfig = AppSettingsConfig;
        }
        #region App login
        public IActionResult Login()
        {
            var VModel = GetViewModel<LoginVM>();            
            return View(VModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _LoginService.ValidateUser(model.UserName, model.Password);
                if(result.Stat)
                {
                    LoginUser UserInfo = (LoginUser) result.StatusObj;
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim("UserID", model.UserName));
                    identity.AddClaim(new Claim("UserType", UserInfo.UserType));
                    identity.AddClaim(new Claim("UserPerm", string.IsNullOrEmpty(UserInfo.UserPerm)? "": UserInfo.UserPerm));
                    //foreach (var role in user.Roles)
                    //{
                    //    identity.AddClaim(new Claim(ClaimTypes.Role, role.Role));
                    //}
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    //return RedirectToAction("Index", "Home");
                    return Json(new { Stat = true, Msg = "Valid User", RtnUrl = "Home/Index" });
                }
                else
                    return Json(new { Stat = false, Msg = "Invalid UserId and Password" });
            }
            else
            {
                return Json(new { Stat = false, Msg = "Invalid UserId and Password" });
            }

        }
        #endregion

        #region App Error

        #endregion
    }
}