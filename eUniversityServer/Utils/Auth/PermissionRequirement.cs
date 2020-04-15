using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Utils.Auth
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public Permission Permission { get; set; }
        public PermissionRequirement(Permission permissions)
        {
            Permission = permissions;
        }
    }
}
