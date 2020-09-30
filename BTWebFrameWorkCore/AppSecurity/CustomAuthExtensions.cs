using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTWebAppFrameWorkCore.AppSecurity
{
    public static class CustomAuthExtensions
    {
        public static AuthenticationBuilder AddCustomCookieAuth(this AuthenticationBuilder builder, Action<CustomCookieAuthSchema> configureOptions)
        {
            return builder.AddScheme<CustomCookieAuthSchema, CustomCookieAuthHandler>("CustomCookieSchema", "Custom Cookie Schema", configureOptions);
        }

        public static AuthenticationBuilder AddCustomJWTAuth(this AuthenticationBuilder builder, Action<CustomJWTAuthSchema> configureOptions)
        {
            return builder.AddScheme<CustomJWTAuthSchema, CustomJWTAuthHandler>("CustomJWTSchema", "Custom JWT Schema", configureOptions);
        }
    }
}
