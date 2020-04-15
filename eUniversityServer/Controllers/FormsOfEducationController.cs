using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Utils;
using eUniversityServer.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FormOfEducationDto = eUniversityServer.Services.Dtos.FormOfEducation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Sieve.Models;
using eUniversityServer.Utils.Auth;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/forms-of-education")]
    public class FormsOfEducationController : SieveControllerBase<FormOfEducationDto, CreateFormOfEducationBindingModel, UpdateFormOfEducationBindingModel, FormOfEducationViewModel>,
                                              ISieveController<FormOfEducationDto, CreateFormOfEducationBindingModel, UpdateFormOfEducationBindingModel, FormOfEducationViewModel>
    {
        public FormsOfEducationController(IMapper mapper, IFormOfEducationService formOfEducationService, Logger logger) : base(mapper, formOfEducationService, logger)
        { }


        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.FormsOfEducation)]
        public new Task<ActionResult<IEnumerable<FormOfEducationViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.FormsOfEducation)]
        public new Task<ActionResult<FormOfEducationViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.FormsOfEducation)]
        public new Task<ActionResult<IEnumerable<FormOfEducationViewModel>>> Get(int page, int size) => base.Get(page, size);

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.FormsOfEducation)]
        public new async Task<ActionResult<SieveResponseModel<FormOfEducationViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.FormsOfEducation)]
        public new Task<ActionResult<Guid>> Post([FromBody] CreateFormOfEducationBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.FormsOfEducation)]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdateFormOfEducationBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.FormsOfEducation)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);
    }
}