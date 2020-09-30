using AppModel;
using AppUtility.AppEncription;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BTWebAppFrameWorkCore.AppSecurity
{
    public interface IAppCookiesAuthService
    {
        Task SignInAsync<T>(string key, T value, bool isPersistent);
        Task SignOutAsync(string key);
    }

    public class AppCookiesAuthService : IAppCookiesAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEncriptionService _AppEncription;
        public AppCookiesAuthService(IHttpContextAccessor httpContextAccessor, IEncriptionService AppEncription)
        {
            _httpContextAccessor = httpContextAccessor;
            _AppEncription = AppEncription;
        }
        
        public async Task SignInAsync<T>(string key, T value, bool isPersistent)
        {
            CookieOptions options = new CookieOptions();
            if (isPersistent)
                options.Expires = DateTime.Now.AddDays(1);
            else
                options.Expires = DateTime.Now.AddHours(1);

            string ResultValue = _AppEncription.ConvertObjectToBase64String(value);

            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, ResultValue, options);

            await Task.Delay(10).ConfigureAwait(false);
        }

        public async Task SignOutAsync(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
            
            await Task.Delay(10).ConfigureAwait(false);
        }
    }
}
