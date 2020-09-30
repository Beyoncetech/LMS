using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BTWebAppFrameWorkCore.AppSecurity
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AppRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public AppRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // Redirect to login if user is not authenticated. This instruction is neccessary for JS async calls, otherwise everycall will return unauthorized without explaining why
            //if(httpContext.Response.StatusCode == 401 && httpContext.Request.Path.Value != "/Account/Login")
            
            if (httpContext.Response.StatusCode == 401 && (httpContext.Request.Path.Value != "/Account/Login" || httpContext.Request.Path.Value != "/"))
            {
                httpContext.Response.Redirect("/Account/Login");
            }

            // Move forward into the pipeline
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AppRedirectMiddlewareExtensions
    {
        public static IApplicationBuilder UseAppRedirectMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AppRedirectMiddleware>();
        }
    }
}
