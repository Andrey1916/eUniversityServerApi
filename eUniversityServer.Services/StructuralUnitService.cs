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
    public class StructuralUnitService : IStructuralUnitService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public StructuralUnitService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task<Guid> AddAsync(Dtos.StructuralUnit dto)
        {
            var validator = new Dtos.StructuralUnitValidator();
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

            var structuralUnit = new Entities.StructuralUnit
            {
                Id          = id,
                CreatedAt   = now,
                UpdatedAt   = now,
                Chief       = dto.Chief,
                Code        = dto.Code,
                FullName    = dto.FullName,
                FullNameEng = dto.FullNameEng,
                ShortName   = dto.ShortName
            };

            await _context.AddAsync(structuralUnit);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.StructuralUnit>> GetAllAsync()
        {
            return await _context.Set<Entities.StructuralUnit>()
                                 .AsNoTracking()
                                 .Select(su => new Dtos.StructuralUnit
                                 {
                                     CreatedAt   = su.CreatedAt,
                                     Id          = su.Id,
                                     UpdatedAt   = su.UpdatedAt,
                                     Chief       = su.Chief,
                                     ShortName   = su.ShortName,
                                     FullNameEng = su.FullNameEng,
                                     FullName    = su.FullName,
                                     Code        = su.Code
                                 })
                                 .OrderBy(su => su.FullName)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.StructuralUnit>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.StructuralUnit>()
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(su => new Dtos.StructuralUnit
                                 {
                                     CreatedAt   = su.CreatedAt,
                                     Id          = su.Id,
                                     UpdatedAt   = su.UpdatedAt,
                                     Chief       = su.Chief,
                                     ShortName   = su.ShortName,
                                     FullNameEng = su.FullNameEng,
                                     FullName    = su.FullName,
                                     Code        = su.Code
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.StructuralUnit> GetByIdAsync(Guid id)
        {
            var structuralUnit = await _context.FindAsync<Entities.StructuralUnit>(id);

            if (structuralUnit == null)
            {
                return null;
            }

            return new Dtos.StructuralUnit
            {
                CreatedAt   = structuralUnit.CreatedAt,
                Id          = structuralUnit.Id,
                UpdatedAt   = structuralUnit.UpdatedAt,
                Code        = structuralUnit.Code,
                FullName    = structuralUnit.FullName,
                FullNameEng = structuralUnit.FullNameEng,
                ShortName   = structuralUnit.ShortName,
                Chief       = structuralUnit.Chief
            };
        }

        public async Task<SieveResult<Dtos.StructuralUnit>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var structuralUnitsQuery = _context.Set<Entities.StructuralUnit>().AsNoTracking();

            structuralUnitsQuery = _sieveProcessor.Apply(model, structuralUnitsQuery, applyPagination: false);

            var result = new SieveResult<Dtos.StructuralUnit>();
            result.TotalCount = await structuralUnitsQuery.CountAsync();

            var someStructuralUnits = await _sieveProcessor.Apply(model, structuralUnitsQuery, applyFiltering: false, applySorting: false).ToListAsync();

            result.Result = someStructuralUnits.Select(structuralUnit => new Dtos.StructuralUnit
            {
                CreatedAt   = structuralUnit.CreatedAt,
                Id          = structuralUnit.Id,
                UpdatedAt   = structuralUnit.UpdatedAt,
                Code        = structuralUnit.Code,
                FullName    = structuralUnit.FullName,
                FullNameEng = structuralUnit.FullNameEng,
                ShortName   = structuralUnit.ShortName,
                Chief       = structuralUnit.Chief
            });

            return result;
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var structuralUnit = await _context.Set<Entities.StructuralUnit>()
                                               .FindAsync(id);

            if (structuralUnit == null)
            {
                throw new NotFoundException();
            }

            _context.Remove(structuralUnit);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.StructuralUnit dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.StructuralUnitValidator();
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

            var structuralUnit = await _context.FindAsync<Entities.StructuralUnit>(dto.Id);

            if (structuralUnit == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            structuralUnit.UpdatedAt   = DateTime.UtcNow;
            structuralUnit.Chief       = dto.Chief;
            structuralUnit.Code        = dto.Code;
            structuralUnit.FullName    = dto.FullName;
            structuralUnit.FullNameEng = dto.FullNameEng;
            structuralUnit.ShortName   = dto.ShortName;

            await _context.SaveChangesAsync();
        }
    }
}
