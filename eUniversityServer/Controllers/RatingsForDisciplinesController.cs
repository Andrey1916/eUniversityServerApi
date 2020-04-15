using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Utils;
using eUniversityServer.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RatingForDisciplineDto = eUniversityServer.Services.Dtos.RatingForDiscipline;
using Dtos = eUniversityServer.Services.Dtos;
using System;
using Sieve.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using eUniversityServer.Utils.Auth;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/ratings-for-disciplines")]
    public class RatingsForDisciplinesController : SieveInfoControllerBase<RatingForDisciplineDto, Dtos.RatingForDisciplineInfo, CreateRatingForDisciplineBindingModel, UpdateRatingForDisciplineBindingModel, RatingForDisciplineViewModel, RatingForDisciplineInfoViewModel>,
                                                   ISieveInfoController<RatingForDisciplineDto, Dtos.RatingForDisciplineInfo, CreateRatingForDisciplineBindingModel, UpdateRatingForDisciplineBindingModel, RatingForDisciplineViewModel, RatingForDisciplineInfoViewModel>
    {
        public RatingsForDisciplinesController(IMapper mapper, IRatingForDisciplineService ratingForDisciplineService, Logger logger) : base(mapper, ratingForDisciplineService, logger)
        { }

        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.RatingsForDisciplines)]
        public new Task<ActionResult<IEnumerable<RatingForDisciplineViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.RatingsForDisciplines)]
        public new Task<ActionResult<RatingForDisciplineViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.RatingsForDisciplines)]
        public new Task<ActionResult<IEnumerable<RatingForDisciplineViewModel>>> Get(int page, int size) => base.Get(page, size);

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.RatingsForDisciplines)]
        public new async Task<ActionResult<SieveResponseModel<RatingForDisciplineViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.RatingsForDisciplines)]
        public new Task<ActionResult<Guid>> Post([FromBody] CreateRatingForDisciplineBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.RatingsForDisciplines)]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdateRatingForDisciplineBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.RatingsForDisciplines)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);

        [HttpGet("search/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.RatingsForDisciplines)]
        public new async Task<ActionResult<SieveResponseModel<RatingForDisciplineInfoViewModel>>> GetWithMoreInfo([FromQuery]SieveModel sieveModel) => await base.GetWithMoreInfo(sieveModel);

        [HttpGet("moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.RatingsForDisciplines)]
        public new async Task<ActionResult<IEnumerable<RatingForDisciplineInfoViewModel>>> GetWithMoreInfo() => await base.GetWithMoreInfo();

        [HttpGet("{page}/{size}/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.RatingsForDisciplines)]
        public new async Task<ActionResult<IEnumerable<RatingForDisciplineInfoViewModel>>> GetWithMoreInfo(int page, int size) => await base.GetWithMoreInfo(page, size);
    }
}