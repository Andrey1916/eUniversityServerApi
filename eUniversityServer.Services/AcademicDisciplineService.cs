using eUniversityServer.Services.Dtos;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities = eUniversityServer.DAL.Entities;

namespace eUniversityServer.Services
{
    public class AcademicDisciplineService : IAcademicDisciplineService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public AcademicDisciplineService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context        = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        /// <exception cref="InvalidModelException"/>
        public async Task<Guid> AddAsync(Dtos.AcademicDiscipline dto)
        {
            var validator = new Dtos.AcademicDisciplineValidator();
            ValidationResult result = validator.Validate(dto);

            if (!result.IsValid)
            {
                string errMess = string.Empty;

                foreach (var failure in result.Errors)
                {
                    errMess += $"Property { failure.PropertyName } failed validation. Error was: { failure.ErrorMessage }\n";
                }

                throw new InvalidModelException(errMess);
            }

            if (dto.AssistantId != null && !await _context.Set<Entities.Teacher>().AnyAsync(d => d.Id == dto.AssistantId))
            {
                throw new InvalidModelException($"Teacher with id: {dto.AssistantId} not found");
            }

            if (!await _context.Set<Entities.Curriculum>().AnyAsync(d => d.Id == dto.CurriculumId))
            {
                throw new InvalidModelException($"Curriculum with id: {dto.CurriculumId} not found");
            }

            if (!await _context.Set<Entities.Department>().AnyAsync(d => d.Id == dto.DepartmentId))
            {
                throw new InvalidModelException($"Department with id: {dto.DepartmentId} not found");
            }

            if (!await _context.Set<Entities.Teacher>().AnyAsync(d => d.Id == dto.LecturerId))
            {
                throw new InvalidModelException($"Teacher with id: {dto.LecturerId} not found");
            }

            if (!await _context.Set<Entities.Specialty>().AnyAsync(d => d.Id == dto.SpecialtyId))
            {
                throw new InvalidModelException($"Specialty with id: {dto.SpecialtyId} not found");
            }

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var academicDiscipline = new Entities.AcademicDiscipline
            {
                AssistantId          = dto.AssistantId,
                Attestation          = dto.Attestation,
                CreatedAt            = now,
                CurriculumId         = dto.CurriculumId,
                DepartmentId         = dto.DepartmentId,
                FullName             = dto.FullName,
                Id                   = id,
                LecturerId           = dto.LecturerId,
                Semester             = dto.Semester,
                NumberOfCredits      = dto.NumberOfCredits,
                ShortName            = dto.ShortName,
                SpecialtyId          = dto.SpecialtyId,
                TypeOfIndividualWork = dto.TypeOfIndividualWork,
                UpdatedAt            = now
            };

            await _context.AddAsync(academicDiscipline);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.AcademicDiscipline>> GetAllAsync()
        {
            return await _context.Set<Entities.AcademicDiscipline>()
                                 .AsNoTracking()
                                 .Select(disc => new Dtos.AcademicDiscipline
                                 {
                                     AssistantId          = disc.AssistantId,
                                     Attestation          = disc.Attestation,
                                     CreatedAt            = disc.CreatedAt,
                                     CurriculumId         = disc.CurriculumId,
                                     DepartmentId         = disc.DepartmentId,
                                     FullName             = disc.FullName,
                                     Id                   = disc.Id,
                                     LecturerId           = disc.LecturerId,
                                     Semester             = disc.Semester,
                                     NumberOfCredits      = disc.NumberOfCredits,
                                     ShortName            = disc.ShortName,
                                     SpecialtyId          = disc.SpecialtyId,
                                     TypeOfIndividualWork = disc.TypeOfIndividualWork,
                                     UpdatedAt            = disc.UpdatedAt
                                 })
                                 .OrderBy(dist => dist.FullName)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.AcademicDiscipline>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.AcademicDiscipline>()
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(disc => new Dtos.AcademicDiscipline
                                 {
                                     AssistantId          = disc.AssistantId,
                                     Attestation          = disc.Attestation,
                                     CreatedAt            = disc.CreatedAt,
                                     CurriculumId         = disc.CurriculumId,
                                     DepartmentId         = disc.DepartmentId,
                                     FullName             = disc.FullName,
                                     Id                   = disc.Id,
                                     LecturerId           = disc.LecturerId,
                                     Semester             = disc.Semester,
                                     NumberOfCredits      = disc.NumberOfCredits,
                                     ShortName            = disc.ShortName,
                                     SpecialtyId          = disc.SpecialtyId,
                                     TypeOfIndividualWork = disc.TypeOfIndividualWork,
                                     UpdatedAt            = disc.UpdatedAt
                                 })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<AcademicDisciplineInfo>> GetAllWithInfoAsync()
        {
            return await _context.Set<Entities.AcademicDiscipline>()
                                 .Include(ad => ad.Department)
                                 .Include(ad => ad.Specialty)
                                 .Include(ad => ad.Assistant)
                                 .ThenInclude(ass => ass.UserInfo)
                                 .Include(ad => ad.Lecturer)
                                 .ThenInclude(lec => lec.UserInfo)
                                 .AsNoTracking()
                                 .Select(disc => new Dtos.AcademicDisciplineInfo
                                 {
                                     AssistantId          = disc.AssistantId,
                                     Attestation          = disc.Attestation,
                                     CreatedAt            = disc.CreatedAt,
                                     CurriculumId         = disc.CurriculumId,
                                     DepartmentId         = disc.DepartmentId,
                                     FullName             = disc.FullName,
                                     Id                   = disc.Id,
                                     LecturerId           = disc.LecturerId,
                                     Semester             = disc.Semester,
                                     NumberOfCredits      = disc.NumberOfCredits,
                                     ShortName            = disc.ShortName,
                                     SpecialtyId          = disc.SpecialtyId,
                                     TypeOfIndividualWork = disc.TypeOfIndividualWork,
                                     UpdatedAt            = disc.UpdatedAt,

                                     DepartmentName       = disc.Department.FullName,
                                     SpecialtyName        = disc.Specialty.Name,
                                     AssistantName        = disc.Assistant.UserInfo.LastName + ' ' + disc.Assistant.UserInfo.FirstName + ' ' + disc.Assistant.UserInfo.Patronymic,
                                     LecturerName         = disc.Lecturer.UserInfo.LastName + ' ' + disc.Lecturer.UserInfo.FirstName + ' ' + disc.Lecturer.UserInfo.Patronymic
                                 })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<AcademicDisciplineInfo>> GetAllWithInfoAsync(int page, int size)
        {
            return await _context.Set<Entities.AcademicDiscipline>()
                                 .Include(ad => ad.Department)
                                 .Include(ad => ad.Specialty)
                                 .Include(ad => ad.Assistant)
                                 .ThenInclude(ass => ass.UserInfo)
                                 .Include(ad => ad.Lecturer)
                                 .ThenInclude(lec => lec.UserInfo)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(disc => new Dtos.AcademicDisciplineInfo
                                 {
                                     AssistantId          = disc.AssistantId,
                                     Attestation          = disc.Attestation,
                                     CreatedAt            = disc.CreatedAt,
                                     CurriculumId         = disc.CurriculumId,
                                     DepartmentId         = disc.DepartmentId,
                                     FullName             = disc.FullName,
                                     Id                   = disc.Id,
                                     LecturerId           = disc.LecturerId,
                                     Semester             = disc.Semester,
                                     NumberOfCredits      = disc.NumberOfCredits,
                                     ShortName            = disc.ShortName,
                                     SpecialtyId          = disc.SpecialtyId,
                                     TypeOfIndividualWork = disc.TypeOfIndividualWork,
                                     UpdatedAt            = disc.UpdatedAt,

                                     DepartmentName       = disc.Department.FullName,
                                     SpecialtyName        = disc.Specialty.Name,
                                     AssistantName        = disc.Assistant.UserInfo.LastName + ' ' + disc.Assistant.UserInfo.FirstName + ' ' + disc.Assistant.UserInfo.Patronymic,
                                     LecturerName         = disc.Lecturer.UserInfo.LastName + ' ' + disc.Lecturer.UserInfo.FirstName + ' ' + disc.Lecturer.UserInfo.Patronymic
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.AcademicDiscipline> GetByIdAsync(Guid id)
        {
            var academicDiscipline = await _context.FindAsync<Entities.AcademicDiscipline>(id);

            if (academicDiscipline == null)
            {
                return null;
            }

            var dto = new Dtos.AcademicDiscipline
            {
                AssistantId          = academicDiscipline.AssistantId,
                Attestation          = academicDiscipline.Attestation,
                CreatedAt            = academicDiscipline.CreatedAt,
                CurriculumId         = academicDiscipline.CurriculumId,
                DepartmentId         = academicDiscipline.DepartmentId,
                FullName             = academicDiscipline.FullName,
                Id                   = academicDiscipline.Id,
                LecturerId           = academicDiscipline.LecturerId,
                NumberOfCredits      = academicDiscipline.NumberOfCredits,
                Semester             = academicDiscipline.Semester,
                ShortName            = academicDiscipline.ShortName,
                SpecialtyId          = academicDiscipline.SpecialtyId,
                TypeOfIndividualWork = academicDiscipline.TypeOfIndividualWork,
                UpdatedAt            = academicDiscipline.UpdatedAt
            };

            return dto;
        }

        public async Task<SieveResult<Dtos.AcademicDiscipline>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var disciplinesQuery = _context.Set<Entities.AcademicDiscipline>()
                                           .AsNoTracking();

            disciplinesQuery = _sieveProcessor.Apply(model, disciplinesQuery, applyPagination: false);

            var result = new SieveResult<Dtos.AcademicDiscipline>();
            result.TotalCount = await disciplinesQuery.CountAsync();

            var someDisciplines = await _sieveProcessor.Apply(model, disciplinesQuery, applyFiltering: false, applySorting: false).ToListAsync();

            result.Result = someDisciplines.Select(discipline => new Dtos.AcademicDiscipline
            {
                AssistantId          = discipline.AssistantId,
                Attestation          = discipline.Attestation,
                CreatedAt            = discipline.CreatedAt,
                CurriculumId         = discipline.CurriculumId,
                DepartmentId         = discipline.DepartmentId,
                FullName             = discipline.FullName,
                Id                   = discipline.Id,
                LecturerId           = discipline.LecturerId,
                Semester             = discipline.Semester,
                NumberOfCredits      = discipline.NumberOfCredits,
                ShortName            = discipline.ShortName,
                SpecialtyId          = discipline.SpecialtyId,
                TypeOfIndividualWork = discipline.TypeOfIndividualWork,
                UpdatedAt            = discipline.UpdatedAt
            });

            return result;
        }

        public async Task<SieveResult<AcademicDisciplineInfo>> GetSomeWithInfoAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var disciplinesQuery = _context.Set<Entities.AcademicDiscipline>()
                                           .Include(ad => ad.Department)
                                           .Include(ad => ad.Specialty)
                                           .Include(ad => ad.Assistant)
                                           .ThenInclude(ass => ass.UserInfo)
                                           .Include(ad => ad.Lecturer)
                                           .ThenInclude(lec => lec.UserInfo)
                                           .AsNoTracking();

            disciplinesQuery = _sieveProcessor.Apply(model, disciplinesQuery, applyPagination: false);

            var result = new SieveResult<Dtos.AcademicDisciplineInfo>();
            result.TotalCount = await disciplinesQuery.CountAsync();

            var someDisciplines = _sieveProcessor.Apply(model, disciplinesQuery, applyFiltering: false, applySorting: false);

            result.Result = someDisciplines.Select(discipline => new Dtos.AcademicDisciplineInfo
            {
                AssistantId          = discipline.AssistantId,
                Attestation          = discipline.Attestation,
                CreatedAt            = discipline.CreatedAt,
                CurriculumId         = discipline.CurriculumId,
                DepartmentId         = discipline.DepartmentId,
                FullName             = discipline.FullName,
                Id                   = discipline.Id,
                LecturerId           = discipline.LecturerId,
                Semester             = discipline.Semester,
                NumberOfCredits      = discipline.NumberOfCredits,
                ShortName            = discipline.ShortName,
                SpecialtyId          = discipline.SpecialtyId,
                TypeOfIndividualWork = discipline.TypeOfIndividualWork,
                UpdatedAt            = discipline.UpdatedAt,

                DepartmentName       = discipline.Department.FullName,
                SpecialtyName        = discipline.Specialty.Name,
                AssistantName        = discipline.Assistant.UserInfo.LastName + ' ' + discipline.Assistant.UserInfo.FirstName + ' ' + discipline.Assistant.UserInfo.Patronymic,
                LecturerName         = discipline.Lecturer.UserInfo.LastName + ' ' + discipline.Lecturer.UserInfo.FirstName + ' ' + discipline.Lecturer.UserInfo.Patronymic
            });

            return result;
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var academicDiscipline = await _context.Set<Entities.AcademicDiscipline>()
                                                   .FindAsync(id);

            if (academicDiscipline == null)
            {
                throw new NotFoundException();
            }

            _context.Remove(academicDiscipline);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.AcademicDiscipline dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.AcademicDisciplineValidator();
            ValidationResult result = validator.Validate(dto);

            if (!result.IsValid)
            {
                string errMess = string.Empty;

                foreach (var failure in result.Errors)
                {
                    errMess += $"Property { failure.PropertyName } failed validation. Error was: { failure.ErrorMessage }\n";
                }

                throw new InvalidModelException(errMess);
            }

            if (!await _context.Set<Entities.Teacher>().AnyAsync(d => d.Id == dto.AssistantId))
            {
                throw new InvalidModelException($"Teacher with id: {dto.AssistantId} not found");
            }

            if (!await _context.Set<Entities.Curriculum>().AnyAsync(d => d.Id == dto.CurriculumId))
            {
                throw new InvalidModelException($"Curriculum with id: {dto.CurriculumId} not found");
            }

            if (!await _context.Set<Entities.Department>().AnyAsync(d => d.Id == dto.DepartmentId))
            {
                throw new InvalidModelException($"Department with id: {dto.DepartmentId} not found");
            }

            if (!await _context.Set<Entities.Teacher>().AnyAsync(d => d.Id == dto.LecturerId))
            {
                throw new InvalidModelException($"Teacher with id: {dto.LecturerId} not found");
            }

            if (!await _context.Set<Entities.Specialty>().AnyAsync(d => d.Id == dto.SpecialtyId))
            {
                throw new InvalidModelException($"Specialty with id: {dto.SpecialtyId} not found");
            }

            var discipline = await _context.FindAsync<Entities.AcademicDiscipline>(dto.Id);

            if (discipline == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            discipline.AssistantId              = dto.AssistantId;
            discipline.Attestation              = dto.Attestation;
            discipline.CurriculumId             = dto.CurriculumId;
            discipline.DepartmentId             = dto.DepartmentId;
            discipline.FullName                 = dto.FullName;
            discipline.LecturerId               = dto.LecturerId;
            discipline.NumberOfCredits          = dto.NumberOfCredits;
            discipline.Semester                 = dto.Semester;
            discipline.ShortName                = dto.ShortName;
            discipline.SpecialtyId              = dto.SpecialtyId;
            discipline.TypeOfIndividualWork     = dto.TypeOfIndividualWork;
            discipline.UpdatedAt                = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}