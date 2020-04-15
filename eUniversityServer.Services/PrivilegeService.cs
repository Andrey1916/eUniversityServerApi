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
    public class PrivilegeService : IPrivilegeService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public PrivilegeService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task<Guid> AddAsync(Dtos.Privilege dto)
        {
            var validator = new Dtos.PrivilegeValidator();
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

            var privilege = new Entities.Privilege
            {
                Id        = id,
                CreatedAt = now,
                UpdatedAt = now,
                Name      = dto.Name,
            };

            await _context.AddAsync(privilege);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.Privilege>> GetAllAsync()
        {
            return await _context.Set<Entities.Privilege>()
                                 .AsNoTracking()
                                 .Select(priv => new Dtos.Privilege
                                 {
                                     CreatedAt = priv.CreatedAt,
                                     Id        = priv.Id,
                                     UpdatedAt = priv.UpdatedAt,
                                     Name      = priv.Name
                                 })
                                 .OrderBy(priv => priv.Name)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.Privilege>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.Privilege>()
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(priv => new Dtos.Privilege
                                 {
                                     CreatedAt = priv.CreatedAt,
                                     Id        = priv.Id,
                                     UpdatedAt = priv.UpdatedAt,
                                     Name      = priv.Name
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.Privilege> GetByIdAsync(Guid id)
        {
            var privilege = await _context.FindAsync<Entities.Privilege>(id);

            if (privilege == null)
            {
                return null;
            }

            return new Dtos.Privilege
            {
                CreatedAt = privilege.CreatedAt,
                Id        = privilege.Id,
                UpdatedAt = privilege.UpdatedAt,
                Name      = privilege.Name
            };
        }

        public async Task<SieveResult<Dtos.Privilege>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var privilegesQuery = _context.Set<Entities.Privilege>().AsNoTracking();

            privilegesQuery = _sieveProcessor.Apply(model, privilegesQuery, applyPagination: false);

            var result = new SieveResult<Dtos.Privilege>();
            result.TotalCount = await privilegesQuery.CountAsync();

            var somePrivileges = await _sieveProcessor.Apply(model, privilegesQuery, applyFiltering: false, applySorting: false).ToListAsync();

            result.Result = somePrivileges.Select(privilege => new Dtos.Privilege
            {
                CreatedAt = privilege.CreatedAt,
                Id        = privilege.Id,
                UpdatedAt = privilege.UpdatedAt,
                Name      = privilege.Name
            });

            return result;
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var privilege = await _context.Set<Entities.Privilege>()
                                          .FindAsync(id);

            if (privilege == null)
            {
                throw new NotFoundException();
            }

            _context.Remove(privilege);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.Privilege dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.PrivilegeValidator();
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

            var privilege = await _context.Set<Entities.Privilege>()
                                          .FindAsync(dto.Id);

            if (privilege == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            privilege.UpdatedAt = DateTime.UtcNow;
            privilege.Name      = dto.Name;

            await _context.SaveChangesAsync();
        }
    }
}
