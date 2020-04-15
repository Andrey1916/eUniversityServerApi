using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Utils;
using eUniversityServer.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeacherDto = eUniversityServer.Services.Dtos.Teacher;
using Dtos = eUniversityServer.Services.Dtos;
using Sieve.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using eUniversityServer.Utils.Auth;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/teachers")]
    public class TeachersController : SieveInfoControllerBase<TeacherDto, Dtos.TeacherInfo, CreateTeacherBindingModel, UpdateTeacherBindingModel, TeacherViewModel, TeacherInfoViewModel>,
                                      ISieveInfoController<TeacherDto, Dtos.TeacherInfo, CreateTeacherBindingModel, UpdateTeacherBindingModel, TeacherViewModel, TeacherInfoViewModel>
    {
        public TeachersController(IMapper mapper, ITeacherService teacherService, Logger logger) : base(mapper, teacherService, logger)
        { }


        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Teachers)]
        public new Task<ActionResult<IEnumerable<TeacherViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Teachers)]
        public new Task<ActionResult<TeacherViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Teachers)]
        public new Task<ActionResult<IEnumerable<TeacherViewModel>>> Get(int page, int size) => base.Get(page, size);

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Teachers)]
        public new async Task<ActionResult<SieveResponseModel<TeacherViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.Teachers)]
        public new Task<ActionResult<Guid>> Post([FromBody] CreateTeacherBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.Teachers)]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdateTeacherBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.Teachers)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);

        [HttpGet("search/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Teachers)]
        public new async Task<ActionResult<SieveResponseModel<TeacherInfoViewModel>>> GetWithMoreInfo([FromQuery]SieveModel sieveModel) => await base.GetWithMoreInfo(sieveModel);

        [HttpGet("moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Teachers)]
        public new async Task<ActionResult<IEnumerable<TeacherInfoViewModel>>> GetWithMoreInfo() => await base.GetWithMoreInfo();

        [HttpGet("{page}/{size}/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Teachers)]
        public new async Task<ActionResult<IEnumerable<TeacherInfoViewModel>>> GetWithMoreInfo(int page, int size) => await base.GetWithMoreInfo(page, size);
    }
}