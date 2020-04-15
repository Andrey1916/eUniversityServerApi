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
    public class RatingForDisciplineService : IRatingForDisciplineService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public RatingForDisciplineService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task<Guid> AddAsync(Dtos.RatingForDiscipline dto)
        {
            var validator = new Dtos.RatingForDisciplineValidator();
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

            if (!await _context.Set<Entities.AcademicDiscipline>().AnyAsync(d => d.Id == dto.AcademicDisciplineId))
            {
                throw new InvalidModelException($"Academic discipline with id: {dto.AcademicDisciplineId} not found");
            }

            if (!await _context.Set<Entities.AcademicGroup>().AnyAsync(d => d.Id == dto.AcademicGroupId))
            {
                throw new InvalidModelException($"Academic group with id: {dto.AcademicGroupId} not found");
            }

            if (!await _context.Set<Entities.ExamsGradesSpreadsheet>().AnyAsync(d => d.Id == dto.ExamsGradesSpreadsheetId))
            {
                throw new InvalidModelException($"Exams grades spreadsheet with id: {dto.ExamsGradesSpreadsheetId} not found");
            }

            if (!await _context.Set<Entities.Student>().AnyAsync(d => d.Id == dto.StudentId))
            {
                throw new InvalidModelException($"Student with id: {dto.StudentId} not found");
            }

            if (!await _context.Set<Entities.Teacher>().AnyAsync(d => d.Id == dto.TeacherId))
            {
                throw new InvalidModelException($"Teacher with id: {dto.TeacherId} not found");
            }

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var ratingForDiscipline = new Entities.RatingForDiscipline
            {
                Id                       = id,
                CreatedAt                = now,
                UpdatedAt                = now,
                AcademicDisciplineId     = dto.AcademicDisciplineId,
                AcademicGroupId          = dto.AcademicGroupId,
                Date                     = dto.Date,
                ExamsGradesSpreadsheetId = dto.ExamsGradesSpreadsheetId,
                Score                    = dto.Score,
                StudentId                = dto.StudentId,
                TeacherId                = dto.TeacherId
            };

            await _context.AddAsync(ratingForDiscipline);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.RatingForDiscipline>> GetAllAsync()
        {
            return await _context.Set<Entities.RatingForDiscipline>()
                                 .AsNoTracking()
                                 .Select(rfd => new Dtos.RatingForDiscipline
                                 {
                                     CreatedAt                = rfd.CreatedAt,
                                     Id                       = rfd.Id,
                                     UpdatedAt                = rfd.UpdatedAt,
                                     TeacherId                = rfd.TeacherId,
                                     StudentId                = rfd.StudentId,
                                     Score                    = rfd.Score,
                                     ExamsGradesSpreadsheetId = rfd.ExamsGradesSpreadsheetId,
                                     Date                     = rfd.Date,
                                     AcademicGroupId          = rfd.AcademicGroupId,
                                     AcademicDisciplineId     = rfd.AcademicDisciplineId
                                 })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.RatingForDiscipline>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.RatingForDiscipline>()
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(rfd => new Dtos.RatingForDiscipline
                                 {
                                     CreatedAt                = rfd.CreatedAt,
                                     Id                       = rfd.Id,
                                     UpdatedAt                = rfd.UpdatedAt,
                                     TeacherId                = rfd.TeacherId,
                                     StudentId                = rfd.StudentId,
                                     Score                    = rfd.Score,
                                     ExamsGradesSpreadsheetId = rfd.ExamsGradesSpreadsheetId,
                                     Date                     = rfd.Date,
                                     AcademicGroupId          = rfd.AcademicGroupId,
                                     AcademicDisciplineId     = rfd.AcademicDisciplineId
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.RatingForDiscipline> GetByIdAsync(Guid id)
        {
            var ratingForDiscipline = await _context.FindAsync<Entities.RatingForDiscipline>(id);

            if (ratingForDiscipline == null)
            {
                return null;
            }

            return new Dtos.RatingForDiscipline
            {
                CreatedAt                = ratingForDiscipline.CreatedAt,
                Id                       = ratingForDiscipline.Id,
                UpdatedAt                = ratingForDiscipline.UpdatedAt,
                TeacherId                = ratingForDiscipline.TeacherId,
                StudentId                = ratingForDiscipline.StudentId,
                Score                    = ratingForDiscipline.Score,
                ExamsGradesSpreadsheetId = ratingForDiscipline.ExamsGradesSpreadsheetId,
                Date                     = ratingForDiscipline.Date,
                AcademicGroupId          = ratingForDiscipline.AcademicGroupId,
                AcademicDisciplineId     = ratingForDiscipline.AcademicDisciplineId
            };
        }

        public async Task<SieveResult<Dtos.RatingForDiscipline>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var ratingForDisciplinesQuery = _context.Set<Entities.RatingForDiscipline>().AsNoTracking();

            ratingForDisciplinesQuery = _sieveProcessor.Apply(model, ratingForDisciplinesQuery, applyPagination: false);

            var result = new SieveResult<Dtos.RatingForDiscipline>();
            result.TotalCount = await ratingForDisciplinesQuery.CountAsync();

            var someRatingForDisciplines = await _sieveProcessor.Apply(model, ratingForDisciplinesQuery, applyFiltering: false, applySorting: false).ToListAsync();

            result.Result = someRatingForDisciplines.Select(rfd => new Dtos.RatingForDiscipline
            {
                CreatedAt                = rfd.CreatedAt,
                Id                       = rfd.Id,
                UpdatedAt                = rfd.UpdatedAt,
                TeacherId                = rfd.TeacherId,
                StudentId                = rfd.StudentId,
                Score                    = rfd.Score,
                ExamsGradesSpreadsheetId = rfd.ExamsGradesSpreadsheetId,
                Date                     = rfd.Date,
                AcademicGroupId          = rfd.AcademicGroupId,
                AcademicDisciplineId     = rfd.AcademicDisciplineId
            });

            return result;
        }

        public async Task<IEnumerable<RatingForDisciplineInfo>> GetAllWithInfoAsync()
        {
            return await _context.Set<Entities.RatingForDiscipline>()
                                 .Include(rfd => rfd.AcademicDiscipline)
                                 .Include(rfd => rfd.AcademicGroup)
                                 .Include(rfd => rfd.ExamsGradesSpreadsheet)
                                 .Include(rfd => rfd.Student)
                                 .ThenInclude(st => st.UserInfo)
                                 .Include(rfd => rfd.Teacher)
                                 .ThenInclude(t => t.UserInfo)
                                 .AsNoTracking()
                                 .Select(rfd => new Dtos.RatingForDisciplineInfo
                                 {
                                     CreatedAt                    = rfd.CreatedAt,
                                     Id                           = rfd.Id,
                                     UpdatedAt                    = rfd.UpdatedAt,
                                     TeacherId                    = rfd.TeacherId,
                                     StudentId                    = rfd.StudentId,
                                     Score                        = rfd.Score,
                                     ExamsGradesSpreadsheetId     = rfd.ExamsGradesSpreadsheetId,
                                     Date                         = rfd.Date,
                                     AcademicGroupId              = rfd.AcademicGroupId,
                                     AcademicDisciplineId         = rfd.AcademicDisciplineId,

                                     AcademicDisciplineName       = rfd.AcademicDiscipline.FullName,
                                     AcademicGroupCode            = rfd.AcademicGroup.Code,
                                     ExamsGradesSpreadsheetNumber = rfd.ExamsGradesSpreadsheet.SpreadsheetNumber,
                                     StudentName                  = rfd.Student.UserInfo.LastName + ' ' + rfd.Student.UserInfo.FirstName + ' ' + rfd.Student.UserInfo.Patronymic,
                                     TeacherName                  = rfd.Teacher.UserInfo.LastName + ' ' + rfd.Teacher.UserInfo.FirstName + ' ' + rfd.Teacher.UserInfo.Patronymic,
                                 })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<RatingForDisciplineInfo>> GetAllWithInfoAsync(int page, int size)
        {
            return await _context.Set<Entities.RatingForDiscipline>()
                                 .Include(rfd => rfd.AcademicDiscipline)
                                 .Include(rfd => rfd.AcademicGroup)
                                 .Include(rfd => rfd.ExamsGradesSpreadsheet)
                                 .Include(rfd => rfd.Student)
                                 .ThenInclude(st => st.UserInfo)
                                 .Include(rfd => rfd.Teacher)
                                 .ThenInclude(t => t.UserInfo)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(rfd => new Dtos.RatingForDisciplineInfo
                                 {
                                     CreatedAt                    = rfd.CreatedAt,
                                     Id                           = rfd.Id,
                                     UpdatedAt                    = rfd.UpdatedAt,
                                     TeacherId                    = rfd.TeacherId,
                                     StudentId                    = rfd.StudentId,
                                     Score                        = rfd.Score,
                                     ExamsGradesSpreadsheetId     = rfd.ExamsGradesSpreadsheetId,
                                     Date                         = rfd.Date,
                                     AcademicGroupId              = rfd.AcademicGroupId,
                                     AcademicDisciplineId         = rfd.AcademicDisciplineId,

                                     AcademicDisciplineName       = rfd.AcademicDiscipline.FullName,
                                     AcademicGroupCode            = rfd.AcademicGroup.Code,
                                     ExamsGradesSpreadsheetNumber = rfd.ExamsGradesSpreadsheet.SpreadsheetNumber,
                                     StudentName                  = rfd.Student.UserInfo.LastName + ' ' + rfd.Student.UserInfo.FirstName + ' ' + rfd.Student.UserInfo.Patronymic,
                                     TeacherName                  = rfd.Teacher.UserInfo.LastName + ' ' + rfd.Teacher.UserInfo.FirstName + ' ' + rfd.Teacher.UserInfo.Patronymic,
                                 })
                                 .ToListAsync();
        }

        public async Task<SieveResult<RatingForDisciplineInfo>> GetSomeWithInfoAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var ratingForDisciplinesQuery = _context.Set<Entities.RatingForDiscipline>()
                                                    .Include(rfd => rfd.AcademicDiscipline)
                                                    .Include(rfd => rfd.AcademicGroup)
                                                    .Include(rfd => rfd.ExamsGradesSpreadsheet)
                                                    .Include(rfd => rfd.Student)
                                                    .ThenInclude(st => st.UserInfo)
                                                    .Include(rfd => rfd.Teacher)
                                                    .ThenInclude(t => t.UserInfo)
                                                    .AsNoTracking();

            ratingForDisciplinesQuery = _sieveProcessor.Apply(model, ratingForDisciplinesQuery, applyPagination: false);

            var result = new SieveResult<Dtos.RatingForDisciplineInfo>();
            result.TotalCount = await ratingForDisciplinesQuery.CountAsync();

            var someRatingForDisciplines = _sieveProcessor.Apply(model, ratingForDisciplinesQuery, applyFiltering: false, applySorting: false);

            result.Result = someRatingForDisciplines.Select(rfd => new Dtos.RatingForDisciplineInfo
            {
                Id                           = rfd.Id,
                CreatedAt                    = rfd.CreatedAt,
                UpdatedAt                    = rfd.UpdatedAt,
                TeacherId                    = rfd.TeacherId,
                StudentId                    = rfd.StudentId,
                Score                        = rfd.Score,
                ExamsGradesSpreadsheetId     = rfd.ExamsGradesSpreadsheetId,
                Date                         = rfd.Date,
                AcademicGroupId              = rfd.AcademicGroupId,
                AcademicDisciplineId         = rfd.AcademicDisciplineId,

                AcademicDisciplineName       = rfd.AcademicDiscipline.FullName,
                AcademicGroupCode            = rfd.AcademicGroup.Code,
                ExamsGradesSpreadsheetNumber = rfd.ExamsGradesSpreadsheet.SpreadsheetNumber,
                StudentName                  = rfd.Student.UserInfo.LastName + ' ' + rfd.Student.UserInfo.FirstName + ' ' + rfd.Student.UserInfo.Patronymic,
                TeacherName                  = rfd.Teacher.UserInfo.LastName + ' ' + rfd.Teacher.UserInfo.FirstName + ' ' + rfd.Teacher.UserInfo.Patronymic,
            });

            return result;
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var ratingForDiscipline = await _context.Set<Entities.RatingForDiscipline>()
                                                    .FindAsync(id);

            if (ratingForDiscipline == null)
            {
                throw new NotFoundException();
            }

            _context.Remove(ratingForDiscipline);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.RatingForDiscipline dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.RatingForDisciplineValidator();
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

            if (!await _context.Set<Entities.AcademicDiscipline>().AnyAsync(d => d.Id == dto.AcademicDisciplineId))
            {
                throw new InvalidModelException($"Academic discipline with id: {dto.AcademicDisciplineId} not found");
            }

            if (!await _context.Set<Entities.AcademicGroup>().AnyAsync(d => d.Id == dto.AcademicGroupId))
            {
                throw new InvalidModelException($"Academic group with id: {dto.AcademicGroupId} not found");
            }

            if (!await _context.Set<Entities.ExamsGradesSpreadsheet>().AnyAsync(d => d.Id == dto.ExamsGradesSpreadsheetId))
            {
                throw new InvalidModelException($"Exams grades spreadsheet with id: {dto.ExamsGradesSpreadsheetId} not found");
            }

            if (!await _context.Set<Entities.Student>().AnyAsync(d => d.Id == dto.StudentId))
            {
                throw new InvalidModelException($"Student with id: {dto.StudentId} not found");
            }

            if (!await _context.Set<Entities.Teacher>().AnyAsync(d => d.Id == dto.TeacherId))
            {
                throw new InvalidModelException($"Teacher with id: {dto.TeacherId} not found");
            }

            var ratingForDiscipline = await _context.FindAsync<Entities.RatingForDiscipline>(dto.Id);

            if (ratingForDiscipline == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            ratingForDiscipline.UpdatedAt                = DateTime.UtcNow;
            ratingForDiscipline.AcademicDisciplineId     = dto.AcademicDisciplineId;
            ratingForDiscipline.AcademicGroupId          = dto.AcademicGroupId;
            ratingForDiscipline.Date                     = dto.Date;
            ratingForDiscipline.ExamsGradesSpreadsheetId = dto.ExamsGradesSpreadsheetId;
            ratingForDiscipline.Score                    = dto.Score;
            ratingForDiscipline.StudentId                = dto.StudentId;
            ratingForDiscipline.TeacherId                = dto.TeacherId;

            await _context.SaveChangesAsync();
        }
    }
}
