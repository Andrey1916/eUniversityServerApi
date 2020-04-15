using eUniversityServer.DAL.Enums;
using eUniversityServer.Services.Dtos;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Interfaces
{
    public interface IRoleService : IService<Role>
    {
        Task AddPermissionToRoleAsync(Guid roleId, AccessModifier access, TargetModifier target);

        Task AddPermissionsToRoleAsync(Guid roleId, IEnumerable<Dtos.Permission> permissions);


        Task RemovePermissionFromRoleAsync(Guid roleId, AccessModifier access, TargetModifier target);

        Task RemovePermissionsFromRoleAsync(Guid roleId, IEnumerable<Dtos.Permission> permissions);
    }
}
