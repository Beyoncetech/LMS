using AppModel;
using AppUtility.AppEncription;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BTWebAppFrameWorkCore.AppSecurity
{
    public class CustomCookieAuthSchema : AuthenticationSchemeOptions
    {
        public CustomCookieAuthSchema()
        {

        }
    }
    public class CustomCookieAuthHandler : AuthenticationHandler<CustomCookieAuthSchema>
    {
        private readonly IEncriptionService _AppEncription;
        public CustomCookieAuthHandler(IOptionsMonitor<CustomCookieAuthSchema> options, ILoggerFactory logger, 
            UrlEncoder encoder, ISystemClock clock, IEncriptionService AppEncription) : base(options, logger, encoder, clock)
        {
            // store custom services here...
            _AppEncription = AppEncription;            
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //check the cookies present
            if (Request.Cookies["LMSAuthCookies"] != null)
            {
                string TempCookiesValue = Request.Cookies["LMSAuthCookies"];                
                try
                {
                    var TempCookiesClaim = _AppEncription.ConvertBase64StringToObject<List<AppKeyValueInfo>>(TempCookiesValue);
                    if(TempCookiesClaim != null || TempCookiesClaim.Count == 0)
                        return AuthenticateResult.Fail("Invalid Token.");

                    var AccessToken = TempCookiesClaim.FirstOrDefault(x => x.Key == "JWTToken")?.Value;
                    if (!string.IsNullOrWhiteSpace(AccessToken))
                    {
                        AppTokenHandler TokenHdlr = new AppTokenHandler();
                        var IsValidToken = await TokenHdlr.ValidateJWTToken(AccessToken);
                        if (IsValidToken != null)
                        {
                            var principal = new ClaimsPrincipal(Request.HttpContext.User.Identity);
                            var ticket = new AuthenticationTicket(principal, Scheme.Name);
                            return AuthenticateResult.Success(ticket);
                        }
                        else
                        {                            
                            return AuthenticateResult.Fail("Token not valid or expire");
                        }
                    }
                    else
                    {
                        return AuthenticateResult.Fail("Token not found");
                    }
                }
                catch (Exception)
                {
                    return AuthenticateResult.Fail("Invalid Token.");
                }               
                       
            }
            
            return AuthenticateResult.NoResult();
        }
        
    }
   
}
