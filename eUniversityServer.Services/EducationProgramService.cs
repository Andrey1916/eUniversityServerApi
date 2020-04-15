using eUniversityServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Entities = eUniversityServer.DAL.Entities;
using Sieve.Services;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Models;
using eUniversityServer.Services.Dtos;

namespace eUniversityServer.Services
{
    public class EducationProgramService : IEducationProgramService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public EducationProgramService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task<Guid> AddAsync(Dtos.EducationProgram dto)
        {
            var validator = new Dtos.EducationProgramValidator();
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

            if (!await _context.Set<Entities.Specialty>().AnyAsync(d => d.Id == dto.SpecialtyId))
            {
                throw new InvalidModelException($"Specialty with id: {dto.SpecialtyId} not found");
            }

            if (!await _context.Set<Entities.EducationLevel>().AnyAsync(d => d.Id == dto.EducationLevelId))
            {
                throw new InvalidModelException($"Education level with id: {dto.EducationLevelId} not found");
            }

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var educationProgram = new Entities.EducationProgram
            {
                Id                   = id,
                CreatedAt            = now,
                UpdatedAt            = now,
                ApprovalYear         = dto.ApprovalYear,
                DurationbOfEducation = dto.DurationOfEducation,
                EducationLevelId     = dto.EducationLevelId,
                Guarantor            = dto.Guarantor,
                Language             = dto.Language,
                SpecialtyId          = dto.SpecialtyId
            };

