using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Utils;
using eUniversityServer.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivilegeDto = eUniversityServer.Services.Dtos.Privilege;
using Sieve.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using eUniversityServer.Services.Dtos;
using eUniversityServer.Utils.Auth;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/privileges")]
    public class PrivilegesController : SieveControllerBase<PrivilegeDto, CreatePrivilegeBindingModel, UpdatePrivilegeBindingModel, PrivilegeViewModel>,
                                        ISieveController<PrivilegeDto, CreatePrivilegeBindingModel, UpdatePrivilegeBindingModel, PrivilegeViewModel>
    {
        public PrivilegesController(IMapper mapper, IPrivilegeService privilegeService, Logger logger) : base(mapper, privilegeService, logger)
        { }


        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Privileges)]
        public new Task<ActionResult<IEnumerable<PrivilegeViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Privileges)]
        public new Task<ActionResult<PrivilegeViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Privileges)]
        public new Task<ActionResult<IEnumerable<PrivilegeViewModel>>> Get(int page, int size) => base.Get(page, size);
        
        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Privileges)]
        public new async Task<ActionResult<SieveResponseModel<PrivilegeViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);
        
        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.Privileges)]
        public new Task<ActionResult<Guid>> Post([FromBody] CreatePrivilegeBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.Privileges)]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdatePrivilegeBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.Privileges)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);
    }
}