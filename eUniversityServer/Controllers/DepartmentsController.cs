using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Utils;
using eUniversityServer.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DepartmentDto = eUniversityServer.Services.Dtos.Department;
using Dtos = eUniversityServer.Services.Dtos;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Sieve.Models;
using eUniversityServer.Utils.Auth;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/departments")]
    public class DepartmentsController : SieveInfoControllerBase<DepartmentDto, Dtos.DepartmentInfo, CreateDepartmentBindingModel, UpdateDepartmentBindingModel, DepartmentViewModel, DepartmentInfoViewModel>,
                                         ISieveInfoController<DepartmentDto, Dtos.DepartmentInfo, CreateDepartmentBindingModel, UpdateDepartmentBindingModel, DepartmentViewModel, DepartmentInfoViewModel>
    {
        public DepartmentsController(IMapper mapper, IDepartmentService departmentService, Logger logger) : base(mapper, departmentService, logger)
        { }


        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Departments)]
        public new Task<ActionResult<IEnumerable<DepartmentViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Departments)]
        public new Task<ActionResult<DepartmentViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Departments)]
        public new Task<ActionResult<IEnumerable<DepartmentViewModel>>> Get(int page, int size) => base.Get(page, size);

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Departments)]
        public new async Task<ActionResult<SieveResponseModel<DepartmentViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.Departments)]
        public new Task<ActionResult<Guid>> Post([FromBody] CreateDepartmentBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.Departments)]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdateDepartmentBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.Departments)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);

        [HttpGet("search/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Departments)]
        public new async Task<ActionResult<SieveResponseModel<DepartmentInfoViewModel>>> GetWithMoreInfo([FromQuery]SieveModel sieveModel) => await base.GetWithMoreInfo(sieveModel);

        [HttpGet("moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Departments)]
        public new async Task<ActionResult<IEnumerable<DepartmentInfoViewModel>>> GetWithMoreInfo() => await base.GetWithMoreInfo();

        [HttpGet("{page}/{size}/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Departments)]
        public new async Task<ActionResult<IEnumerable<DepartmentInfoViewModel>>> GetWithMoreInfo(int page, int size) => await base.GetWithMoreInfo(page, size);
    }
}
