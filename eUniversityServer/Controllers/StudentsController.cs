using System;
using System.Threading.Tasks;
using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentDto = eUniversityServer.Services.Dtos.Student;
using PassportDto = eUniversityServer.Services.Dtos.Passport;
using IdentificationCodeDto = eUniversityServer.Services.Dtos.IdentificationCode;
using EducationDocumentDto = eUniversityServer.Services.Dtos.EducationDocument;
using Dtos = eUniversityServer.Services.Dtos;
using eUniversityServer.Services.Logger;
using eUniversityServer.Services.Exceptions;
using System.Collections.Generic;
using Sieve.Models;
using eUniversityServer.Utils.Auth;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/students")]
    public class StudentsController : SieveInfoControllerBase<StudentDto, Dtos.StudentInfo, CreateStudentBindingModel, UpdateStudentBindingModel, StudentViewModel, StudentInfoViewModel>,
                                      ISieveInfoController<StudentDto, Dtos.StudentInfo,  CreateStudentBindingModel, UpdateStudentBindingModel, StudentViewModel, StudentInfoViewModel>
    {
        private new IStudentService _service { get; set; }

        public StudentsController(IMapper mapper, IStudentService studentsService, Logger logger) : base(mapper, studentsService, logger)
        {
            this._service = studentsService ?? throw new NullReferenceException(nameof(studentsService));
        }


        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Students)]
        public new Task<ActionResult<IEnumerable<StudentViewModel>>> Get() => base.Get();

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Students)]
        public new Task<ActionResult<StudentViewModel>> Get(Guid id) => base.Get(id);

        [HttpGet("{page}/{size}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Students)]
        public new Task<ActionResult<IEnumerable<StudentViewModel>>> Get(int page, int size) => base.Get(page, size);

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Students)]
        public new async Task<ActionResult<SieveResponseModel<StudentViewModel>>> Get([FromQuery]SieveModel sieveModel) => await base.Get(sieveModel);

        [HttpPost]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanCreate, DAL.Enums.TargetModifier.Students)]
        public new Task<ActionResult<Guid>> Post([FromBody] CreateStudentBindingModel model) => base.Post(model);

        [HttpPut("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.Students)]
        public new Task<ActionResult> Put(Guid id, [FromBody] UpdateStudentBindingModel model) => base.Put(id, model);

        [HttpDelete("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.Students)]
        public new async Task<ActionResult> Delete(Guid id) => await base.Delete(id);

        [HttpGet("search/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Students)]
        public new async Task<ActionResult<SieveResponseModel<StudentInfoViewModel>>> GetWithMoreInfo([FromQuery]SieveModel sieveModel) => await base.GetWithMoreInfo(sieveModel);

        [HttpGet("moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Students)]
        public new async Task<ActionResult<IEnumerable<StudentInfoViewModel>>> GetWithMoreInfo() => await base.GetWithMoreInfo();

        [HttpGet("{page}/{size}/moreinfo")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Students)]
        public new async Task<ActionResult<IEnumerable<StudentInfoViewModel>>> GetWithMoreInfo(int page, int size) => await base.GetWithMoreInfo(page, size);

        [HttpGet("specialty/{specialtyId}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Students)]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Specialties)]
        public async Task<ActionResult<StudentInfoViewModel>> GetStudentBySpecialty(Guid specialtyId)
        {
            if (specialtyId.Equals(Guid.Empty))
            {
                return BadRequest("Id is empty");
            }

            try
            {
                var students = await _service.GetStudentBySpecialty(specialtyId);
                if (students == null)
                    return NotFound();

                var viewItem = _mapper.Map<IEnumerable<Dtos.Student>, IEnumerable<StudentInfoViewModel>>(students);
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

        #region Passport
        [HttpGet("{studentId}/passport")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Students)]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.StudentDocuments)]
        public async Task<ActionResult<PassportViewModel>> GetPassport(Guid studentId)
        {
            if (studentId.Equals(Guid.Empty))
                return BadRequest("Student id is equals 0");

            try
            {
                var studentPassportDto = await ((IStudentService)_service).GetPassportAsync(studentId);
                if (studentPassportDto == null)
                    return NotFound();

                var passport = _mapper.Map<PassportDto, PassportViewModel>(studentPassportDto);
                return Ok(passport);
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
        
        [HttpPost("{studentId}/passport")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.Students)]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.StudentDocuments)]
        public async Task<ActionResult> SetPassport(Guid studentId, [FromBody] PassportBindingModel model)
        {
            if (studentId.Equals(Guid.Empty))
                return BadRequest("Student id is equals 0");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var passport = _mapper.Map<PassportBindingModel, PassportDto>(model);
                await ((IStudentService)_service).SetPassportAsync(studentId, passport);

                return Ok();
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
        
        [HttpDelete("{studentId}/passport")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.Students)]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.StudentDocuments)]
        public async Task<ActionResult> DeletePassport(Guid studentId)
        {
            if (studentId.Equals(Guid.Empty))
                return BadRequest("Student id is equals 0");

            try
            {
                await ((IStudentService)_service).RemovePassportAsync(studentId);
                return Ok();
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
        #endregion
            
        #region Identification code
        [HttpGet("{studentId}/identification-code")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Students)]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.StudentDocuments)]
        public async Task<ActionResult<IdentificationCodeViewModel>> GetIdentificationCode(Guid studentId)
        {
            if (studentId.Equals(Guid.Empty))
                return BadRequest("Student id is equals 0");

            try
            {
                var studentIdentificationCodeDto = await ((IStudentService)_service).GetIdentificationCodeAsync(studentId);
                if (studentIdentificationCodeDto == null)
                    return NotFound();

                var identificationCode = _mapper.Map<IdentificationCodeDto, IdentificationCodeViewModel>(studentIdentificationCodeDto);
                return Ok(identificationCode);
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
        
        [HttpPost("{studentId}/identification-code")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.Students)]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.StudentDocuments)]
        public async Task<ActionResult> SetIdentificationCode(Guid studentId, [FromBody] IdentificationCodeBindingModel model)
        {
            if (studentId.Equals(Guid.Empty))
                return BadRequest("Student id is equals 0");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var identificationCode = _mapper.Map<IdentificationCodeBindingModel, IdentificationCodeDto>(model);
                await ((IStudentService)_service).SetIdentificationCodeAsync(studentId, identificationCode);

                return Ok();
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
        
        [HttpDelete("{studentId}/identification-code")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.Students)]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.StudentDocuments)]
        public async Task<ActionResult> DeleteIdentificationCode(Guid studentId)
        {
            if (studentId.Equals(Guid.Empty))
                return BadRequest("Student id is equals 0");

            try
            {
                await ((IStudentService)_service).RemoveIdentificationCodeAsync(studentId);
                return Ok();
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
        #endregion

        #region Education document
        [HttpGet("{studentId}/education-document")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Students)]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.StudentDocuments)]
        public async Task<ActionResult<EducationDocumentViewModel>> GetEducationDocument(Guid studentId)
        {
            if (studentId.Equals(Guid.Empty))
                return BadRequest("Student id is equals 0");

            try
            {
                var studentEducationDocumentDto = await ((IStudentService)_service).GetEducationDocumentAsync(studentId);
                if (studentEducationDocumentDto == null)
                    return NotFound();

                var educationDocument = _mapper.Map<EducationDocumentDto, EducationDocumentViewModel>(studentEducationDocumentDto);
                return Ok(educationDocument);
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

        [HttpPost("{studentId}/education-document")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.Students)]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanUpdate, DAL.Enums.TargetModifier.StudentDocuments)]
        public async Task<ActionResult> SetEducationDocument(Guid studentId, [FromBody] EducationDocumentBindingModel model)
        {
            if (studentId.Equals(Guid.Empty))
                return BadRequest("Student id is equals 0");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var educationDocument = _mapper.Map<EducationDocumentBindingModel, EducationDocumentDto>(model);
                await ((IStudentService)_service).SetEducationDocumentAsync(studentId, educationDocument);

                return Ok();
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

        [HttpDelete("{studentId}/education-document")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.Students)]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanDelete, DAL.Enums.TargetModifier.StudentDocuments)]
        public async Task<ActionResult> DeleteEducationDocument(Guid studentId)
        {
            if (studentId.Equals(Guid.Empty))
                return BadRequest("Student id is equals 0");

            try
            {
                await ((IStudentService)_service).RemoveEducationDocumentAsync(studentId);
                return Ok();
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
        #endregion
        
    }
}
