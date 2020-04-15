using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Utils;
using eUniversityServer.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExamsGradesSpreadsheetDto = eUniversityServer.Services.Dtos.ExamsGradesSpreadsheet;
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
    [Route("api/exams-grades-spreadsheets")]
    public class ExamsGradesSpreadsheetsController : SieveInfoControllerBase<ExamsGradesSpreadsheetDto, Dtos.ExamsGradesSpreadsheetInfo, CreateExamsGradesSpreadsheetBindingModel, UpdateExamsGradesSpreadsheetBindingModel, ExamsGradesSpreadsheetViewModel, ExamsGradesSpreadsheetInfoViewModel>, ISieveInfoController<ExamsGradesSpreadsheetDto, Dtos.ExamsGradesSpreadsheetInfo, CreateExamsGradesSpreadsheetBindingModel, UpdateExamsGradesSpreadsheetBindingModel, ExamsGradesSpreadsheetViewModel, ExamsGradesSpreadsheetInfoViewModel>
    {
        public ExamsGradesSpreadsheetsController(IMapper mapper, IExamsGradesSpreadsheetService examsGradesSpreadsheetService, Logger logger) : base(mapper, examsGradesSpreadsheetService, logger)
        { }


        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.ExamsGradesSpreadsheets)]
        public new Task<ActionResult<IEnumerable<ExamsGradesSpreadsheetViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.ExamsGradesSpreadsheets)]
        public new Task<ActionResult<ExamsGradesSpreadsheetViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.ExamsGradesSpreadsheets)]
        public new Task<ActionResult<IEnumerable<ExamsGradesSpreadsheetViewModel>>> Get(int page, int size) => base.Get(page, size);

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.ExamsGradesSpreadsheets)]
        public new async Task<ActionResult<SieveResponseModel<ExamsGradesSpreadsheetViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.ExamsGradesSpreadsheets)]
        public new Task<ActionResult<Guid>> Post([FromBody] CreateExamsGradesSpreadsheetBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.ExamsGradesSpreadsheets)]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdateExamsGradesSpreadsheetBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.ExamsGradesSpreadsheets)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);

        [HttpGet("search/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.ExamsGradesSpreadsheets)]
        public new async Task<ActionResult<SieveResponseModel<ExamsGradesSpreadsheetInfoViewModel>>> GetWithMoreInfo([FromQuery]SieveModel sieveModel) => await base.GetWithMoreInfo(sieveModel);

        [HttpGet("moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.ExamsGradesSpreadsheets)]
        public new async Task<ActionResult<IEnumerable<ExamsGradesSpreadsheetInfoViewModel>>> GetWithMoreInfo() => await base.GetWithMoreInfo();

        [HttpGet("{page}/{size}/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.ExamsGradesSpreadsheets)]
        public new async Task<ActionResult<IEnumerable<ExamsGradesSpreadsheetInfoViewModel>>> GetWithMoreInfo(int page, int size) => await base.GetWithMoreInfo(page, size);
    }
}