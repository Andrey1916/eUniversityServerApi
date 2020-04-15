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
    public class AcademicGroupService : IAcademicGroupService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public AcademicGroupService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context        = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task<Guid> AddAsync(Dtos.AcademicGroup dto)
        {
            var validator = new Dtos.AcademicGroupValidator();
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

            if (!await _context.Set<Entities.Department>().AnyAsync(d => d.Id == dto.DepartmentId))
            {
                throw new InvalidModelException($"Department with id: {dto.DepartmentId} not found");
            }

            if (!await _context.Set<Entities.EducationLevel>().AnyAsync(d => d.Id == dto.EducationLevelId))
            {
                throw new InvalidModelException($"Education level with id: {dto.EducationLevelId} not found");
            }

            if (!await _context.Set<Entities.FormOfEducation>().AnyAsync(d => d.Id == dto.FormOfEducationId))
            {
                throw new InvalidModelException($"Form of education with id: {dto.FormOfEducationId} not found");
            }
            
            if (!await _context.Set<Entities.StructuralUnit>().AnyAsync(d => d.Id == dto.StructuralUnitId))
            {
                throw new InvalidModelException($"Structural unit with id: {dto.StructuralUnitId} not found");
            }

            var specialty = await _context.FindAsync<Entities.Specialty>(dto.SpecialtyId);

            if (specialty == null)
            {
                throw new InvalidModelException($"Specialty with id: {dto.SpecialtyId} not found");
            }

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;;

            string code = $"{dto.Grade}{dto.Number:00}-{specialty.GroupsCode}";

            var academicGroup = new Entities.AcademicGroup
            {
                Captain           = dto.Captain,
                Code              = code,
                Curator           = dto.Curator,
                DepartmentId      = dto.DepartmentId,
                Id                = id,
                EducationLevelId  = dto.EducationLevelId,
                FormOfEducationId = dto.FormOfEducationId,
                Grade             = dto.Grade,
                Number            = dto.Number,
                StructuralUnitId  = dto.StructuralUnitId,
                SpecialtyId       = dto.SpecialtyId,
                UIN               = dto.UIN,
                CreatedAt         = now,
                UpdatedAt         = now
            };

            await _context.AddAsync(academicGroup);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.AcademicGroup>> GetAllAsync()
        {
            return await _context.Set<Entities.AcademicGroup>()
                                 .AsNoTracking()
                                 .Select(grp => new Dtos.AcademicGroup
                                 {
                                     Captain              = grp.Captain,
                                     CreatedAt            = grp.CreatedAt,
                                     UIN                  = grp.UIN,
                                     SpecialtyId          = grp.SpecialtyId,
                                     StructuralUnitId     = grp.StructuralUnitId,
                                     Number               = grp.Number,
                                     Grade                = grp.Grade,
                                     Code                 = grp.Code,
                                     Curator              = grp.Curator,
                                     DepartmentId         = grp.DepartmentId,
                                     EducationLevelId     = grp.EducationLevelId,
                                     FormOfEducationId    = grp.FormOfEducationId,
                                     Id                   = grp.Id,
                                     UpdatedAt            = grp.UpdatedAt
                                 })
                                 .OrderBy(group => group.Code)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.AcademicGroup>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.AcademicGroup>()
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(grp => new Dtos.AcademicGroup
                                 {
                                     Captain              = grp.Captain,
                                     CreatedAt            = grp.CreatedAt,
                                     UIN                  = grp.UIN,
                                     SpecialtyId          = grp.SpecialtyId,
                                     StructuralUnitId     = grp.StructuralUnitId,
                                     Number               = grp.Number,
                                     Grade                = grp.Grade,
                                     Code                 = grp.Code,
                                     Curator              = grp.Curator,
                                     DepartmentId         = grp.DepartmentId,
                                     EducationLevelId     = grp.EducationLevelId,
                                     FormOfEducationId    = grp.FormOfEducationId,
                                     Id                   = grp.Id,
                                     UpdatedAt            = grp.UpdatedAt
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.AcademicGroup> GetByIdAsync(Guid id)
        {
            var academicGroup = await _context.FindAsync<Entities.AcademicGroup>(id);

            if (academicGroup == null)
            {
                return null;
            }

            return new Dtos.AcademicGroup
            {
                Captain           = academicGroup.Captain,
                CreatedAt         = academicGroup.CreatedAt,
                UIN               = academicGroup.UIN,
                SpecialtyId       = academicGroup.SpecialtyId,
                StructuralUnitId  = academicGroup.StructuralUnitId,
                Number            = academicGroup.Number,
                Grade             = academicGroup.Grade,
                Code              = academicGroup.Code,
                Curator           = academicGroup.Curator,
                DepartmentId      = academicGroup.DepartmentId,
                EducationLevelId  = academicGroup.EducationLevelId,
                FormOfEducationId = academicGroup.FormOfEducationId,
                Id                = academicGroup.Id,
                UpdatedAt         = academicGroup.UpdatedAt
            };
        }

        public async Task<SieveResult<Dtos.AcademicGroup>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var groupsQuery = _context.Set<Entities.AcademicGroup>()
                                      .AsNoTracking();

            groupsQuery = _sieveProcessor.Apply(model, groupsQuery, applyPagination: false);

            var result = new SieveResult<Dtos.AcademicGroup>();
            result.TotalCount = await groupsQuery.CountAsync();

            var someGroups = await _sieveProcessor.Apply(model, groupsQuery, applyFiltering: false, applySorting: false).ToListAsync();

            result.Result = someGroups.Select(group => new Dtos.AcademicGroup
            {
                Captain           = group.Captain,
                CreatedAt         = group.CreatedAt,
                UIN               = group.UIN,
                SpecialtyId       = group.SpecialtyId,
                StructuralUnitId  = group.StructuralUnitId,
                Number            = group.Number,
                Grade             = group.Grade,
                Code              = group.Code,
                Curator           = group.Curator,
                DepartmentId      = group.DepartmentId,
                EducationLevelId  = group.EducationLevelId,
                FormOfEducationId = group.FormOfEducationId,
                Id                = group.Id,
                UpdatedAt         = group.UpdatedAt
            });

            return result;
        }

        public async Task<IEnumerable<AcademicGroupInfo>> GetAllWithInfoAsync()
        {
            return await _context.Set<Entities.AcademicGroup>()
                                 .Include(grp => grp.Department)
                                 .Include(grp => grp.EducationLevel)
                                 .Include(grp => grp.FormOfEducation)
                                 .Include(grp => grp.Specialty)
                                 .Include(grp => grp.StructuralUnit)
                                 .AsNoTracking()
                                 .Select(grp => new Dtos.AcademicGroupInfo
                                 {
                                     Captain            = grp.Captain,
                                     CreatedAt          = grp.CreatedAt,
                                     UIN                = grp.UIN,
                                     SpecialtyId        = grp.SpecialtyId,
                                     StructuralUnitId   = grp.StructuralUnitId,
                                     Number             = grp.Number,
                                     Grade              = grp.Grade,
                                     Code               = grp.Code,
                                     Curator            = grp.Curator,
                                     DepartmentId       = grp.DepartmentId,
                                     EducationLevelId   = grp.EducationLevelId,
                                     FormOfEducationId  = grp.FormOfEducationId,
                                     Id                 = grp.Id,
                                     UpdatedAt          = grp.UpdatedAt,
                                                        
                                     DepartmentName     = grp.Department.FullName,
                                     EducationLevel     = grp.EducationLevel.Name,
                                     FormOfEducation    = grp.FormOfEducation.Name,
                                     SpecialtyName      = grp.Specialty.Name,
                                     StructuralUnitName = grp.StructuralUnit.FullName
                                 })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<AcademicGroupInfo>> GetAllWithInfoAsync(int page, int size)
        {
            return await _context.Set<Entities.AcademicGroup>()
                                 .Include(grp => grp.Department)
                                 .Include(grp => grp.EducationLevel)
                                 .Include(grp => grp.FormOfEducation)
                                 .Include(grp => grp.Specialty)
                                 .Include(grp => grp.StructuralUnit)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(grp => new Dtos.AcademicGroupInfo
                                 {
                                     Captain            = grp.Captain,
                                     CreatedAt          = grp.CreatedAt,
                                     UIN                = grp.UIN,
                                     SpecialtyId        = grp.SpecialtyId,
                                     StructuralUnitId   = grp.StructuralUnitId,
                                     Number             = grp.Number,
                                     Grade              = grp.Grade,
                                     Code               = grp.Code,
                                     Curator            = grp.Curator,
                                     DepartmentId       = grp.DepartmentId,
                                     EducationLevelId   = grp.EducationLevelId,
                                     FormOfEducationId  = grp.FormOfEducationId,
                                     Id                 = grp.Id,
                                     UpdatedAt          = grp.UpdatedAt,
                                                        
                                     DepartmentName     = grp.Department.FullName,
                                     EducationLevel     = grp.EducationLevel.Name,
                                     FormOfEducation    = grp.FormOfEducation.Name,
                                     SpecialtyName      = grp.Specialty.Name,
                                     StructuralUnitName = grp.StructuralUnit.FullName
                                 })
                                 .ToListAsync();
        }

        public async Task<SieveResult<AcademicGroupInfo>> GetSomeWithInfoAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var groupsQuery = _context.Set<Entities.AcademicGroup>()
                                      .Include(grp => grp.Department)
                                      .Include(grp => grp.EducationLevel)
                                      .Include(grp => grp.FormOfEducation)
                                      .Include(grp => grp.Specialty)
                                      .Include(grp => grp.StructuralUnit)
                                      .AsNoTracking();

            groupsQuery = _sieveProcessor.Apply(model, groupsQuery, applyPagination: false);

            var result = new SieveResult<Dtos.AcademicGroupInfo>();
            result.TotalCount = await groupsQuery.CountAsync();

            var someGroups = _sieveProcessor.Apply(model, groupsQuery, applyFiltering: false, applySorting: false);

            result.Result = someGroups.Select(group => new Dtos.AcademicGroupInfo
            {
                Captain            = group.Captain,
                CreatedAt          = group.CreatedAt,
                UIN                = group.UIN,
                SpecialtyId        = group.SpecialtyId,
                StructuralUnitId   = group.StructuralUnitId,
                Number             = group.Number,
                Grade              = group.Grade,
                Code               = group.Code,
                Curator            = group.Curator,
                DepartmentId       = group.DepartmentId,
                EducationLevelId   = group.EducationLevelId,
                FormOfEducationId  = group.FormOfEducationId,
                Id                 = group.Id,
                UpdatedAt          = group.UpdatedAt,

                DepartmentName     = group.Department.FullName,
                EducationLevel     = group.EducationLevel.Name,
                FormOfEducation    = group.FormOfEducation.Name,
                SpecialtyName      = group.Specialty.Name,
                StructuralUnitName = group.StructuralUnit.FullName
            });

            return result;
        }

        public async Task<IEnumerable<Dtos.Student>> GetStudentsAsync(Guid groupId)
        {
            var group = await _context.Set<Entities.AcademicGroup>().AnyAsync(g => g.Id == groupId);

            if (!group)
            {
                throw new NotFoundException("Academic group not fount");
            }

            return await _context.Set<Entities.Student>()
                                 .Include(x => x.UserInfo)
                                 .Include(x => x.AcademicGroup)
                                 .AsNoTracking()
                                 .Where(s => s.AcademicGroupId == groupId)
                                 .Select(s => new Dtos.Student {
                                     AcademicGroupId            = s.AcademicGroupId,
                                     AcceleratedFormOfEducation = s.AcceleratedFormOfEducation,
                                     AddressOfResidence         = s.AddressOfResidence,
                                     Chummery                   = s.Chummery,
                                     CreatedAt                  = s.CreatedAt,
                                     EducationLevelId           = s.EducationLevelId,
                                     EducationProgramId         = s.EducationProgramId,
                                     EndDate                    = s.EndDate,
                                     EntryDate                  = s.EntryDate,
                                     Financing                  = s.Financing,
                                     ForeignLanguage            = s.ForeignLanguage,
                                     FormOfEducationId          = s.FormOfEducationId,
                                     Id                         = s.Id,
                                     MilitaryRegistration       = s.MilitaryRegistration,
                                     NumberOfRecordBook         = s.NumberOfRecordBook,
                                     PrivilegeId                = s.PrivilegeId,
                                     Sex                        = s.Sex,
                                     StudentTicketNumber        = s.StudentTicketNumber,
                                     UpdatedAt                  = s.UpdatedAt,

                                     DateOfBirth                = s.UserInfo.DateOfBirth,
                                     Email                      = s.UserInfo.Email,
                                     FirstName                  = s.UserInfo.FirstName,
                                     FirstNameEng               = s.UserInfo.FirstNameEng,
                                     LastName                   = s.UserInfo.LastName,
                                     LastNameEng                = s.UserInfo.LastNameEng,
                                     Patronymic                 = s.UserInfo.Patronymic,
                                     PhoneNumber                = s.UserInfo.PhoneNumber
                                 })
                                 .ToListAsync();
            }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var academicGroup = await _context.Set<Entities.AcademicGroup>().FindAsync(id);

            if (academicGroup == null)
            {
                throw new NotFoundException();
            }

            _context.Remove(academicGroup);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.AcademicGroup dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.AcademicGroupValidator();
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

            if (!await _context.Set<Entities.Department>().AnyAsync(d => d.Id == dto.DepartmentId))
            {
                throw new InvalidModelException($"Department with id: {dto.DepartmentId} not found");
            }

            if (!await _context.Set<Entities.EducationLevel>().AnyAsync(d => d.Id == dto.EducationLevelId))
            {
                throw new InvalidModelException($"Education level with id: {dto.EducationLevelId} not found");
            }

            if (!await _context.Set<Entities.FormOfEducation>().AnyAsync(d => d.Id == dto.FormOfEducationId))
            {
                throw new InvalidModelException($"Form of education with id: {dto.FormOfEducationId} not found");
            }

            if (!await _context.Set<Entities.StructuralUnit>().AnyAsync(d => d.Id == dto.StructuralUnitId))
            {
                throw new InvalidModelException($"Structural unit with id: {dto.StructuralUnitId} not found");
            }

            var specialty = await _context.FindAsync<Entities.Specialty>(dto.SpecialtyId);

            if (specialty == null)
            {
                throw new InvalidModelException($"Specialty with id: {dto.SpecialtyId} not found");
            }

            string code = $"{dto.Grade}{dto.Number:00}-{specialty.GroupsCode}";

            var group = await _context.FindAsync<Entities.AcademicGroup>(dto.Id);

            if (group == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            group.Captain           = dto.Captain;
            group.Code              = code;
            group.Curator           = dto.Curator;
            group.DepartmentId      = dto.DepartmentId;
            group.EducationLevelId  = dto.EducationLevelId;
            group.FormOfEducationId = dto.FormOfEducationId;
            group.Grade             = dto.Grade;
            group.Number            = dto.Number;
            group.SpecialtyId       = dto.SpecialtyId;
            group.StructuralUnitId  = dto.StructuralUnitId;
            group.UIN               = dto.UIN;
            group.UpdatedAt         = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
