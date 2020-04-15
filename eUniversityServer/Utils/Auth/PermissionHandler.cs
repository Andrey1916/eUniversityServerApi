using eUniversityServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eUniversityServer.Utils.Auth
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        
        private readonly IUserService service;

        public PermissionHandler(IUserService service) : base()
        {
            this.service = service ?? throw new NullReferenceException(nameof(service));
        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var idClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            
            if (idClaim == null)
            {
                return Task.CompletedTask;
            }

            if (Guid.TryParse(idClaim.Value, out Guid id))
            {
                bool hasPerm = service.HasPermissionAsync(id, requirement.Permission.AccessModifier, requirement.Permission.TargetModifier).Result;

                if (!hasPerm)
                {
                    return Task.CompletedTask;
                }

                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
