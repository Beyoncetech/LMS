using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BTWebAppFrameWorkCore.AppSecurity
{
    public class CustomJWTAuthSchema : AuthenticationSchemeOptions
    {
        public CustomJWTAuthSchema()
        {

        }
    }
    public class CustomJWTAuthHandler : AuthenticationHandler<CustomJWTAuthSchema>
    {
        public CustomJWTAuthHandler(IOptionsMonitor<CustomJWTAuthSchema> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            // store custom services here...
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // build the claims and put them in "Context"; you need to import the Microsoft.AspNetCore.Authentication package
            if (Request.Headers.ContainsKey("Authorization"))
            {
                //Authorization header not in request
                if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out AuthenticationHeaderValue headerValue))
                {
                    //Invalid Authorization header
                    return AuthenticateResult.Fail("invalid Auth Header");
                }
                AppTokenHandler TokenHdlr = new AppTokenHandler();
                var tokenPrincipal = await TokenHdlr.ValidateJWTToken(headerValue.Parameter);
                if (tokenPrincipal != null)
                {
                    var ticket = new AuthenticationTicket(tokenPrincipal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    return AuthenticateResult.Fail("invalid token");
                }
            }
                       
            
            return AuthenticateResult.NoResult();
        }
    }
       
}
