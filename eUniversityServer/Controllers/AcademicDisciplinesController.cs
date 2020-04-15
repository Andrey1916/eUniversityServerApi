using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Dtos;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Logger;
using eUniversityServer.Utils;
using eUniversityServer.Utils.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using AcademicDisciplineDto = eUniversityServer.Services.Dtos.AcademicDiscipline;
using AcademicDisciplineDtoInfo = eUniversityServer.Services.Dtos.AcademicDisciplineInfo;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/disciplines")]
    public class AcademicDisciplinesController : SieveInfoControllerBase<AcademicDisciplineDto, AcademicDisciplineDtoInfo, CreateAcademicDisciplineBindingModel, UpdateAcademicDisciplineBindingModel, AcademicDisciplineViewModel, AcademicDisciplineInfoViewModel>,
                                                 ISieveInfoController<AcademicDisciplineDto, AcademicDisciplineDtoInfo, CreateAcademicDisciplineBindingModel, UpdateAcademicDisciplineBindingModel, AcademicDisciplineViewModel, AcademicDisciplineInfoViewModel>
    {
        public AcademicDisciplinesController(IMapper mapper, IAcademicDisciplineService disciplineService, Logger logger) : base(mapper, disciplineService, logger)
        { }

        /// <summary>
        /// Get all Academic Disciplines
        /// </summary>
        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicDisciplines)]
        public new async Task<ActionResult<IEnumerable<AcademicDisciplineViewModel>>> Get() => await base.Get();

        /// <summary>
        /// Get Academic Disciplines by id
        /// </summary>
        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicDisciplines)]
        public new async Task<ActionResult<AcademicDisciplineViewModel>> Get(Guid id) => await base.Get(id);

        /// <summary>
        /// Get users by pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicDisciplines)]
        public new async Task<ActionResult<IEnumerable<AcademicDisciplineViewModel>>> Get(int page, int size) => await base.Get(page, size);

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicDisciplines)]
        public new async Task<ActionResult<SieveResponseModel<AcademicDisciplineViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.AcademicDisciplines)]
        public new async Task<ActionResult<Guid>> Post([FromBody] CreateAcademicDisciplineBindingModel model) => await base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.AcademicDisciplines)]
        public new async Task<ActionResult> Put(Guid id, [FromBody] UpdateAcademicDisciplineBindingModel model) => await base.Put(id, model);

        /// <summary>
        /// Delete user by Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.AcademicDisciplines)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);
        
        [HttpGet("search/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicDisciplines)]
        public new async Task<ActionResult<SieveResponseModel<AcademicDisciplineInfoViewModel>>> GetWithMoreInfo([FromQuery]SieveModel sieveModel) => await base.GetWithMoreInfo(sieveModel);

        [HttpGet("moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicDisciplines)]
        public new async Task<ActionResult<IEnumerable<AcademicDisciplineInfoViewModel>>> GetWithMoreInfo() => await base.GetWithMoreInfo();

        [HttpGet("{page}/{size}/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicDisciplines)]
        public new async Task<ActionResult<IEnumerable<AcademicDisciplineInfoViewModel>>> GetWithMoreInfo(int page, int size) => await base.GetWithMoreInfo(page, size);
    }
}
