using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Exceptions;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using Entities = eUniversityServer.DAL.Entities;
using eUniversityServer.Services.Models;

namespace eUniversityServer.Services
{
    public class EducationLevelService : IEducationLevelService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public EducationLevelService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task<Guid> AddAsync(Dtos.EducationLevel dto)
        {
            var validator = new Dtos.EducationLevelValidator();
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

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var educationLevel = new Entities.EducationLevel
            {
                Id        = id,
                CreatedAt = now,
                UpdatedAt = now,
                Name      = dto.Name,
                Number    = dto.Number
            };

            await _context.AddAsync(educationLevel);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.EducationLevel>> GetAllAsync()
        {
            return await _context.Set<Entities.EducationLevel>()
                                 .AsNoTracking()
                                 .Select(el => new Dtos.EducationLevel
                                 {
                                     CreatedAt = el.CreatedAt,
                                     Number    = el.Number,
                                     Name      = el.Name,
                                     Id        = el.Id,
                                     UpdatedAt = el.UpdatedAt
                                 })
                                 .OrderBy(el => el.Name)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.EducationLevel>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.EducationLevel>()
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(el => new Dtos.EducationLevel
                                 {
                                     CreatedAt = el.CreatedAt,
                                     Number    = el.Number,
                                     Name      = el.Name,
                                     Id        = el.Id,
                                     UpdatedAt = el.UpdatedAt
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.EducationLevel> GetByIdAsync(Guid id)
        {
            var educationLevel = await _context.FindAsync<Entities.EducationLevel>(id);

            if (educationLevel == null)
            {
                return null;
            }

            return new Dtos.EducationLevel
            {
                CreatedAt = educationLevel.CreatedAt,
                Number    = educationLevel.Number,
                Name      = educationLevel.Name,
                Id        = educationLevel.Id,
                UpdatedAt = educationLevel.UpdatedAt
            };
        }

        public async Task<SieveResult<Dtos.EducationLevel>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var educationLevelsQuery = _context.Set<Entities.EducationLevel>().AsNoTracking();

            educationLevelsQuery = _sieveProcessor.Apply(model, educationLevelsQuery, applyPagination: false);

            var result = new SieveResult<Dtos.EducationLevel>();
            result.TotalCount = await educationLevelsQuery.CountAsync();

            var someEducationLevels = await _sieveProcessor.Apply(model, educationLevelsQuery, applyFiltering: false, applySorting: false).ToListAsync();

            result.Result = someEducationLevels.Select(el => new Dtos.EducationLevel
            {
                CreatedAt = el.CreatedAt,
                Number    = el.Number,
                Name      = el.Name,
                Id        = el.Id,
                UpdatedAt = el.UpdatedAt
            });

            return result;
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var educationLevel = await _context.Set<Entities.EducationLevel>()
                                               .FindAsync(id);

            if (educationLevel == null)
            {
                throw new NotFoundException();
            }

            _context.Remove(educationLevel);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.EducationLevel dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.EducationLevelValidator();
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

            var educationLevel = await _context.FindAsync<Entities.EducationLevel>(dto.Id);

            if (educationLevel == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            educationLevel.Name      = dto.Name;
            educationLevel.Number    = dto.Number;
            educationLevel.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

    }
}
