using eUniversityServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Entities = eUniversityServer.DAL.Entities;
using RoleDto = eUniversityServer.Services.Dtos.Role;
using Sieve.Services;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Models;
using eUniversityServer.DAL;
using eUniversityServer.DAL.Enums;
using eUniversityServer.Services.Dtos;

namespace eUniversityServer.Services
{
    public class RoleService : IRoleService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public RoleService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        /// <exception cref="InvalidModelException"/>
        public async Task<Guid> AddAsync(RoleDto role)
        {
            var validator = new Dtos.RoleValidator();
            ValidationResult result = validator.Validate(role);

            if (!result.IsValid)
            {
                string errMess = string.Empty;

                foreach (var failure in result.Errors)
                {
                    errMess += $"Property { failure.PropertyName } failed validation. Error was: { failure.ErrorMessage }\n";
                }

                throw new InvalidModelException(errMess);
            }

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var roleEntity = new Entities.Role
            {
                Id        = id,
                CreatedAt = now,
                UpdatedAt = now,
                Name      = role.Name
            };

            await _context.AddAsync(roleEntity);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            return await _context.Set<Entities.Role>()
                                 .Include(r => r.RolePermissions)
                                 .ThenInclude(rp => rp.Permission)
                                 .Where(role => role.Name != AppRoles.SuperAdmin && role.Name != AppRoles.User)
                                 .AsNoTracking()
                                 .Select(role => new RoleDto
                                 {
                                     CreatedAt   = role.CreatedAt,
                                     Id          = role.Id,
                                     UpdatedAt   = role.UpdatedAt,
                                     Name        = role.Name,
                                     Permissions = role.RolePermissions
                                                       .GroupBy(x => x.Permission)
                                                       .Select(p => new Dtos.Permission
                                                       {
                                                           Id             = p.Key.Id,
                                                           AccessModifier = p.Key.AccessModifier,
                                                           TargetModifier = p.Key.TargetModifier
                                                       }
                                                       )
                                 })
                                 .OrderBy(role => role.Name)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.Role>()
                                 .Skip(page * size)
                                 .Take(size)
                                 .Where(role => role.Name != AppRoles.SuperAdmin && role.Name != AppRoles.User)
                                 .AsNoTracking()
                                 .Select(r => new Dtos.Role
                                 {
                                     CreatedAt   = r.CreatedAt,
                                     Id          = r.Id,
                                     UpdatedAt   = r.UpdatedAt,
                                     Name        = r.Name,
                                     Permissions = r.RolePermissions
                                                    .GroupBy(x => x.Permission)
                                                    .Select(p => new Dtos.Permission
                                                    {
                                                        Id             = p.Key.Id,
                                                        AccessModifier = p.Key.AccessModifier,
                                                        TargetModifier = p.Key.TargetModifier
                                                    }
                                                    )
                                 })
                                 .ToListAsync();
        }

        public async Task<RoleDto> GetByIdAsync(Guid id)
        {
            var role = await _context.FindAsync<Entities.Role>(id);
            
            if (role == null || role.Name == AppRoles.User || role.Name == AppRoles.SuperAdmin)
            {
                return null;
            }

            return new RoleDto
            {
                CreatedAt   = role.CreatedAt,
                Id          = role.Id,
                UpdatedAt   = role.UpdatedAt,
                Name        = role.Name,
                Permissions = _context.Set<Entities.RolePermissions>()
                                      .Include(rp => rp.Permission)
                                      .Where(rp => rp.RoleId == id)
                                      .Select(rp => new Dtos.Permission
                                      {
                                          Id             = rp.Permission.Id,
                                          TargetModifier = rp.Permission.TargetModifier,
                                          AccessModifier = rp.Permission.AccessModifier
                                      })
        };
        }

        public async Task<SieveResult<RoleDto>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var rolesQuery = _context.Set<Entities.Role>()
                                     .Include(r => r.RolePermissions)
                                     .ThenInclude(rp => rp.Permission)
                                     .Where(role => role.Name != AppRoles.SuperAdmin && role.Name != AppRoles.User)
                                     .AsNoTracking();

            rolesQuery = _sieveProcessor.Apply(model, rolesQuery, applyPagination: false);

            var result = new SieveResult<RoleDto>();
            result.TotalCount = await rolesQuery.CountAsync();

            var someRoles = _sieveProcessor.Apply(model, rolesQuery, applyFiltering: false, applySorting: false);

            result.Result = someRoles.Select(role => new RoleDto
            {
                CreatedAt   = role.CreatedAt,
                Id          = role.Id,
                UpdatedAt   = role.UpdatedAt,
                Name        = role.Name,
                Permissions = role.RolePermissions
                                  .GroupBy(rp => rp.Permission)
                                  .Select(p => new Dtos.Permission
                                  {
                                      Id             = p.Key.Id,
                                      AccessModifier = p.Key.AccessModifier,
                                      TargetModifier = p.Key.TargetModifier
                                  })
            });

            return result;
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var role = await _context.Set<Entities.Role>()
                                     .FindAsync(id);

