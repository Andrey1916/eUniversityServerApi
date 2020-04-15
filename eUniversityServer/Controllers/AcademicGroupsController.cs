using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Logger;
using eUniversityServer.Utils;
using eUniversityServer.Utils.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicGroupDto = eUniversityServer.Services.Dtos.AcademicGroup;
using Dtos = eUniversityServer.Services.Dtos;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/groups")]
    public class AcademicGroupsController : SieveInfoControllerBase<AcademicGroupDto, Dtos.AcademicGroupInfo, CreateAcademicGroupBindingModel, UpdateAcademicGroupBindingModel, AcademicGroupViewModel, AcademicGroupInfoViewModel>,
                                            ISieveInfoController<AcademicGroupDto, Dtos.AcademicGroupInfo, CreateAcademicGroupBindingModel, UpdateAcademicGroupBindingModel, AcademicGroupViewModel, AcademicGroupInfoViewModel>
    {
        private readonly IAcademicGroupService _academicGroupService;

        public AcademicGroupsController(IMapper mapper, IAcademicGroupService groupService, Logger logger) : base(mapper, groupService, logger)
        {
            this._academicGroupService = groupService ?? throw new NullReferenceException(nameof(groupService));
        }


        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicGroups)]
        public new Task<ActionResult<IEnumerable<AcademicGroupViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicGroups)]
        public new Task<ActionResult<AcademicGroupViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicGroups)]
        public new Task<ActionResult<IEnumerable<AcademicGroupViewModel>>> Get(int page, int size) => base.Get(page, size);

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicGroups)]
        public new async Task<ActionResult<SieveResponseModel<AcademicGroupViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.Curricula)]
        public new Task<ActionResult<Guid>> Post([FromBody] CreateAcademicGroupBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.Curricula)]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdateAcademicGroupBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.Curricula)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);
        
        [HttpGet("{id}/students")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Students)]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicGroups)]
        public async Task<ActionResult<IEnumerable<StudentViewModel>>> GetStudents(Guid groupId)
        {
            if (groupId.Equals(Guid.Empty))
                return BadRequest("Id is empty");

            try
            {
                var students = await _academicGroupService.GetStudentsAsync(groupId);

                var viewItem = _mapper.Map<IEnumerable<Dtos.Student>, IEnumerable<StudentViewModel>>(students);
                return Ok(viewItem);
            }
            catch (ServiceException ex)
            {
                return StatusCode((int)ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                 await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet("search/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicGroups)]
        public new async Task<ActionResult<SieveResponseModel<AcademicGroupInfoViewModel>>> GetWithMoreInfo([FromQuery]SieveModel sieveModel) => await base.GetWithMoreInfo(sieveModel);

        [HttpGet("moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicGroups)]
        public new async Task<ActionResult<IEnumerable<AcademicGroupInfoViewModel>>> GetWithMoreInfo() => await base.GetWithMoreInfo();

        [HttpGet("{page}/{size}/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.AcademicGroups)]
        public new async Task<ActionResult<IEnumerable<AcademicGroupInfoViewModel>>> GetWithMoreInfo(int page, int size) => await base.GetWithMoreInfo(page, size);
    }
}
