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
    public class TeacherService : ITeacherService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly UserInfoService _userInfoService;

        public TeacherService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
            
            this._userInfoService = new UserInfoService(context);
        }

        /// <exception cref="ServiceException"/>
        /// <exception cref="NotFoundException"/>
        public async Task<Guid> AddAsync(Dtos.Teacher dto)
        {
            var validator = new Dtos.TeacherValidator();
            ValidationResult result = validator.Validate(dto);

            if (!result.IsValid)
            {
                string errMess = string.Empty;

                foreach (var failure in result.Errors)
                {
                    errMess += $"Property { failure.PropertyName } failed validation. Error was: { failure.ErrorMessage }\n";
                }

                throw new ServiceException(errMess);
            }

            if (!await _context.Set<Entities.Department>().AnyAsync(d => d.Id == dto.DepartmentId))
            {
                throw new InvalidModelException($"Department with id: {dto.DepartmentId} not found");
            }

            var id = Guid.NewGuid();
            var uiId = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var userInfo = new Entities.UserInfo
            {
                DateOfBirth  = dto.DateOfBirth,
                PhoneNumber  = dto.PhoneNumber,
                Email        = dto.Email,
                FirstName    = dto.FirstName,
                FirstNameEng = dto.FirstNameEng,
                LastName     = dto.LastName,
                LastNameEng  = dto.LastNameEng,
                Patronymic   = dto.Patronymic,
                Id           = uiId
            };

            await _context.AddAsync(userInfo);

            var teacher = new Entities.Teacher
            {
                Id               = id,
                CreatedAt        = now,
                UpdatedAt        = now,
                AcademicRank     = dto.AcademicRank,
                DepartmentId     = dto.DepartmentId,
                Position         = dto.Position,
                ScientificDegree = dto.ScientificDegree,
                TypeOfEmployment = dto.TypeOfEmployment,
                UserInfoId       = uiId
            };

            await _context.AddAsync(teacher);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.Teacher>> GetAllAsync()
        {
            return await _context.Set<Entities.Teacher>()
                                 .Include(x => x.UserInfo)
                                 .AsNoTracking()
                                 .Select(t => new Dtos.Teacher
                                 {
                                     CreatedAt        = t.CreatedAt,
                                     Id               = t.Id,
                                     UpdatedAt        = t.UpdatedAt,
                                     TypeOfEmployment = t.TypeOfEmployment,
                                     ScientificDegree = t.ScientificDegree,
                                     Position         = t.Position,
                                     DepartmentId     = t.DepartmentId,
                                     AcademicRank     = t.AcademicRank,

                                     DateOfBirth      = t.UserInfo.DateOfBirth,
                                     Email            = t.UserInfo.Email,
                                     FirstName        = t.UserInfo.FirstName,
                                     FirstNameEng     = t.UserInfo.FirstNameEng,
                                     LastName         = t.UserInfo.LastName,
                                     LastNameEng      = t.UserInfo.LastNameEng,
                                     Patronymic       = t.UserInfo.Patronymic,
                                     PhoneNumber      = t.UserInfo.PhoneNumber
                                 })
                                 .OrderBy(s => s.FirstName)
                                 .ThenBy(s => s.LastName)
                                 .ThenBy(s => s.Patronymic)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.Teacher>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.Teacher>()
                                 .Include(x => x.UserInfo)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(t => new Dtos.Teacher
                                 {
                                     CreatedAt        = t.CreatedAt,
                                     Id               = t.Id,
                                     UpdatedAt        = t.UpdatedAt,
                                     TypeOfEmployment = t.TypeOfEmployment,
                                     ScientificDegree = t.ScientificDegree,
                                     Position         = t.Position,
                                     DepartmentId     = t.DepartmentId,
                                     AcademicRank     = t.AcademicRank,

                                     DateOfBirth      = t.UserInfo.DateOfBirth,
                                     Email            = t.UserInfo.Email,
                                     FirstName        = t.UserInfo.FirstName,
                                     FirstNameEng     = t.UserInfo.FirstNameEng,
                                     LastName         = t.UserInfo.LastName,
                                     LastNameEng      = t.UserInfo.LastNameEng,
                                     Patronymic       = t.UserInfo.Patronymic,
                                     PhoneNumber      = t.UserInfo.PhoneNumber
                                 })
                                 .ToListAsync();
        }

        public async Task<Dtos.Teacher> GetByIdAsync(Guid id)
        {
            var teacher = await _context.Set<Entities.Teacher>()
                                        .Include(x => x.UserInfo)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                return null;
            }

            return new Dtos.Teacher
            {
                CreatedAt        = teacher.CreatedAt,
                Id               = teacher.Id,
                UpdatedAt        = teacher.UpdatedAt,
                TypeOfEmployment = teacher.TypeOfEmployment,
                ScientificDegree = teacher.ScientificDegree,
                Position         = teacher.Position,
                DepartmentId     = teacher.DepartmentId,
                AcademicRank     = teacher.AcademicRank,

                DateOfBirth      = teacher.UserInfo.DateOfBirth,
                Email            = teacher.UserInfo.Email,
                FirstName        = teacher.UserInfo.FirstName,
                FirstNameEng     = teacher.UserInfo.FirstNameEng,
                LastName         = teacher.UserInfo.LastName,
                LastNameEng      = teacher.UserInfo.LastNameEng,
                Patronymic       = teacher.UserInfo.Patronymic,
                PhoneNumber      = teacher.UserInfo.PhoneNumber
            };
        }

        public async Task<SieveResult<Dtos.Teacher>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var teachersQuery = _context.Set<Entities.Teacher>()
                                        .Include(en => en.UserInfo)
                                        .AsNoTracking();

            teachersQuery = _sieveProcessor.Apply(model, teachersQuery, applyPagination: false);

            var result = new SieveResult<Dtos.Teacher>();
            result.TotalCount = await teachersQuery.CountAsync();

            var someTeachers = _sieveProcessor.Apply(model, teachersQuery, applyFiltering: false, applySorting: false);

            result.Result = someTeachers.Select(teacher => new Dtos.Teacher
            {
                CreatedAt        = teacher.CreatedAt,
                Id               = teacher.Id,
                UpdatedAt        = teacher.UpdatedAt,
                TypeOfEmployment = teacher.TypeOfEmployment,
                ScientificDegree = teacher.ScientificDegree,
                Position         = teacher.Position,
                DepartmentId     = teacher.DepartmentId,
                AcademicRank     = teacher.AcademicRank,

                DateOfBirth      = teacher.UserInfo.DateOfBirth,
                Email            = teacher.UserInfo.Email,
                FirstName        = teacher.UserInfo.FirstName,
                FirstNameEng     = teacher.UserInfo.FirstNameEng,
                LastName         = teacher.UserInfo.LastName,
                LastNameEng      = teacher.UserInfo.LastNameEng,
                Patronymic       = teacher.UserInfo.Patronymic,
                PhoneNumber      = teacher.UserInfo.PhoneNumber
            });

            return result;
        }

        public async Task<IEnumerable<TeacherInfo>> GetAllWithInfoAsync()
        {
            return await _context.Set<Entities.Teacher>()
                                 .Include(x => x.UserInfo)
                                 .Include(x => x.Department)
                                 .AsNoTracking()
                                 .Select(t => new Dtos.TeacherInfo
                                 {
                                     CreatedAt        = t.CreatedAt,
                                     Id               = t.Id,
                                     UpdatedAt        = t.UpdatedAt,
                                     TypeOfEmployment = t.TypeOfEmployment,
                                     ScientificDegree = t.ScientificDegree,
                                     Position         = t.Position,
                                     DepartmentId     = t.DepartmentId,
                                     AcademicRank     = t.AcademicRank,

                                     DateOfBirth      = t.UserInfo.DateOfBirth,
                                     Email            = t.UserInfo.Email,
                                     FirstName        = t.UserInfo.FirstName,
                                     FirstNameEng     = t.UserInfo.FirstNameEng,
                                     LastName         = t.UserInfo.LastName,
                                     LastNameEng      = t.UserInfo.LastNameEng,
                                     Patronymic       = t.UserInfo.Patronymic,
                                     PhoneNumber      = t.UserInfo.PhoneNumber,

                                     DepartmentName   = t.Department.FullName
                                 })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<TeacherInfo>> GetAllWithInfoAsync(int page, int size)
        {
            return await _context.Set<Entities.Teacher>()
                                 .Include(x => x.UserInfo)
                                 .Include(x => x.Department)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(t => new Dtos.TeacherInfo
                                 {
                                     CreatedAt        = t.CreatedAt,
                                     Id               = t.Id,
                                     UpdatedAt        = t.UpdatedAt,
                                     TypeOfEmployment = t.TypeOfEmployment,
                                     ScientificDegree = t.ScientificDegree,
                                     Position         = t.Position,
                                     DepartmentId     = t.DepartmentId,
                                     AcademicRank     = t.AcademicRank,

                                     DateOfBirth      = t.UserInfo.DateOfBirth,
                                     Email            = t.UserInfo.Email,
                                     FirstName        = t.UserInfo.FirstName,
                                     FirstNameEng     = t.UserInfo.FirstNameEng,
                                     LastName         = t.UserInfo.LastName,
                                     LastNameEng      = t.UserInfo.LastNameEng,
                                     Patronymic       = t.UserInfo.Patronymic,
                                     PhoneNumber      = t.UserInfo.PhoneNumber,

                                     DepartmentName   = t.Department.FullName
                                 })
                                 .ToListAsync();
        }

        public async Task<SieveResult<TeacherInfo>> GetSomeWithInfoAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var teachersQuery = _context.Set<Entities.Teacher>()
                                        .Include(en => en.UserInfo)
                                        .Include(en => en.Department)
                                        .AsNoTracking();

            teachersQuery = _sieveProcessor.Apply(model, teachersQuery, applyPagination: false);

            var result = new SieveResult<Dtos.TeacherInfo>();
            result.TotalCount = await teachersQuery.CountAsync();

            var someTeachers = await _sieveProcessor.Apply(model, teachersQuery, applyFiltering: false, applySorting: false).ToListAsync();

            result.Result = someTeachers.Select(teacher => new Dtos.TeacherInfo
            {
                CreatedAt        = teacher.CreatedAt,
                Id               = teacher.Id,
                UpdatedAt        = teacher.UpdatedAt,
                TypeOfEmployment = teacher.TypeOfEmployment,
                ScientificDegree = teacher.ScientificDegree,
                Position         = teacher.Position,
                DepartmentId     = teacher.DepartmentId,
                AcademicRank     = teacher.AcademicRank,

                DateOfBirth      = teacher.UserInfo.DateOfBirth,
                Email            = teacher.UserInfo.Email,
                FirstName        = teacher.UserInfo.FirstName,
                FirstNameEng     = teacher.UserInfo.FirstNameEng,
                LastName         = teacher.UserInfo.LastName,
                LastNameEng      = teacher.UserInfo.LastNameEng,
                Patronymic       = teacher.UserInfo.Patronymic,
                PhoneNumber      = teacher.UserInfo.PhoneNumber,

                DepartmentName   = teacher.Department.FullName
            });

            return result;
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveAsync(Guid id)
        {
            var teacher = await _context.Set<Entities.Teacher>()
                                        .FirstOrDefaultAsync(x => x.Id == id);
            if (teacher == null)
            {
                throw new NotFoundException("Teacher not found.");
            }

            var userInfoId = teacher.UserInfoId;

            _context.Remove(teacher);
            await _context.SaveChangesAsync();
            await _userInfoService.RemoveAsync(userInfoId);
        }

        /// <exception cref="ServiceException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.Teacher dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.TeacherValidator();
            ValidationResult result = validator.Validate(dto);

            if (!result.IsValid)
            {
                string errMess = string.Empty;

                foreach (var failure in result.Errors)
                {
                    errMess += $"Property { failure.PropertyName } failed validation. Error was: { failure.ErrorMessage }\n";
                }

                throw new ServiceException(errMess);
            }

            if (!await _context.Set<Entities.Department>().AnyAsync(d => d.Id == dto.DepartmentId))
            {
                throw new InvalidModelException($"Department with id: {dto.DepartmentId} not found");
            }

            var teacher = await _context.Set<Entities.Teacher>()
                                        .Include(x => x.UserInfo)
                                        .FirstOrDefaultAsync(t => t.Id == dto.Id);

            if (teacher == null)
            {
                throw new NotFoundException($"Entity with id: {dto.Id} not found.");
            }

            teacher.UpdatedAt             = DateTime.UtcNow;
            teacher.AcademicRank          = dto.AcademicRank;
            teacher.DepartmentId          = dto.DepartmentId;
            teacher.Position              = dto.Position;
            teacher.ScientificDegree      = dto.ScientificDegree;
            teacher.TypeOfEmployment      = dto.TypeOfEmployment;
                                          
            teacher.UserInfo.DateOfBirth  = dto.DateOfBirth;
            teacher.UserInfo.Email        = dto.Email;
            teacher.UserInfo.FirstName    = dto.FirstName;
            teacher.UserInfo.FirstNameEng = dto.FirstNameEng;
            teacher.UserInfo.LastName     = dto.LastName;
            teacher.UserInfo.LastNameEng  = dto.LastNameEng;
            teacher.UserInfo.Patronymic   = dto.Patronymic;
            teacher.UserInfo.PhoneNumber  = dto.PhoneNumber;

            await _context.SaveChangesAsync();
        }
    }
}