            if (role == null)
            {
                throw new NotFoundException();
            }

            var usersWithRole = _context.Set<Entities.UserRoles>()
                                        .Where(ur => ur.RoleId == id);

            foreach(var ur in usersWithRole)
            {
                _context.Remove(ur);
            }

            _context.Remove(role);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(RoleDto dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.RoleValidator();
            ValidationResult result = validator.Validate(dto);

            if (!result.IsValid)
            {
                string errMess = string.Empty;

                foreach (var failure in result.Errors)
                {
                    errMess += $"Property { failure.PropertyName } failed validation. Error was: { failure.ErrorMessage }\n";
                }

                throw new InvalidModelException(errMess);
            }

            var role = await _context.FindAsync<Entities.Role>(dto.Id);

            if (role == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            role.UpdatedAt = DateTime.UtcNow;
            role.Name      = dto.Name;

            await _context.SaveChangesAsync();
        }

        public async Task AddPermissionToRoleAsync(Guid roleId, AccessModifier access, TargetModifier target)
        {
            var role = (await _context.FindAsync<Entities.Role>(roleId)) ?? throw new NotFoundException($"Role with Id: { roleId } not found");
            var permission = await _context.Set<Entities.Permission>()
                                           .FirstOrDefaultAsync(p => p.TargetModifier == target && p.AccessModifier == access);

            if (permission != null)
            {
                await _context.AddAsync(
                    new Entities.RolePermissions
                    {
                        Permission = permission,
                        Role = role
                    }
                    );
            }
            else
            {
                permission = new Entities.Permission
                {
                    AccessModifier = access,
                    CreatedAt      = DateTime.UtcNow,
                    Id             = Guid.NewGuid(),
                    TargetModifier = target,
                    UpdatedAt      = DateTime.UtcNow
                };

                await _context.AddAsync(permission);

                await _context.AddAsync(
                    new Entities.RolePermissions
                    {
                        Permission = permission,
                        Role       = role
                    }
                    );
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddPermissionsToRoleAsync(Guid roleId, IEnumerable<Permission> permissions)
        {
            var role = (await _context.FindAsync<Entities.Role>(roleId)) ?? throw new NotFoundException($"Role with Id: { roleId } not found");

            foreach (var perm in permissions)
            {
                var permission = await _context.Set<Entities.Permission>()
                                               .FirstOrDefaultAsync(p => p.TargetModifier == perm.TargetModifier && 
                                                                         p.AccessModifier == perm.AccessModifier);

                if (permission != null)
                {
                    await _context.AddAsync(
                        new Entities.RolePermissions
                        {
                            Permission = permission,
                            Role = role
                        }
                        );
                }
                else
                {
                    permission = new Entities.Permission
                    {
                        AccessModifier = perm.AccessModifier,
                        CreatedAt      = DateTime.UtcNow,
                        Id             = Guid.NewGuid(),
                        TargetModifier = perm.TargetModifier,
                        UpdatedAt      = DateTime.UtcNow
                    };

                    await _context.AddAsync(permission);

                    await _context.AddAsync(
                        new Entities.RolePermissions
                        {
                            Permission = permission,
                            Role = role
                        }
                        );
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemovePermissionFromRoleAsync(Guid roleId, AccessModifier access, TargetModifier target)
        {
            var rolePerm = await _context.Set<Entities.RolePermissions>()
                                         .Include(rp => rp.Permission)
                                         .FirstOrDefaultAsync(rp => rp.RoleId == roleId &&
                                                                    rp.Permission.TargetModifier == target &&
                                                                    rp.Permission.AccessModifier == access);

            if (rolePerm == null)
            {
                throw new NotFoundException("Role not found");
            }

            var permId = rolePerm.PermissionId;

            _context.Remove(rolePerm);

            var hasAnyRoleWithPerm = await _context.Set<Entities.RolePermissions>()
                                                   .AnyAsync(rp => rp.PermissionId == permId);

            if (!hasAnyRoleWithPerm)
            {
                _context.Remove(
                    await _context.FindAsync<Entities.Permission>(permId)
                    );
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemovePermissionsFromRoleAsync(Guid roleId, IEnumerable<Permission> permissions)
        {
            foreach (var perm in permissions)
            {
                var rolePerm = await _context.Set<Entities.RolePermissions>()
                                             .Include(rp => rp.Permission)
                                             .FirstOrDefaultAsync(rp => rp.RoleId == roleId &&
                                                                        rp.Permission.TargetModifier == perm.TargetModifier &&
                                                                        rp.Permission.AccessModifier == perm.AccessModifier);

                if (rolePerm == null)
                {
                    throw new NotFoundException("Role not found");
                }

                var permId = rolePerm.PermissionId;

                _context.Remove(rolePerm);

                var hasAnyRoleWithPerm = await _context.Set<Entities.RolePermissions>()
                                                       .AnyAsync(rp => rp.PermissionId == permId);

                if (!hasAnyRoleWithPerm)
                {
                    _context.Remove(
                        await _context.FindAsync<Entities.Permission>(permId)
                        );
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
