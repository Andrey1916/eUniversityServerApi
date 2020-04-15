using System;
using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Utils;
using eUniversityServer.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleDto = eUniversityServer.Services.Dtos.Role;
using Dtos = eUniversityServer.Services.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using eUniversityServer.Services.Exceptions;
using Sieve.Models;
using eUniversityServer.DAL.Enums;
using eUniversityServer.Utils.Auth;

namespace eUniversityServer.Controllers
{
    [Authorize(Policy = AppPolicies.RequireElevatedPermissions)]
    [ApiController]
    [Route("api/roles")]
    public class RolesController : SieveControllerBase<RoleDto, CreateRoleBindingModel, UpdateRoleBindingModel, RoleViewModel>,
                                   ISieveController<RoleDto, CreateRoleBindingModel, UpdateRoleBindingModel, RoleViewModel>
    {
        private readonly IRoleService roleService;

        public RolesController(IMapper mapper, IRoleService service, Logger logger) : base(mapper, service, logger)
        {
            this.roleService = service ?? throw new NullReferenceException(nameof(service));
        }


        [HttpGet]
        public new Task<ActionResult<IEnumerable<RoleViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        public new Task<ActionResult<RoleViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        public new Task<ActionResult<IEnumerable<RoleViewModel>>> Get(int page, int size) => base.Get(page, size);

        [HttpGet("search")]
        public new async Task<ActionResult<SieveResponseModel<RoleViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        public new Task<ActionResult<Guid>> Post([FromBody] CreateRoleBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdateRoleBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);

        [HttpPatch("{roleId}/add-perm/{access}/{target}")]
        public async Task<ActionResult> AddPermToRole(Guid roleId, AccessModifier access, TargetModifier target)
        {
            if (roleId.Equals(Guid.Empty))
            {
                return this.BadRequest("Role Id is empty");
            }

            try
            {
                await roleService.AddPermissionToRoleAsync(roleId, access, target);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return StatusCode((int)ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                 await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPatch("{roleId}/add-perms")]
        public async Task<ActionResult> AddPermsToRole(Guid roleId, [FromBody] IEnumerable<Permission> permissions)
        {
            if (roleId.Equals(Guid.Empty))
            {
                return this.BadRequest("Role Id is empty");
            }

            try
            {
                var permissionDtos = _mapper.Map<IEnumerable<Permission>, IEnumerable<Dtos.Permission>>(permissions);

                await roleService.AddPermissionsToRoleAsync(roleId, permissionDtos);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return StatusCode((int)ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                 await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPatch("{roleId}/remove-perm/{access}/{target}")]
        public async Task<ActionResult> RemovePermFromRole(Guid roleId, AccessModifier access, TargetModifier target)
        {
            if (roleId.Equals(Guid.Empty))
            {
                return this.BadRequest("Role Id is empty");
            }

            try
            {
                await roleService.RemovePermissionFromRoleAsync(roleId, access, target);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return StatusCode((int)ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                 await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPatch("{roleId}/remove-perms")]
        public async Task<ActionResult> RemovePermsFromRole(Guid roleId, [FromBody] IEnumerable<Permission> permissions)
        {
            if (roleId.Equals(Guid.Empty))
            {
                return this.BadRequest("Role Id is empty");
            }

            try
            {
                var permissionDtos = _mapper.Map<IEnumerable<Permission>, IEnumerable<Dtos.Permission>>(permissions);

                await roleService.RemovePermissionsFromRoleAsync(roleId, permissionDtos);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return StatusCode((int)ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                 await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

    }
}
