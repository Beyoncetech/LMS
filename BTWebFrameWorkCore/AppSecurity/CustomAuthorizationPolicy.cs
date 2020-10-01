using AppBAL.Sevices.Authentication;
using AppUtility.AppEncription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTWebAppFrameWorkCore.AppSecurity
{
    public class ManageUserPermissionRequirement : IAuthorizationRequirement
    {
        public Boolean IsCaching { get; set; }
        public ManageUserPermissionRequirement(Boolean isCaching = false)
        {
            IsCaching = isCaching;
        }
    }

    public class CanAllowOnlyAuthUsersHandler : AuthorizationHandler<ManageUserPermissionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISiteMapService _SiteMapService;
        public CanAllowOnlyAuthUsersHandler(IHttpContextAccessor httpContextAccessor, ISiteMapService SiteMapService)
        { 
            _httpContextAccessor = httpContextAccessor;
            _SiteMapService = SiteMapService;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageUserPermissionRequirement requirement)
        {           
            if (!context.User.HasClaim(Claim => Claim.Type == "UserID"))
            {
                return Task.CompletedTask;
            }
            if (!context.User.HasClaim(Claim => Claim.Type == "UserType"))
            {
                return Task.CompletedTask;
            }
            if (!context.User.HasClaim(Claim => Claim.Type == "UserPerm"))
            {
                return Task.CompletedTask;
            }
            var UserType = context.User.Claims.FirstOrDefault(c => c.Type == "UserType").Value;
            var UserPerm = context.User.Claims.FirstOrDefault(c => c.Type == "UserPerm").Value;
            // admin have access all resources
            if (UserType.Equals("A"))
            {
                context.Succeed(requirement);
            }
            else // if not admin then check the permission on that page
            {
                var CurActionPath = _httpContextAccessor.HttpContext.Request.Path.Value;
                if(_SiteMapService.IsValidAction(CurActionPath, UserPerm))
                {
                    context.Succeed(requirement);
                }                
            }            
            
            return Task.CompletedTask;
        }
    }
}
