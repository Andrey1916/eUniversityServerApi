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
    public class ExamsGradesSpreadsheetService : IExamsGradesSpreadsheetService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public ExamsGradesSpreadsheetService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task<Guid> AddAsync(Dtos.ExamsGradesSpreadsheet dto)
        {
            var validator = new Dtos.ExamsGradesSpreadsheetValidator();
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

            if (!await _context.Set<Entities.StructuralUnit>().AnyAsync(d => d.Id == dto.StructuralUnitId))
            {
                throw new InvalidModelException($"Structural unit with id: {dto.StructuralUnitId} not found");
            }

            if (!await _context.Set<Entities.AcademicDiscipline>().AnyAsync(d => d.Id == dto.AcademicDisciplineId))
            {
                throw new InvalidModelException($"Academic discipline with id: {dto.AcademicDisciplineId} not found");
            }

            if (!await _context.Set<Entities.AcademicGroup>().AnyAsync(d => d.Id == dto.AcademicGroupId))
            {
                throw new InvalidModelException($"Academic group with id: {dto.AcademicGroupId} not found");
            }

            if (!await _context.Set<Entities.EducationProgram>().AnyAsync(d => d.Id == dto.EducationProgramId))
            {
                throw new InvalidModelException($"Education program with id: {dto.EducationProgramId} not found");
            }

            if (!await _context.Set<Entities.FormOfEducation>().AnyAsync(d => d.Id == dto.FormOfEducationId))
            {
                throw new InvalidModelException($"Form of education with id: {dto.FormOfEducationId} not found");
            }

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var examsGradesSpreadsheet = new Entities.ExamsGradesSpreadsheet
            {
                Id                              = id,
                CreatedAt                       = now,
                UpdatedAt                       = now,
                StructuralUnitId                = dto.StructuralUnitId,
                AcademicDisciplineId            = dto.AcademicDisciplineId,
                AcademicGroupId                 = dto.AcademicGroupId,
                EducationProgramId              = dto.EducationProgramId,
                ExamDate                        = dto.ExamDate,
                ExamsSpreadsheetAttestationType = dto.ExamsSpreadsheetAttestationType,
                ExamsSpreadsheetType            = dto.ExamsSpreadsheetType,
                FormOfEducationId               = dto.FormOfEducationId,
                SemesterNumber                  = dto.SemesterNumber,
                SpecialtyId                     = dto.SpecialtyId,
                SpreadsheetNumber               = dto.SpreadsheetNumber
            };

            await _context.AddAsync(examsGradesSpreadsheet);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.ExamsGradesSpreadsheet>> GetAllAsync()
        {
            return await _context.Set<Entities.ExamsGradesSpreadsheet>()
                                 .AsNoTracking()
                                 .Select(egs => new Dtos.ExamsGradesSpreadsheet
                                 {
                                     CreatedAt                       = egs.CreatedAt,
                                     StructuralUnitId                = egs.StructuralUnitId,
                                     Id                              = egs.Id,
                                     UpdatedAt                       = egs.UpdatedAt,
                                     SpreadsheetNumber               = egs.SpreadsheetNumber,
                                     SpecialtyId                     = egs.SpecialtyId,
                                     SemesterNumber                  = egs.SemesterNumber,
                                     FormOfEducationId               = egs.FormOfEducationId,
                                     ExamsSpreadsheetType            = egs.ExamsSpreadsheetType,
                                     ExamsSpreadsheetAttestationType = egs.ExamsSpreadsheetAttestationType,
                                     ExamDate                        = egs.ExamDate,
                                     EducationProgramId              = egs.EducationProgramId,
                                     AcademicGroupId                 = egs.AcademicGroupId,
                                     AcademicDisciplineId            = egs.AcademicDisciplineId
                                 })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.ExamsGradesSpreadsheet>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.ExamsGradesSpreadsheet>()
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(egs => new Dtos.ExamsGradesSpreadsheet
                                 {
                                     CreatedAt                       = egs.CreatedAt,
                                     StructuralUnitId                = egs.StructuralUnitId,
                                     Id                              = egs.Id,
                                     UpdatedAt                       = egs.UpdatedAt,
                                     SpreadsheetNumber               = egs.SpreadsheetNumber,
                                     SpecialtyId                     = egs.SpecialtyId,
                                     SemesterNumber                  = egs.SemesterNumber,
                                     FormOfEducationId               = egs.FormOfEducationId,
                                     ExamsSpreadsheetType            = egs.ExamsSpreadsheetType,
                                     ExamsSpreadsheetAttestationType = egs.ExamsSpreadsheetAttestationType,
                                     ExamDate                        = egs.ExamDate,
                                     EducationProgramId              = egs.EducationProgramId,
                                     AcademicGroupId                 = egs.AcademicGroupId,
                                     AcademicDisciplineId            = egs.AcademicDisciplineId
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.ExamsGradesSpreadsheet> GetByIdAsync(Guid id)
        {
            var examsGradesSpreadsheet = await _context.FindAsync<Entities.ExamsGradesSpreadsheet>(id);

            if (examsGradesSpreadsheet == null)
            {
                return null;
            }

            return new Dtos.ExamsGradesSpreadsheet
            {
                CreatedAt                       = examsGradesSpreadsheet.CreatedAt,
                StructuralUnitId                = examsGradesSpreadsheet.StructuralUnitId,
                Id                              = examsGradesSpreadsheet.Id,
                UpdatedAt                       = examsGradesSpreadsheet.UpdatedAt,
                SpreadsheetNumber               = examsGradesSpreadsheet.SpreadsheetNumber,
                SpecialtyId                     = examsGradesSpreadsheet.SpecialtyId,
                SemesterNumber                  = examsGradesSpreadsheet.SemesterNumber,
                FormOfEducationId               = examsGradesSpreadsheet.FormOfEducationId,
                ExamsSpreadsheetType            = examsGradesSpreadsheet.ExamsSpreadsheetType,
                ExamsSpreadsheetAttestationType = examsGradesSpreadsheet.ExamsSpreadsheetAttestationType,
                ExamDate                        = examsGradesSpreadsheet.ExamDate,
                EducationProgramId              = examsGradesSpreadsheet.EducationProgramId,
                AcademicGroupId                 = examsGradesSpreadsheet.AcademicGroupId,
                AcademicDisciplineId            = examsGradesSpreadsheet.AcademicDisciplineId
            };
        }

        public async Task<SieveResult<Dtos.ExamsGradesSpreadsheet>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var examsGradesSpreadsheetsQuery = _context.Set<Entities.ExamsGradesSpreadsheet>().AsNoTracking();

            examsGradesSpreadsheetsQuery = _sieveProcessor.Apply(model, examsGradesSpreadsheetsQuery, applyPagination: false);

            var result = new SieveResult<Dtos.ExamsGradesSpreadsheet>();
            result.TotalCount = await examsGradesSpreadsheetsQuery.CountAsync();

            var someExamsGradesSpreadsheets = await _sieveProcessor.Apply(model, examsGradesSpreadsheetsQuery, applyFiltering: false, applySorting: false).ToListAsync();

            result.Result = someExamsGradesSpreadsheets.Select(egs => new Dtos.ExamsGradesSpreadsheet
            {
                CreatedAt                       = egs.CreatedAt,
                StructuralUnitId                = egs.StructuralUnitId,
                Id                              = egs.Id,
                UpdatedAt                       = egs.UpdatedAt,
                SpreadsheetNumber               = egs.SpreadsheetNumber,
                SpecialtyId                     = egs.SpecialtyId,
                SemesterNumber                  = egs.SemesterNumber,
                FormOfEducationId               = egs.FormOfEducationId,
                ExamsSpreadsheetType            = egs.ExamsSpreadsheetType,
                ExamsSpreadsheetAttestationType = egs.ExamsSpreadsheetAttestationType,
                ExamDate                        = egs.ExamDate,
                EducationProgramId              = egs.EducationProgramId,
                AcademicGroupId                 = egs.AcademicGroupId,
                AcademicDisciplineId            = egs.AcademicDisciplineId
            });

            return result;
        }

        public async Task<IEnumerable<Dtos.ExamsGradesSpreadsheetInfo>> GetAllWithInfoAsync()
        {
            return await _context.Set<Entities.ExamsGradesSpreadsheet>()
                                 .Include(egs => egs.AcademicDiscipline)
                                 .Include(egs => egs.AcademicGroup)
                                 .Include(egs => egs.FormOfEducation)
                                 .Include(egs => egs.Specialty)
                                 .Include(egs => egs.StructuralUnit)
                                 .AsNoTracking()
                                 .Select(egs => new Dtos.ExamsGradesSpreadsheetInfo
                                 {
                                     CreatedAt                       = egs.CreatedAt,
                                     StructuralUnitId                = egs.StructuralUnitId,
                                     Id                              = egs.Id,
                                     UpdatedAt                       = egs.UpdatedAt,
                                     SpreadsheetNumber               = egs.SpreadsheetNumber,
                                     SpecialtyId                     = egs.SpecialtyId,
                                     SemesterNumber                  = egs.SemesterNumber,
                                     FormOfEducationId               = egs.FormOfEducationId,
                                     ExamsSpreadsheetType            = egs.ExamsSpreadsheetType,
                                     ExamsSpreadsheetAttestationType = egs.ExamsSpreadsheetAttestationType,
                                     ExamDate                        = egs.ExamDate,
                                     EducationProgramId              = egs.EducationProgramId,
                                     AcademicGroupId                 = egs.AcademicGroupId,
                                     AcademicDisciplineId            = egs.AcademicDisciplineId,

                                     DisciplineName                  = egs.AcademicDiscipline.FullName,
                                     GroupCode                       = egs.AcademicGroup.Code,
                                     NameOfFormOfEducation           = egs.FormOfEducation.Name,
                                     SpecialtyName                   = egs.Specialty.Name,
                                     StructuralUnitName              = egs.StructuralUnit.FullName
                                 })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.ExamsGradesSpreadsheetInfo>> GetAllWithInfoAsync(int page, int size)
        {
            return await _context.Set<Entities.ExamsGradesSpreadsheet>()
                                 .Include(egs => egs.AcademicDiscipline)
                                 .Include(egs => egs.AcademicGroup)
                                 .Include(egs => egs.FormOfEducation)
                                 .Include(egs => egs.Specialty)
                                 .Include(egs => egs.StructuralUnit)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(egs => new Dtos.ExamsGradesSpreadsheetInfo
                                 {
                                     CreatedAt                       = egs.CreatedAt,
                                     StructuralUnitId                = egs.StructuralUnitId,
                                     Id                              = egs.Id,
                                     UpdatedAt                       = egs.UpdatedAt,
                                     SpreadsheetNumber               = egs.SpreadsheetNumber,
                                     SpecialtyId                     = egs.SpecialtyId,
                                     SemesterNumber                  = egs.SemesterNumber,
                                     FormOfEducationId               = egs.FormOfEducationId,
                                     ExamsSpreadsheetType            = egs.ExamsSpreadsheetType,
                                     ExamsSpreadsheetAttestationType = egs.ExamsSpreadsheetAttestationType,
                                     ExamDate                        = egs.ExamDate,
                                     EducationProgramId              = egs.EducationProgramId,
                                     AcademicGroupId                 = egs.AcademicGroupId,
                                     AcademicDisciplineId            = egs.AcademicDisciplineId,

                                     DisciplineName                  = egs.AcademicDiscipline.FullName,
                                     GroupCode                       = egs.AcademicGroup.Code,
                                     NameOfFormOfEducation           = egs.FormOfEducation.Name,
                                     SpecialtyName                   = egs.Specialty.Name,
                                     StructuralUnitName              = egs.StructuralUnit.FullName
                                 })
                                 .ToListAsync();
        }

        public async Task<SieveResult<ExamsGradesSpreadsheetInfo>> GetSomeWithInfoAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var examsGradesSpreadsheetsQuery = _context.Set<Entities.ExamsGradesSpreadsheet>().AsNoTracking();

            examsGradesSpreadsheetsQuery = _sieveProcessor.Apply(model, examsGradesSpreadsheetsQuery, applyPagination: false);

            var result = new SieveResult<Dtos.ExamsGradesSpreadsheetInfo>();
            result.TotalCount = await examsGradesSpreadsheetsQuery.CountAsync();

            var someExamsGradesSpreadsheets = _sieveProcessor.Apply(model, examsGradesSpreadsheetsQuery, applyFiltering: false, applySorting: false);

            result.Result = someExamsGradesSpreadsheets.Select(egs => new Dtos.ExamsGradesSpreadsheetInfo
            {
                CreatedAt                       = egs.CreatedAt,
                StructuralUnitId                = egs.StructuralUnitId,
                Id                              = egs.Id,
                UpdatedAt                       = egs.UpdatedAt,
                SpreadsheetNumber               = egs.SpreadsheetNumber,
                SpecialtyId                     = egs.SpecialtyId,
                SemesterNumber                  = egs.SemesterNumber,
                FormOfEducationId               = egs.FormOfEducationId,
                ExamsSpreadsheetType            = egs.ExamsSpreadsheetType,
                ExamsSpreadsheetAttestationType = egs.ExamsSpreadsheetAttestationType,
                ExamDate                        = egs.ExamDate,
                EducationProgramId              = egs.EducationProgramId,
                AcademicGroupId                 = egs.AcademicGroupId,
                AcademicDisciplineId            = egs.AcademicDisciplineId,

                DisciplineName                  = egs.AcademicDiscipline.FullName,
                GroupCode                       = egs.AcademicGroup.Code,
                NameOfFormOfEducation           = egs.FormOfEducation.Name,
                SpecialtyName                   = egs.Specialty.Name,
                StructuralUnitName              = egs.StructuralUnit.FullName
            });

            return result;
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var examsGradesSpreadsheet = await _context.Set<Entities.ExamsGradesSpreadsheet>()
                                                       .FindAsync(id);

            if (examsGradesSpreadsheet == null)
            {
                throw new NotFoundException();
            }

            _context.Remove(examsGradesSpreadsheet);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.ExamsGradesSpreadsheet dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.ExamsGradesSpreadsheetValidator();
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

            if (!await _context.Set<Entities.StructuralUnit>().AnyAsync(d => d.Id == dto.StructuralUnitId))
            {
                throw new InvalidModelException($"Structural unit with id: {dto.StructuralUnitId} not found");
            }

            if (!await _context.Set<Entities.AcademicDiscipline>().AnyAsync(d => d.Id == dto.AcademicDisciplineId))
            {
                throw new InvalidModelException($"Academic discipline with id: {dto.AcademicDisciplineId} not found");
            }

            if (!await _context.Set<Entities.AcademicGroup>().AnyAsync(d => d.Id == dto.AcademicGroupId))
            {
                throw new InvalidModelException($"Academic group with id: {dto.AcademicGroupId} not found");
            }

            if (!await _context.Set<Entities.EducationProgram>().AnyAsync(d => d.Id == dto.EducationProgramId))
            {
                throw new InvalidModelException($"Education program with id: {dto.EducationProgramId} not found");
            }

            if (!await _context.Set<Entities.FormOfEducation>().AnyAsync(d => d.Id == dto.FormOfEducationId))
            {
                throw new InvalidModelException($"Form of education with id: {dto.FormOfEducationId} not found");
            }

            var examsGradesSpreadsheet = await _context.FindAsync<Entities.ExamsGradesSpreadsheet>(dto.Id);

            if (examsGradesSpreadsheet == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            examsGradesSpreadsheet.UpdatedAt                       = DateTime.UtcNow;
            examsGradesSpreadsheet.StructuralUnitId                = dto.StructuralUnitId;
            examsGradesSpreadsheet.AcademicDisciplineId            = dto.AcademicDisciplineId;
            examsGradesSpreadsheet.AcademicGroupId                 = dto.AcademicGroupId;
            examsGradesSpreadsheet.EducationProgramId              = dto.EducationProgramId;
            examsGradesSpreadsheet.ExamDate                        = dto.ExamDate;
            examsGradesSpreadsheet.ExamsSpreadsheetAttestationType = dto.ExamsSpreadsheetAttestationType;
            examsGradesSpreadsheet.ExamsSpreadsheetType            = dto.ExamsSpreadsheetType;
            examsGradesSpreadsheet.FormOfEducationId               = dto.FormOfEducationId;
            examsGradesSpreadsheet.SemesterNumber                  = dto.SemesterNumber;
            examsGradesSpreadsheet.SpecialtyId                     = dto.SpecialtyId;
            examsGradesSpreadsheet.SpreadsheetNumber               = dto.SpreadsheetNumber;

            await _context.SaveChangesAsync();
        }
    }
}