            await _context.AddAsync(educationProgram);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.EducationProgram>> GetAllAsync()
        {
            return await _context.Set<Entities.EducationProgram>()
                                 .Include(ep => ep.Specialty)
                                 .Include(ep => ep.EducationLevel)
                                 .AsNoTracking()
                                 .Select(ep => new Dtos.EducationProgram
                                 {
                                     CreatedAt           = ep.CreatedAt,
                                     Id                  = ep.Id,
                                     UpdatedAt           = ep.UpdatedAt,
                                     SpecialtyId         = ep.SpecialtyId,
                                     Language            = ep.Language,
                                     Guarantor           = ep.Guarantor,
                                     EducationLevelId    = ep.EducationLevelId,
                                     DurationOfEducation = ep.DurationbOfEducation,
                                     ApprovalYear        = ep.ApprovalYear,
                                     ShortName           = $"{ ep.Specialty.GroupsCode } ({ ep.EducationLevel.Name }) { ep.DurationbOfEducation }" + (ep.ApprovalYear.HasValue ? ep.ApprovalYear.Value.ToString("yyyy.MM.dd") : "")
                                 })
                                 .OrderBy(ep => ep.ShortName)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.EducationProgram>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.EducationProgram>()
                                 .Include(ep => ep.Specialty)
                                 .Include(ep => ep.EducationLevel)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(ep => new Dtos.EducationProgram
                                 {
                                     CreatedAt           = ep.CreatedAt,
                                     Id                  = ep.Id,
                                     UpdatedAt           = ep.UpdatedAt,
                                     SpecialtyId         = ep.SpecialtyId,
                                     Language            = ep.Language,
                                     Guarantor           = ep.Guarantor,
                                     EducationLevelId    = ep.EducationLevelId,
                                     DurationOfEducation = ep.DurationbOfEducation,
                                     ApprovalYear        = ep.ApprovalYear,
                                     ShortName           = $"{ ep.Specialty.GroupsCode } ({ ep.EducationLevel.Name }) { ep.DurationbOfEducation }" + (ep.ApprovalYear.HasValue ? ep.ApprovalYear.Value.ToString("yyyy.MM.dd") : "")
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.EducationProgram> GetByIdAsync(Guid id)
        {
            var educationProgram = await _context.Set<Entities.EducationProgram>()
                                                 .Include(ep => ep.Specialty)
                                                 .Include(ep => ep.EducationLevel)
                                                 .FirstOrDefaultAsync(ep => ep.Id == id);

            if (educationProgram == null)
            {
                return null;
            }

            return new Dtos.EducationProgram
            {
                CreatedAt           = educationProgram.CreatedAt,
                Id                  = educationProgram.Id,
                UpdatedAt           = educationProgram.UpdatedAt,
                SpecialtyId         = educationProgram.SpecialtyId,
                Language            = educationProgram.Language,
                Guarantor           = educationProgram.Guarantor,
                EducationLevelId    = educationProgram.EducationLevelId,
                DurationOfEducation = educationProgram.DurationbOfEducation,
                ApprovalYear        = educationProgram.ApprovalYear,
                ShortName           = $"{ educationProgram.Specialty.GroupsCode } ({ educationProgram.EducationLevel.Name }) { educationProgram.DurationbOfEducation }" + (educationProgram.ApprovalYear.HasValue ? educationProgram.ApprovalYear.Value.ToString("yyyy.MM.dd") : "")
            };
        }

        public async Task<SieveResult<Dtos.EducationProgram>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var educationProgramsQuery = _context.Set<Entities.EducationProgram>()
                                                 .Include(ep => ep.Specialty)
                                                 .Include(ep => ep.EducationLevel)
                                                 .AsNoTracking();

            educationProgramsQuery = _sieveProcessor.Apply(model, educationProgramsQuery, applyPagination: false);

            var result = new SieveResult<Dtos.EducationProgram>();
            result.TotalCount = await educationProgramsQuery.CountAsync();

            var someEducationPrograms = _sieveProcessor.Apply(model, educationProgramsQuery, applyFiltering: false, applySorting: false);

            result.Result = someEducationPrograms.Select(ep => new Dtos.EducationProgram
            {
                CreatedAt           = ep.CreatedAt,
                Id                  = ep.Id,
                UpdatedAt           = ep.UpdatedAt,
                SpecialtyId         = ep.SpecialtyId,
                Language            = ep.Language,
                Guarantor           = ep.Guarantor,
                EducationLevelId    = ep.EducationLevelId,
                DurationOfEducation = ep.DurationbOfEducation,
                ApprovalYear        = ep.ApprovalYear,
                ShortName           = $"{ ep.Specialty.GroupsCode } ({ ep.EducationLevel.Name }) { ep.DurationbOfEducation }" + (ep.ApprovalYear.HasValue ? ep.ApprovalYear.Value.ToString("yyyy.MM.dd") : "")
            });

            return result;
        }

        public async Task<IEnumerable<EducationProgramInfo>> GetAllWithInfoAsync()
        {
            return await _context.Set<Entities.EducationProgram>()
                                 .Include(ep => ep.Specialty)
                                 .Include(ep => ep.EducationLevel)
                                 .AsNoTracking()
                                 .Select(ep => new Dtos.EducationProgramInfo
                                 {
                                     CreatedAt           = ep.CreatedAt,
                                     Id                  = ep.Id,
                                     UpdatedAt           = ep.UpdatedAt,
                                     SpecialtyId         = ep.SpecialtyId,
                                     Language            = ep.Language,
                                     Guarantor           = ep.Guarantor,
                                     EducationLevelId    = ep.EducationLevelId,
                                     DurationOfEducation = ep.DurationbOfEducation,
                                     ApprovalYear        = ep.ApprovalYear,
                                     ShortName           = $"{ ep.Specialty.GroupsCode } ({ ep.EducationLevel.Name }) { ep.DurationbOfEducation }" + (ep.ApprovalYear.HasValue ? ep.ApprovalYear.Value.ToString(" yyyy.MM.dd") : ""),

                                     EducationLevel      = ep.EducationLevel.Name,
                                     SpecialtyName       = ep.Specialty.Name
                                 })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<EducationProgramInfo>> GetAllWithInfoAsync(int page, int size)
        {
            return await _context.Set<Entities.EducationProgram>()
                                 .Include(ep => ep.Specialty)
                                 .Include(ep => ep.EducationLevel)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(ep => new Dtos.EducationProgramInfo
                                 {
                                     CreatedAt           = ep.CreatedAt,
                                     Id                  = ep.Id,
                                     UpdatedAt           = ep.UpdatedAt,
                                     SpecialtyId         = ep.SpecialtyId,
                                     Language            = ep.Language,
                                     Guarantor           = ep.Guarantor,
                                     EducationLevelId    = ep.EducationLevelId,
                                     DurationOfEducation = ep.DurationbOfEducation,
                                     ApprovalYear        = ep.ApprovalYear,
                                     ShortName           = $"{ ep.Specialty.GroupsCode } ({ ep.EducationLevel.Name }) { ep.DurationbOfEducation }" + (ep.ApprovalYear.HasValue ? ep.ApprovalYear.Value.ToString("yyyy.MM.dd") : ""),

                                     EducationLevel      = ep.EducationLevel.Name,
                                     SpecialtyName       = ep.Specialty.Name
                                 })
                                 .ToListAsync();
        }

        public async Task<SieveResult<EducationProgramInfo>> GetSomeWithInfoAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var educationProgramsQuery = _context.Set<Entities.EducationProgram>()
                                                 .Include(ep => ep.Specialty)
                                                 .Include(ep => ep.EducationLevel)
                                                 .AsNoTracking();

            educationProgramsQuery = _sieveProcessor.Apply(model, educationProgramsQuery, applyPagination: false);

            var result = new SieveResult<Dtos.EducationProgramInfo>();
            result.TotalCount = await educationProgramsQuery.CountAsync();

            var someEducationPrograms = _sieveProcessor.Apply(model, educationProgramsQuery, applyFiltering: false, applySorting: false);

            result.Result = someEducationPrograms.Select(ep => new Dtos.EducationProgramInfo
            {
                CreatedAt           = ep.CreatedAt,
                Id                  = ep.Id,
                UpdatedAt           = ep.UpdatedAt,
                SpecialtyId         = ep.SpecialtyId,
                Language            = ep.Language,
                Guarantor           = ep.Guarantor,
                EducationLevelId    = ep.EducationLevelId,
                DurationOfEducation = ep.DurationbOfEducation,
                ApprovalYear        = ep.ApprovalYear,
                ShortName           = $"{ ep.Specialty.GroupsCode } ({ ep.EducationLevel.Name }) { ep.DurationbOfEducation }" + (ep.ApprovalYear.HasValue ? ep.ApprovalYear.Value.ToString("yyyy.MM.dd") : ""),

                EducationLevel      = ep.EducationLevel.Name,
                SpecialtyName       = ep.Specialty.Name
            });

            return result;
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var educationProgram = await _context.Set<Entities.EducationProgram>()
                                                 .FindAsync(id);

            if (educationProgram == null)
            {
                throw new NotFoundException();
            }

            _context.Remove(educationProgram);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.EducationProgram dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.EducationProgramValidator();
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

            if (!await _context.Set<Entities.Specialty>().AnyAsync(d => d.Id == dto.SpecialtyId))
            {
                throw new InvalidModelException($"Specialty with id: {dto.SpecialtyId} not found");
            }

            if (!await _context.Set<Entities.EducationLevel>().AnyAsync(d => d.Id == dto.EducationLevelId))
            {
                throw new InvalidModelException($"Education level with id: {dto.EducationLevelId} not found");
            }

            var educationProgram = await _context.FindAsync<Entities.EducationProgram>(dto.Id);

            if (educationProgram == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            educationProgram.UpdatedAt            = DateTime.UtcNow;
            educationProgram.ApprovalYear         = dto.ApprovalYear;
            educationProgram.DurationbOfEducation = dto.DurationOfEducation;
            educationProgram.EducationLevelId     = dto.EducationLevelId;
            educationProgram.Guarantor            = dto.Guarantor;
            educationProgram.Language             = dto.Language;
            educationProgram.SpecialtyId          = dto.SpecialtyId;

            await _context.SaveChangesAsync();
        }
    }
}
