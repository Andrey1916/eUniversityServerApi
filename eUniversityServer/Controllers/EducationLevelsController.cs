using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Logger;
using eUniversityServer.Utils;
using eUniversityServer.Utils.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EducationLevelDto = eUniversityServer.Services.Dtos.EducationLevel;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/education-levels")]
    public class EducationLevelsController : SieveControllerBase<EducationLevelDto, CreateEducationLevelBindingModel, UpdateEducationLevelBindingModel, EducationLevelViewModel>,
                                             ISieveController<EducationLevelDto, CreateEducationLevelBindingModel, UpdateEducationLevelBindingModel, EducationLevelViewModel>
    {
        public EducationLevelsController(IMapper mapper, IEducationLevelService educationLevelService, Logger logger) : base(mapper, educationLevelService, logger)
        { }


        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.EducationLevels)]
        public new Task<ActionResult<IEnumerable<EducationLevelViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.EducationLevels)]
        public new Task<ActionResult<EducationLevelViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.EducationLevels)]
        public new Task<ActionResult<IEnumerable<EducationLevelViewModel>>> Get(int page, int size) => base.Get(page, size);

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.EducationLevels)]
        public new async Task<ActionResult<SieveResponseModel<EducationLevelViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.EducationLevels)]
        public new Task<ActionResult<Guid>> Post([FromBody] CreateEducationLevelBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.EducationLevels)]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdateEducationLevelBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.EducationLevels)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);
    }
}
