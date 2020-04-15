using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Logger;
using eUniversityServer.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EducationProgramDto = eUniversityServer.Services.Dtos.EducationProgram;
using Dtos = eUniversityServer.Services.Dtos;
using eUniversityServer.Utils.Auth;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/education-programs")]
    public class EducationProgramsController : SieveInfoControllerBase<EducationProgramDto, Dtos.EducationProgramInfo, CreateEducationProgramBindingModel, UpdateEducationProgramBindingModel, EducationProgramViewModel, EducationProgramInfoViewModel>,
                                               ISieveInfoController<EducationProgramDto, Dtos.EducationProgramInfo, CreateEducationProgramBindingModel, UpdateEducationProgramBindingModel, EducationProgramViewModel, EducationProgramInfoViewModel>
    {
        public EducationProgramsController(IMapper mapper, IEducationProgramService educationProgramService, Logger logger) : base(mapper, educationProgramService, logger)
        { }


        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.EducationPrograms)]
        public new Task<ActionResult<IEnumerable<EducationProgramViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.EducationPrograms)]
        public new Task<ActionResult<EducationProgramViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.EducationPrograms)]
        public new Task<ActionResult<IEnumerable<EducationProgramViewModel>>> Get(int page, int size) => base.Get(page, size);

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.EducationPrograms)]
        public new async Task<ActionResult<SieveResponseModel<EducationProgramViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.EducationPrograms)]
        public new Task<ActionResult<Guid>> Post([FromBody] CreateEducationProgramBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.EducationPrograms)]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdateEducationProgramBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.EducationPrograms)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);

        [HttpGet("search/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.EducationPrograms)]
        public new async Task<ActionResult<SieveResponseModel<EducationProgramInfoViewModel>>> GetWithMoreInfo([FromQuery]SieveModel sieveModel) => await base.GetWithMoreInfo(sieveModel);

        [HttpGet("moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.EducationPrograms)]
        public new async Task<ActionResult<IEnumerable<EducationProgramInfoViewModel>>> GetWithMoreInfo() => await base.GetWithMoreInfo();

        [HttpGet("{page}/{size}/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.EducationPrograms)]
        public new async Task<ActionResult<IEnumerable<EducationProgramInfoViewModel>>> GetWithMoreInfo(int page, int size) => await base.GetWithMoreInfo(page, size);
    }
}
