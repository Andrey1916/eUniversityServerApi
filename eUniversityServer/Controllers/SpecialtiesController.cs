using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Logger;
using eUniversityServer.Utils.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dtos = eUniversityServer.Services.Dtos;
using SpecialtyDto = eUniversityServer.Services.Dtos.Specialty;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/specialties")]
    public class SpecialtiesController : SieveControllerBase<SpecialtyDto, CreateSpecialtyBindingModel, UpdateSpecialtyBindingModel, SpecialtyViewModel>,
                                         ISieveController<SpecialtyDto, CreateSpecialtyBindingModel, UpdateSpecialtyBindingModel, SpecialtyViewModel>
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtiesController(IMapper mapper, ISpecialtyService specialtyService, Logger logger) : base(mapper, specialtyService, logger)
        {
            _specialtyService = specialtyService ?? throw new NullReferenceException(nameof(specialtyService));
        }


        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Specialties)]
        public new Task<ActionResult<IEnumerable<SpecialtyViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Specialties)]
        public new Task<ActionResult<SpecialtyViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Specialties)]
        public new Task<ActionResult<IEnumerable<SpecialtyViewModel>>> Get(int page, int size) => base.Get(page, size);

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Specialties)]
        public new async Task<ActionResult<SieveResponseModel<SpecialtyViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.Specialties)]
        public new Task<ActionResult<Guid>> Post([FromBody] CreateSpecialtyBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.Specialties)]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdateSpecialtyBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.Specialties)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);
        
        [HttpGet("{id}/students")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Students)]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Specialties)]
        public async Task<ActionResult<IEnumerable<StudentViewModel>>> GetStudents(Guid specialtyId)
        {
            if (specialtyId.Equals(Guid.Empty))
                return BadRequest("Id is empty");

            try
            {
                var students = await _specialtyService.GetStudentsAsync(specialtyId);
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
    }
}