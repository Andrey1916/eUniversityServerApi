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

namespace eUniversityServer.Services
{
    public class FormOfEducationService : IFormOfEducationService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public FormOfEducationService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task<Guid> AddAsync(Dtos.FormOfEducation dto)
        {
            var validator = new Dtos.FormOfEducationValidator();
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

            var formOfEducation = new Entities.FormOfEducation
            {
                Id           = id,
                CreatedAt    = now,
                UpdatedAt    = now,
                Name         = dto.Name
            };

            await _context.AddAsync(formOfEducation);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.FormOfEducation>> GetAllAsync()
        {
            return await _context.Set<Entities.FormOfEducation>()
                                 .AsNoTracking()
                                 .Select(foe => new Dtos.FormOfEducation
                                 {
                                     CreatedAt = foe.CreatedAt,
                                     Id        = foe.Id,
                                     UpdatedAt = foe.UpdatedAt,
                                     Name      = foe.Name
                                 })
                                 .OrderBy(foe => foe.Name)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.FormOfEducation>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.FormOfEducation>()
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(foe => new Dtos.FormOfEducation
                                 {
                                     CreatedAt = foe.CreatedAt,
                                     Id        = foe.Id,
                                     UpdatedAt = foe.UpdatedAt,
                                     Name      = foe.Name
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.FormOfEducation> GetByIdAsync(Guid id)
        {
            var formOfEducation = await _context.FindAsync<Entities.FormOfEducation>(id);

            if (formOfEducation == null)
            {
                return null;
            }

            return new Dtos.FormOfEducation
            {
                CreatedAt = formOfEducation.CreatedAt,
                Id        = formOfEducation.Id,
                UpdatedAt = formOfEducation.UpdatedAt,
                Name      = formOfEducation.Name
            };
        }

        public async Task<SieveResult<Dtos.FormOfEducation>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var formOfEducationsQuery = _context.Set<Entities.FormOfEducation>().AsNoTracking();

            formOfEducationsQuery = _sieveProcessor.Apply(model, formOfEducationsQuery, applyPagination: false);

            var result = new SieveResult<Dtos.FormOfEducation>();
            result.TotalCount = await formOfEducationsQuery.CountAsync();

            var someFormOfEducations = await _sieveProcessor.Apply(model, formOfEducationsQuery, applyFiltering: false, applySorting: false).ToListAsync();

            result.Result = someFormOfEducations.Select(foe => new Dtos.FormOfEducation
            {
                CreatedAt = foe.CreatedAt,
                Id        = foe.Id,
                UpdatedAt = foe.UpdatedAt,
                Name      = foe.Name
            });

            return result;
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var formOfEducation = await _context.Set<Entities.FormOfEducation>()
                                                .FindAsync(id);

            if (formOfEducation == null)
            {
                throw new NotFoundException();
            }

            _context.Remove(formOfEducation);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.FormOfEducation dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.FormOfEducationValidator();
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

            var formOfEducation = await _context.FindAsync<Entities.FormOfEducation>(dto.Id);

            if (formOfEducation == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            formOfEducation.UpdatedAt = DateTime.UtcNow;
            formOfEducation.Name = dto.Name;

            await _context.SaveChangesAsync();
        }
    }
}
