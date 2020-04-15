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
    public class DepartmentService : IDepartmentService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public DepartmentService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task<Guid> AddAsync(Dtos.Department dto)
        {
            var validator = new Dtos.DepartmentValidator();
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

            if (dto.StructuralUnitId != null && !await _context.Set<Entities.StructuralUnit>().AnyAsync(d => d.Id == dto.StructuralUnitId))
            {
                throw new InvalidModelException($"Structural unit with id: {dto.StructuralUnitId} not found");
            }

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var department = new Entities.Department
            {
                Code             = dto.Code,
                Id               = id,
                CreatedAt        = now,
                UpdatedAt        = now,
                Chief            = dto.Chief,
                StructuralUnitId = dto.StructuralUnitId,
                FullName         = dto.FullName,
                FullNameEng      = dto.FullNameEng,
                ShortName        = dto.ShortName
            };

            await _context.AddAsync(department);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.Department>> GetAllAsync()
        {
            return await _context.Set<Entities.Department>()
                                 .AsNoTracking()
                                 .Select(d => new Dtos.Department
                                 {
                                     CreatedAt        = d.CreatedAt,
                                     StructuralUnitId = d.StructuralUnitId,
                                     Code             = d.Code,
                                     Id               = d.Id,
                                     UpdatedAt        = d.UpdatedAt,
                                     ShortName        = d.ShortName,
                                     FullNameEng      = d.FullNameEng,
                                     FullName         = d.FullName,
                                     Chief            = d.Chief
                                 })
                                 .OrderBy(d => d.FullName)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.Department>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.Department>()
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(d => new Dtos.Department
                                 {
                                     CreatedAt        = d.CreatedAt,
                                     StructuralUnitId = d.StructuralUnitId,
                                     Code             = d.Code,
                                     Id               = d.Id,
                                     UpdatedAt        = d.UpdatedAt,
                                     ShortName        = d.ShortName,
                                     FullNameEng      = d.FullNameEng,
                                     FullName         = d.FullName,
                                     Chief            = d.Chief
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.Department> GetByIdAsync(Guid id)
        {
            var department = await _context.FindAsync<Entities.Department>(id);

            if (department == null)
            {
                return null;
            }

            return new Dtos.Department
            {
                CreatedAt        = department.CreatedAt,
                StructuralUnitId = department.StructuralUnitId,
                Code             = department.Code,
                Id               = department.Id,
                UpdatedAt        = department.UpdatedAt,
                ShortName        = department.ShortName,
                FullNameEng      = department.FullNameEng,
                FullName         = department.FullName,
                Chief            = department.Chief
            };
        }

        public async Task<SieveResult<Dtos.Department>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var departmentsQuery = _context.Set<Entities.Department>().AsNoTracking();

            departmentsQuery = _sieveProcessor.Apply(model, departmentsQuery, applyPagination: false);

            var result = new SieveResult<Dtos.Department>();
            result.TotalCount = await departmentsQuery.CountAsync();

            var someDepartments = await _sieveProcessor.Apply(model, departmentsQuery, applyFiltering: false, applySorting: false).ToListAsync();

            result.Result = someDepartments.Select(d => new Dtos.Department
            {
                CreatedAt        = d.CreatedAt,
                StructuralUnitId = d.StructuralUnitId,
                Code             = d.Code,
                Id               = d.Id,
                UpdatedAt        = d.UpdatedAt,
                ShortName        = d.ShortName,
                FullNameEng      = d.FullNameEng,
                FullName         = d.FullName,
                Chief            = d.Chief
            });

            return result;
        }

        public async Task<IEnumerable<DepartmentInfo>> GetAllWithInfoAsync()
        {
            return await _context.Set<Entities.Department>()
                                 .Include(d => d.StructuralUnit)
                                 .AsNoTracking()
                                 .Select(d => new Dtos.DepartmentInfo
                                 {
                                     CreatedAt          = d.CreatedAt,
                                     StructuralUnitId   = d.StructuralUnitId,
                                     Code               = d.Code,
                                     Id                 = d.Id,
                                     UpdatedAt          = d.UpdatedAt,
                                     ShortName          = d.ShortName,
                                     FullNameEng        = d.FullNameEng,
                                     FullName           = d.FullName,
                                     Chief              = d.Chief,

                                     StructuralUnitName = d.StructuralUnit.FullName
                                 })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<DepartmentInfo>> GetAllWithInfoAsync(int page, int size)
        {
            return await _context.Set<Entities.Department>()
                                 .Include(d => d.StructuralUnit)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(d => new Dtos.DepartmentInfo
                                 {
                                     CreatedAt          = d.CreatedAt,
                                     StructuralUnitId   = d.StructuralUnitId,
                                     Code               = d.Code,
                                     Id                 = d.Id,
                                     UpdatedAt          = d.UpdatedAt,
                                     ShortName          = d.ShortName,
                                     FullNameEng        = d.FullNameEng,
                                     FullName           = d.FullName,
                                     Chief              = d.Chief,

                                     StructuralUnitName = d.StructuralUnit.FullName
                                 })
                                 .ToListAsync();
        }

        public async Task<SieveResult<DepartmentInfo>> GetSomeWithInfoAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var departmentsQuery = _context.Set<Entities.Department>()
                                           .Include(d => d.StructuralUnit)
                                           .AsNoTracking();

            departmentsQuery = _sieveProcessor.Apply(model, departmentsQuery, applyPagination: false);

            var result = new SieveResult<Dtos.DepartmentInfo>();
            result.TotalCount = await departmentsQuery.CountAsync();

            var someDepartments = _sieveProcessor.Apply(model, departmentsQuery, applyFiltering: false, applySorting: false);

            result.Result = someDepartments.Select(d => new Dtos.DepartmentInfo
            {
                CreatedAt          = d.CreatedAt,
                StructuralUnitId   = d.StructuralUnitId,
                Code               = d.Code,
                Id                 = d.Id,
                UpdatedAt          = d.UpdatedAt,
                ShortName          = d.ShortName,
                FullNameEng        = d.FullNameEng,
                FullName           = d.FullName,
                Chief              = d.Chief,

                StructuralUnitName = d.StructuralUnit.FullName
            });

            return result;
        }


        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var department = await _context.Set<Entities.Department>()
                                           .FindAsync(id);

            if (department == null)
            {
                throw new NotFoundException();
            }

            _context.Remove(department);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.Department dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.DepartmentValidator();
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

            if (!await _context.Set<Entities.StructuralUnit>().AnyAsync(d => d.Id == dto.StructuralUnitId))
            {
                throw new InvalidModelException($"Structural unit with id: {dto.StructuralUnitId} not found");
            }

            var department = await _context.FindAsync<Entities.Department>(dto.Id);

            if (department == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            department.Chief            = dto.Chief;
            department.Code             = dto.Code;
            department.FullName         = dto.FullName;
            department.FullNameEng      = dto.FullNameEng;
            department.ShortName        = dto.ShortName;
            department.StructuralUnitId = dto.StructuralUnitId;
            department.UpdatedAt        = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
