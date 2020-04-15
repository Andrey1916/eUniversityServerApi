using eUniversityServer.DAL;
using Microsoft.AspNetCore.Authorization;

namespace eUniversityServer.Utils
{
    public static class AppPolicies
    {
        public const string RequireElevatedPermissions = "RequireElevatedPermissions";
        public const string AdministratorsOnly = "AdministratorsOnly";

        public static void SetPolicies(AuthorizationOptions options)
        {
            options.AddPolicy(RequireElevatedPermissions, policy => policy.RequireRole(AppRoles.SuperAdmin));
            options.AddPolicy(AdministratorsOnly, policy => policy.RequireRole(AppRoles.SuperAdmin, AppRoles.Admin));
        }
    }
}
