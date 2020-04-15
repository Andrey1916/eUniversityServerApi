using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Utils;
using eUniversityServer.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StructuralUnitDto = eUniversityServer.Services.Dtos.StructuralUnit;
using Sieve.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using eUniversityServer.Utils.Auth;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/structural-units")]
    public class StructuralUnitsController : SieveControllerBase<StructuralUnitDto, CreateStructuralUnitBindingModel, UpdateStructuralUnitBindingModel, StructuralUnitViewModel>,
                                             ISieveController<StructuralUnitDto, CreateStructuralUnitBindingModel, UpdateStructuralUnitBindingModel, StructuralUnitViewModel>
    {
        public StructuralUnitsController(IMapper mapper, IStructuralUnitService structuralUnitService, Logger logger) : base(mapper, structuralUnitService, logger)
        { }


        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.StructuralUnits)]
        public new Task<ActionResult<IEnumerable<StructuralUnitViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.StructuralUnits)]
        public new Task<ActionResult<StructuralUnitViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.StructuralUnits)]
        public new Task<ActionResult<IEnumerable<StructuralUnitViewModel>>> Get(int page, int size) => base.Get(page, size);

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.StructuralUnits)]
        public new async Task<ActionResult<SieveResponseModel<StructuralUnitViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.StructuralUnits)]
        public new Task<ActionResult<Guid>> Post([FromBody] CreateStructuralUnitBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.StructuralUnits)]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdateStructuralUnitBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.StructuralUnits)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);
    }
}