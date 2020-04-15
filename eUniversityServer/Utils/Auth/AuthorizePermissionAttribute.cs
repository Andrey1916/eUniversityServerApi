using Enums = eUniversityServer.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Utils.Auth
{
    public class PermissionFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authService;
        private readonly Permission _permission;

        public PermissionFilter(IAuthorizationService authService, Permission permission)
        {
            _authService = authService;
            _permission = permission;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool ok = (await _authService.AuthorizeAsync(context.HttpContext.User, null, new PermissionRequirement(_permission))).Succeeded;

            if (!ok) context.Result = new ChallengeResult();
        }
    }

    public class AuthorizePermissionAttribute : TypeFilterAttribute
    {
        public AuthorizePermissionAttribute(Enums.AccessModifier access, Enums.TargetModifier target)
              : base(typeof(PermissionFilter))
        {
            Arguments = new[] { new Permission(access, target) };
            Order = Int32.MaxValue;
        }
    }
}
