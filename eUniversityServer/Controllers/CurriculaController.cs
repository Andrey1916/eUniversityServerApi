using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Utils;
using eUniversityServer.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CurriculumDto = eUniversityServer.Services.Dtos.Curriculum;
using Dtos = eUniversityServer.Services.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Sieve.Models;
using eUniversityServer.Utils.Auth;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/curriculum")]
    public class CurriculaController : SieveInfoControllerBase<CurriculumDto, Dtos.CurriculumInfo, CreateCurriculumBindingModel, UpdateCurriculumBindingModel, CurriculumViewModel, CurriculumInfoViewModel>,
                                       ISieveInfoController<CurriculumDto, Dtos.CurriculumInfo, CreateCurriculumBindingModel, UpdateCurriculumBindingModel, CurriculumViewModel, CurriculumInfoViewModel>
    {
        public CurriculaController(IMapper mapper, ICurriculumService curriculaService, Logger logger) : base(mapper, curriculaService, logger)
        { }


        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Curricula)]
        public new Task<ActionResult<IEnumerable<CurriculumViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Curricula)]
        public new Task<ActionResult<CurriculumViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Curricula)]
        public new Task<ActionResult<IEnumerable<CurriculumViewModel>>> Get(int page, int size) => base.Get(page, size);

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Curricula)]
        public new async Task<ActionResult<SieveResponseModel<CurriculumViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.Curricula)]
        public new Task<ActionResult<Guid>> Post([FromBody] CreateCurriculumBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.Curricula)]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdateCurriculumBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.Curricula)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);

        [HttpGet("search/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Curricula)]
        public new async Task<ActionResult<SieveResponseModel<CurriculumInfoViewModel>>> GetWithMoreInfo([FromQuery]SieveModel sieveModel) => await base.GetWithMoreInfo(sieveModel);

        [HttpGet("moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Curricula)]
        public new async Task<ActionResult<IEnumerable<CurriculumInfoViewModel>>> GetWithMoreInfo() => await base.GetWithMoreInfo();

        [HttpGet("{page}/{size}/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Curricula)]
        public new async Task<ActionResult<IEnumerable<CurriculumInfoViewModel>>> GetWithMoreInfo(int page, int size) => await base.GetWithMoreInfo(page, size);
    }
}
