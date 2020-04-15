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
    public class SpecialtyService : ISpecialtyService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public SpecialtyService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task<Guid> AddAsync(Dtos.Specialty dto)
        {
            var validator = new Dtos.SpecialtyValidator();
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

            var specialty = new Entities.Specialty
            {
                Id         = id,
                CreatedAt  = now,
                UpdatedAt  = now,
                Name       = dto.Name,
                Code       = dto.Code,
                Discipline = dto.Discipline,
                GroupsCode = dto.GroupsCode
            };

            await _context.AddAsync(specialty);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.Specialty>> GetAllAsync()
        {
            return await _context.Set<Entities.Specialty>()
                                 .AsNoTracking()
                                 .Select(spec => new Dtos.Specialty
                                 {
                                     CreatedAt  = spec.CreatedAt,
                                     Id         = spec.Id,
                                     UpdatedAt  = spec.UpdatedAt,
                                     Name       = spec.Name,
                                     Code       = spec.Code,
                                     Discipline = spec.Discipline,
                                     GroupsCode = spec.GroupsCode
                                 })
                                 .OrderBy(spec => spec.Name)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.Specialty>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.Specialty>()
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(spec => new Dtos.Specialty
                                 {
                                     CreatedAt  = spec.CreatedAt,
                                     Id         = spec.Id,
                                     UpdatedAt  = spec.UpdatedAt,
                                     Name       = spec.Name,
                                     Code       = spec.Code,
                                     Discipline = spec.Discipline,
                                     GroupsCode = spec.GroupsCode
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.Specialty> GetByIdAsync(Guid id)
        {
            var specialty = await _context.FindAsync<Entities.Specialty>(id);

            if (specialty == null)
            {
                return null;
            }

            return new Dtos.Specialty
            {
                CreatedAt  = specialty.CreatedAt,
                Id         = specialty.Id,
                UpdatedAt  = specialty.UpdatedAt,
                Name       = specialty.Name,
                GroupsCode = specialty.GroupsCode,
                Discipline = specialty.Discipline,
                Code       = specialty.Code
            };
        }

        public async Task<SieveResult<Dtos.Specialty>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var specialtiesQuery = _context.Set<Entities.Specialty>().AsNoTracking();

            specialtiesQuery = _sieveProcessor.Apply(model, specialtiesQuery, applyPagination: false);

            var result = new SieveResult<Dtos.Specialty>();
            result.TotalCount = await specialtiesQuery.CountAsync();

            var someSpecialties = await _sieveProcessor.Apply(model, specialtiesQuery, applyFiltering: false, applySorting: false).ToListAsync();

            result.Result = someSpecialties.Select(specialty => new Dtos.Specialty
            {
                CreatedAt  = specialty.CreatedAt,
                Id         = specialty.Id,
                UpdatedAt  = specialty.UpdatedAt,
                Name       = specialty.Name,
                GroupsCode = specialty.GroupsCode,
                Discipline = specialty.Discipline,
                Code       = specialty.Code
            });

            return result;
        }

        public async Task<IEnumerable<Dtos.Student>> GetStudentsAsync(Guid specialtyId)
        {
            var specialty = await _context.Set<Entities.Specialty>().AnyAsync(g => g.Id == specialtyId);
            if (!specialty)
            {
                throw new NotFoundException("Specialty not found");
            }

            return await _context.Set<Entities.Specialty>()
                                 .Include(x => x.AcademicGroups)
                                 .ThenInclude(x => x.Select(g => g.Students))
                                 .Where(s => s.Id == specialtyId)
                                 .SelectMany(x => x.AcademicGroups.SelectMany(s => s.Students).Select(s => new Dtos.Student
                                 {
                                     AcademicGroupId = s.AcademicGroupId,
                                     AcceleratedFormOfEducation = s.AcceleratedFormOfEducation,
                                     AddressOfResidence = s.AddressOfResidence,
                                     Chummery = s.Chummery,
                                     CreatedAt = s.CreatedAt,
                                     EducationLevelId = s.EducationLevelId,
                                     EducationProgramId = s.EducationProgramId,
                                     EndDate = s.EndDate,
                                     EntryDate = s.EntryDate,
                                     Financing = s.Financing,
                                     ForeignLanguage = s.ForeignLanguage,
                                     FormOfEducationId = s.FormOfEducationId,
                                     Id = s.Id,
                                     MilitaryRegistration = s.MilitaryRegistration,
                                     NumberOfRecordBook = s.NumberOfRecordBook,
                                     PrivilegeId = s.PrivilegeId,
                                     Sex = s.Sex,
                                     StudentTicketNumber = s.StudentTicketNumber,
                                     UpdatedAt = s.UpdatedAt,

                                     DateOfBirth = s.UserInfo.DateOfBirth,
                                     Email = s.UserInfo.Email,
                                     FirstName = s.UserInfo.FirstName,
                                     FirstNameEng = s.UserInfo.FirstNameEng,
                                     LastName = s.UserInfo.LastName,
                                     LastNameEng = s.UserInfo.LastNameEng,
                                     Patronymic = s.UserInfo.Patronymic,
                                     PhoneNumber = s.UserInfo.PhoneNumber
                                 }))
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var specialty = await _context.Set<Entities.Specialty>()
                                          .FindAsync(id);

            if (specialty == null)
            {
                throw new NotFoundException();
            }

            _context.Remove(specialty);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.Specialty dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.SpecialtyValidator();
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

            var specialty = await _context.FindAsync<Entities.Specialty>(dto.Id);

            if (specialty == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            specialty.UpdatedAt  = DateTime.UtcNow;
            specialty.Name       = dto.Name;
            specialty.Code       = dto.Code;
            specialty.Discipline = dto.Discipline;
            specialty.GroupsCode = dto.GroupsCode;

            await _context.SaveChangesAsync();
        }
    }
}
