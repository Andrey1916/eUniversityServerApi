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
    public class CurriculumService : ICurriculumService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public CurriculumService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task<Guid> AddAsync(Dtos.Curriculum dto)
        {
            var validator = new Dtos.CurriculumValidator();
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

            if (!await _context.Set<Entities.EducationProgram>().AnyAsync(d => d.Id == dto.EducationProgramId))
            {
                throw new InvalidModelException($"Education program with id: {dto.EducationProgramId} not found");
            }

            if (!await _context.Set<Entities.FormOfEducation>().AnyAsync(d => d.Id == dto.FormOfEducationId))
            {
                throw new InvalidModelException($"Form of education with id: {dto.FormOfEducationId} not found");
            }

            if (!await _context.Set<Entities.Specialty>().AnyAsync(d => d.Id == dto.SpecialtyId))
            {
                throw new InvalidModelException($"Specialty with id: {dto.SpecialtyId} not found");
            }

            if (!await _context.Set<Entities.StructuralUnit>().AnyAsync(d => d.Id == dto.StructuralUnitId))
            {
                throw new InvalidModelException($"Structural unit with id: {dto.StructuralUnitId} not found");
            }

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var curriculum = new Entities.Curriculum
            {
                CreatedAt                                = now,
                DateOfApproval                           = dto.DateOfApproval,
                DepartmentId                             = dto.DepartmentId,
                EducationLevelId                         = dto.EducationLevelId,
                EducationProgramId                       = dto.EducationProgramId,
                FormOfEducationId                        = dto.FormOfEducationId,
                Id                                       = id,
                ListOfApprovals                          = dto.ListOfApprovals,
                OrderOfApprovals                         = dto.OrderOfApprovals,
                ProtocolOfAcademicCouncilOfUnit          = dto.ProtocolOfAcademicCouncilOfUnit,
                ProtocolOfAcademicCouncilOfUniversity    = dto.ProtocolOfAcademicCouncilOfUniversity,
                ScheduleOfEducationProcess               = dto.ScheduleOfEducationProcess,
                SpecialtyGuarantor                       = dto.SpecialtyGuarantor,
                SpecialtyId                              = dto.SpecialtyId,
                StructuralUnitId                         = dto.StructuralUnitId,
                UpdatedAt                                = now,
                YearOfAdmission                          = dto.YearOfAdmission
            };

            _context.Add(curriculum);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.Curriculum>> GetAllAsync()
        {
            return await _context.Set<Entities.Curriculum>()
                                 .Include(cur => cur.Specialty)
                                 .Include(cur => cur.EducationLevel)
                                 .Include(cur => cur.FormOfEducation)
                                 .AsNoTracking()
                                 .Select(cur => new Dtos.Curriculum
                                 {
                                     DepartmentId                          = cur.DepartmentId,
                                     EducationLevelId                      = cur.EducationLevelId,
                                     FormOfEducationId                     = cur.FormOfEducationId,
                                     Id                                    = cur.Id,
                                     UpdatedAt                             = cur.UpdatedAt,
                                     CreatedAt                             = cur.CreatedAt,
                                     DateOfApproval                        = cur.DateOfApproval,
                                     EducationProgramId                    = cur.EducationProgramId,
                                     ListOfApprovals                       = cur.ListOfApprovals,
                                     OrderOfApprovals                      = cur.OrderOfApprovals,
                                     ProtocolOfAcademicCouncilOfUnit       = cur.ProtocolOfAcademicCouncilOfUnit,
                                     ProtocolOfAcademicCouncilOfUniversity = cur.ProtocolOfAcademicCouncilOfUniversity,
                                     ScheduleOfEducationProcess            = cur.ScheduleOfEducationProcess,
                                     SpecialtyGuarantor                    = cur.SpecialtyGuarantor,
                                     SpecialtyId                           = cur.SpecialtyId,
                                     StructuralUnitId                      = cur.StructuralUnitId,
                                     YearOfAdmission                       = cur.YearOfAdmission,
                                     ShortName                             = cur.Specialty.GroupsCode + $" ({ cur.EducationLevel.Name }, {cur.FormOfEducation.Name}) " + cur.Specialty.Code + (cur.DateOfApproval.HasValue ? ' ' + cur.DateOfApproval.Value.ToString("yyyy.MM.dd") : "")
                                 })
                                 .OrderBy(cur => cur.ShortName)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.Curriculum>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.Curriculum>()
                                 .Include(cur => cur.EducationLevel)
                                 .Include(cur => cur.FormOfEducation)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(cur => new Dtos.Curriculum
                                 {
                                     DepartmentId                          = cur.DepartmentId,
                                     EducationLevelId                      = cur.EducationLevelId,
                                     FormOfEducationId                     = cur.FormOfEducationId,
                                     Id                                    = cur.Id,
                                     UpdatedAt                             = cur.UpdatedAt,
                                     CreatedAt                             = cur.CreatedAt,
                                     DateOfApproval                        = cur.DateOfApproval,
                                     EducationProgramId                    = cur.EducationProgramId,
                                     ListOfApprovals                       = cur.ListOfApprovals,
                                     OrderOfApprovals                      = cur.OrderOfApprovals,
                                     ProtocolOfAcademicCouncilOfUnit       = cur.ProtocolOfAcademicCouncilOfUnit,
                                     ProtocolOfAcademicCouncilOfUniversity = cur.ProtocolOfAcademicCouncilOfUniversity,
                                     ScheduleOfEducationProcess            = cur.ScheduleOfEducationProcess,
                                     SpecialtyGuarantor                    = cur.SpecialtyGuarantor,
                                     SpecialtyId                           = cur.SpecialtyId,
                                     StructuralUnitId                      = cur.StructuralUnitId,
                                     YearOfAdmission                       = cur.YearOfAdmission,
                                     ShortName                             = cur.Specialty.GroupsCode + $" ({ cur.EducationLevel.Name }, {cur.FormOfEducation.Name}) " + cur.Specialty.Code + (cur.DateOfApproval.HasValue ? ' ' + cur.DateOfApproval.Value.ToString("yyyy.MM.dd") : "")
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.Curriculum> GetByIdAsync(Guid id)
        {
            var curriculum = await _context.Set<Entities.Curriculum>()
                                           .Include(cur => cur.EducationLevel)
                                           .Include(cur => cur.FormOfEducation)
                                           .FirstOrDefaultAsync(cur => cur.Id == id);

            if (curriculum == null)
            {
                return null;
            }

            return new Dtos.Curriculum
            {
                DepartmentId                          = curriculum.DepartmentId,
                EducationLevelId                      = curriculum.EducationLevelId,
                FormOfEducationId                     = curriculum.FormOfEducationId,
                Id                                    = curriculum.Id,
                UpdatedAt                             = curriculum.UpdatedAt,
                CreatedAt                             = curriculum.CreatedAt,
                DateOfApproval                        = curriculum.DateOfApproval,
                EducationProgramId                    = curriculum.EducationProgramId,
                ListOfApprovals                       = curriculum.ListOfApprovals,
                OrderOfApprovals                      = curriculum.OrderOfApprovals,
                ProtocolOfAcademicCouncilOfUnit       = curriculum.ProtocolOfAcademicCouncilOfUnit,
                ProtocolOfAcademicCouncilOfUniversity = curriculum.ProtocolOfAcademicCouncilOfUniversity,
                ScheduleOfEducationProcess            = curriculum.ScheduleOfEducationProcess,
                SpecialtyGuarantor                    = curriculum.SpecialtyGuarantor,
                SpecialtyId                           = curriculum.SpecialtyId,
                StructuralUnitId                      = curriculum.StructuralUnitId,
                YearOfAdmission                       = curriculum.YearOfAdmission,
                ShortName                             = curriculum.Specialty.GroupsCode + $" ({ curriculum.EducationLevel.Name }, {curriculum.FormOfEducation.Name}) " + curriculum.Specialty.Code + (curriculum.DateOfApproval.HasValue ? ' ' + curriculum.DateOfApproval.Value.ToString("yyyy.MM.dd") : "")
            };
        }

        public async Task<SieveResult<Dtos.Curriculum>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var curriculaQuery = _context.Set<Entities.Curriculum>()
                                         .Include(cur => cur.EducationLevel)
                                         .Include(cur => cur.FormOfEducation)
                                         .AsNoTracking();

            curriculaQuery = _sieveProcessor.Apply(model, curriculaQuery, applyPagination: false);

            var result = new SieveResult<Dtos.Curriculum>();
            result.TotalCount = await curriculaQuery.CountAsync();

            var someCurricula = _sieveProcessor.Apply(model, curriculaQuery, applyFiltering: false, applySorting: false);

            result.Result = someCurricula.Select(cur => new Dtos.Curriculum
            {
                DepartmentId                          = cur.DepartmentId,
                EducationLevelId                      = cur.EducationLevelId,
                FormOfEducationId                     = cur.FormOfEducationId,
                Id                                    = cur.Id,
                UpdatedAt                             = cur.UpdatedAt,
                CreatedAt                             = cur.CreatedAt,
                DateOfApproval                        = cur.DateOfApproval,
                EducationProgramId                    = cur.EducationProgramId,
                ListOfApprovals                       = cur.ListOfApprovals,
                OrderOfApprovals                      = cur.OrderOfApprovals,
                ProtocolOfAcademicCouncilOfUnit       = cur.ProtocolOfAcademicCouncilOfUnit,
                ProtocolOfAcademicCouncilOfUniversity = cur.ProtocolOfAcademicCouncilOfUniversity,
                ScheduleOfEducationProcess            = cur.ScheduleOfEducationProcess,
                SpecialtyGuarantor                    = cur.SpecialtyGuarantor,
                SpecialtyId                           = cur.SpecialtyId,
                StructuralUnitId                      = cur.StructuralUnitId,
                YearOfAdmission                       = cur.YearOfAdmission,
                ShortName                             = cur.Specialty.GroupsCode + $" ({ cur.EducationLevel.Name }, {cur.FormOfEducation.Name}) " + cur.Specialty.Code + (cur.DateOfApproval.HasValue ? ' ' + cur.DateOfApproval.Value.ToString("yyyy.MM.dd") : "")
            });

            return result;
        }

        public async Task<IEnumerable<CurriculumInfo>> GetAllWithInfoAsync()
        {
            return await _context.Set<Entities.Curriculum>()
                                 .Include(cur => cur.EducationLevel)
                                 .Include(cur => cur.FormOfEducation)
                                 .Include(cur => cur.Department)
                                 .Include(cur => cur.Specialty)
                                 .Include(cur => cur.StructuralUnit)
                                 .AsNoTracking()
                                 .Select(cur => new Dtos.CurriculumInfo
                                 {
                                     DepartmentId                          = cur.DepartmentId,
                                     EducationLevelId                      = cur.EducationLevelId,
                                     FormOfEducationId                     = cur.FormOfEducationId,
                                     Id                                    = cur.Id,
                                     UpdatedAt                             = cur.UpdatedAt,
                                     CreatedAt                             = cur.CreatedAt,
                                     DateOfApproval                        = cur.DateOfApproval,
                                     EducationProgramId                    = cur.EducationProgramId,
                                     ListOfApprovals                       = cur.ListOfApprovals,
                                     OrderOfApprovals                      = cur.OrderOfApprovals,
                                     ProtocolOfAcademicCouncilOfUnit       = cur.ProtocolOfAcademicCouncilOfUnit,
                                     ProtocolOfAcademicCouncilOfUniversity = cur.ProtocolOfAcademicCouncilOfUniversity,
                                     ScheduleOfEducationProcess            = cur.ScheduleOfEducationProcess,
                                     SpecialtyGuarantor                    = cur.SpecialtyGuarantor,
                                     SpecialtyId                           = cur.SpecialtyId,
                                     StructuralUnitId                      = cur.StructuralUnitId,
                                     YearOfAdmission                       = cur.YearOfAdmission,
                                     ShortName                             = cur.Specialty.GroupsCode + $" ({ cur.EducationLevel.Name }, {cur.FormOfEducation.Name}) " + cur.Specialty.Code + (cur.DateOfApproval.HasValue ? ' ' + cur.DateOfApproval.Value.ToString("yyyy.MM.dd") : ""),

                                     DepartmentName                        = cur.Department.FullName,
                                     FormOfEducation                       = cur.FormOfEducation.Name,
                                     EducationLevel                        = cur.EducationLevel.Name,
                                     SpecialtyName                         = cur.Specialty.Name,
                                     StructuralUnitName                    = cur.StructuralUnit.FullName
                                 })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<CurriculumInfo>> GetAllWithInfoAsync(int page, int size)
        {
            return await _context.Set<Entities.Curriculum>()
                                 .Include(cur => cur.EducationLevel)
                                 .Include(cur => cur.FormOfEducation)
                                 .Include(cur => cur.Department)
                                 .Include(cur => cur.Specialty)
                                 .Include(cur => cur.StructuralUnit)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(cur => new Dtos.CurriculumInfo
                                 {
                                     DepartmentId                          = cur.DepartmentId,
                                     EducationLevelId                      = cur.EducationLevelId,
                                     FormOfEducationId                     = cur.FormOfEducationId,
                                     Id                                    = cur.Id,
                                     UpdatedAt                             = cur.UpdatedAt,
                                     CreatedAt                             = cur.CreatedAt,
                                     DateOfApproval                        = cur.DateOfApproval,
                                     EducationProgramId                    = cur.EducationProgramId,
                                     ListOfApprovals                       = cur.ListOfApprovals,
                                     OrderOfApprovals                      = cur.OrderOfApprovals,
                                     ProtocolOfAcademicCouncilOfUnit       = cur.ProtocolOfAcademicCouncilOfUnit,
                                     ProtocolOfAcademicCouncilOfUniversity = cur.ProtocolOfAcademicCouncilOfUniversity,
                                     ScheduleOfEducationProcess            = cur.ScheduleOfEducationProcess,
                                     SpecialtyGuarantor                    = cur.SpecialtyGuarantor,
                                     SpecialtyId                           = cur.SpecialtyId,
                                     StructuralUnitId                      = cur.StructuralUnitId,
                                     YearOfAdmission                       = cur.YearOfAdmission,
                                     ShortName                             = cur.Specialty.GroupsCode + $" ({ cur.EducationLevel.Name }, {cur.FormOfEducation.Name}) " + cur.Specialty.Code + (cur.DateOfApproval.HasValue ? ' ' + cur.DateOfApproval.Value.ToString("yyyy.MM.dd") : ""),

                                     DepartmentName                        = cur.Department.FullName,
                                     FormOfEducation                       = cur.FormOfEducation.Name,
                                     EducationLevel                        = cur.EducationLevel.Name,
                                     SpecialtyName                         = cur.Specialty.Name,
                                     StructuralUnitName                    = cur.StructuralUnit.FullName
                                 })
                                 .ToListAsync();
        }

        public async Task<SieveResult<CurriculumInfo>> GetSomeWithInfoAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var curriculaQuery = _context.Set<Entities.Curriculum>()
                                         .Include(cur => cur.EducationLevel)
                                         .Include(cur => cur.FormOfEducation)
                                         .Include(cur => cur.Department)
                                         .Include(cur => cur.Specialty)
                                         .Include(cur => cur.StructuralUnit)
                                         .AsNoTracking();

            curriculaQuery = _sieveProcessor.Apply(model, curriculaQuery, applyPagination: false);

            var result = new SieveResult<Dtos.CurriculumInfo>();
            result.TotalCount = await curriculaQuery.CountAsync();

            var someCurricula = _sieveProcessor.Apply(model, curriculaQuery, applyFiltering: false, applySorting: false);

            result.Result = someCurricula.Select(cur => new Dtos.CurriculumInfo
            {
                DepartmentId                          = cur.DepartmentId,
                EducationLevelId                      = cur.EducationLevelId,
                FormOfEducationId                     = cur.FormOfEducationId,
                Id                                    = cur.Id,
                UpdatedAt                             = cur.UpdatedAt,
                CreatedAt                             = cur.CreatedAt,
                DateOfApproval                        = cur.DateOfApproval,
                EducationProgramId                    = cur.EducationProgramId,
                ListOfApprovals                       = cur.ListOfApprovals,
                OrderOfApprovals                      = cur.OrderOfApprovals,
                ProtocolOfAcademicCouncilOfUnit       = cur.ProtocolOfAcademicCouncilOfUnit,
                ProtocolOfAcademicCouncilOfUniversity = cur.ProtocolOfAcademicCouncilOfUniversity,
                ScheduleOfEducationProcess            = cur.ScheduleOfEducationProcess,
                SpecialtyGuarantor                    = cur.SpecialtyGuarantor,
                SpecialtyId                           = cur.SpecialtyId,
                StructuralUnitId                      = cur.StructuralUnitId,
                YearOfAdmission                       = cur.YearOfAdmission,
                ShortName                             = cur.Specialty.GroupsCode + $" ({ cur.EducationLevel.Name }, {cur.FormOfEducation.Name}) " + cur.Specialty.Code + (cur.DateOfApproval.HasValue ? ' ' + cur.DateOfApproval.Value.ToString("yyyy.MM.dd") : ""),

                DepartmentName                        = cur.Department.FullName,
                FormOfEducation                       = cur.FormOfEducation.Name,
                EducationLevel                        = cur.EducationLevel.Name,
                SpecialtyName                         = cur.Specialty.Name,
                StructuralUnitName                    = cur.StructuralUnit.FullName
            });

            return result;
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var curriculum = await _context.FindAsync<Entities.Curriculum>(id);

            if (curriculum == null)
            {
                throw new NotFoundException();
            }

            _context.Remove(curriculum);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.Curriculum dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.CurriculumValidator();
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

            if (!await _context.Set<Entities.EducationProgram>().AnyAsync(d => d.Id == dto.EducationProgramId))
            {
                throw new InvalidModelException($"Education program with id: {dto.EducationProgramId} not found");
            }

            if (!await _context.Set<Entities.FormOfEducation>().AnyAsync(d => d.Id == dto.FormOfEducationId))
            {
                throw new InvalidModelException($"Form of education with id: {dto.FormOfEducationId} not found");
            }

            if (!await _context.Set<Entities.Specialty>().AnyAsync(d => d.Id == dto.SpecialtyId))
            {
                throw new InvalidModelException($"Specialty with id: {dto.SpecialtyId} not found");
            }

            if (!await _context.Set<Entities.StructuralUnit>().AnyAsync(d => d.Id == dto.StructuralUnitId))
            {
                throw new InvalidModelException($"Structural unit with id: {dto.StructuralUnitId} not found");
            }

            var curriculum = await _context.FindAsync<Dtos.Curriculum>(dto.Id);

            if (curriculum == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            curriculum.ListOfApprovals                       = dto.ListOfApprovals;
            curriculum.DateOfApproval                        = dto.DateOfApproval;
            curriculum.DepartmentId                          = dto.DepartmentId;
            curriculum.EducationLevelId                      = dto.EducationLevelId;
            curriculum.EducationProgramId                    = dto.EducationProgramId;
            curriculum.FormOfEducationId                     = dto.FormOfEducationId;
            curriculum.OrderOfApprovals                      = dto.OrderOfApprovals;
            curriculum.ProtocolOfAcademicCouncilOfUnit       = dto.ProtocolOfAcademicCouncilOfUnit;
            curriculum.ProtocolOfAcademicCouncilOfUniversity = dto.ProtocolOfAcademicCouncilOfUniversity;
            curriculum.ScheduleOfEducationProcess            = dto.ScheduleOfEducationProcess;
            curriculum.SpecialtyGuarantor                    = dto.SpecialtyGuarantor;
            curriculum.SpecialtyId                           = dto.SpecialtyId;
            curriculum.StructuralUnitId                      = dto.StructuralUnitId;
            curriculum.UpdatedAt                             = DateTime.UtcNow;
            curriculum.YearOfAdmission                       = dto.YearOfAdmission;

            await _context.SaveChangesAsync();
        }
    }
}
