using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eUniversityServer.Services.Dtos;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using Entities = eUniversityServer.DAL.Entities;

namespace eUniversityServer.Services
{
    public class StudentService : IStudentService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly UserInfoService _userInfoService;

        public StudentService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
            
            this._userInfoService = new UserInfoService(context);
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task<Guid> AddAsync(Dtos.Student dto)
        {
            var validator = new Dtos.StudentValidator();
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

            if (!await _context.Set<Entities.AcademicGroup>().AnyAsync(d => d.Id == dto.AcademicGroupId))
            {
                throw new InvalidModelException($"Academic group with id: {dto.AcademicGroupId} not found");
            }

            if (dto.EducationProgramId != null && !await _context.Set<Entities.EducationProgram>().AnyAsync(d => d.Id == dto.EducationProgramId))
            {
                throw new InvalidModelException($"Education program with id: {dto.EducationProgramId} not found");
            }

            if (!await _context.Set<Entities.EducationLevel>().AnyAsync(d => d.Id == dto.EducationLevelId))
            {
                throw new InvalidModelException($"Education level with id: {dto.EducationLevelId} not found");
            }

            if (!await _context.Set<Entities.FormOfEducation>().AnyAsync(d => d.Id == dto.FormOfEducationId))
            {
                throw new InvalidModelException($"Form of education with id: {dto.FormOfEducationId} not found");
            }

            if (dto.PrivilegeId != null && !await _context.Set<Entities.Privilege>().AnyAsync(d => d.Id == dto.PrivilegeId))
            {
                throw new InvalidModelException($"Privilege with id: {dto.PrivilegeId} not found");
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

            var student = new Entities.Student
            {
                Id                         = id,
                CreatedAt                  = now,
                UpdatedAt                  = now,
                AcademicGroupId            = dto.AcademicGroupId,
                AcceleratedFormOfEducation = dto.AcceleratedFormOfEducation,
                AddressOfResidence         = dto.AddressOfResidence,
                Chummery                   = dto.Chummery,
                EducationProgramId         = dto.EducationProgramId,
                EducationLevelId           = dto.EducationLevelId,
                EndDate                    = dto.EndDate,
                EntryDate                  = dto.EntryDate,
                FormOfEducationId          = dto.FormOfEducationId,
                Financing                  = dto.Financing,
                ForeignLanguage            = dto.ForeignLanguage,
                MilitaryRegistration       = dto.MilitaryRegistration,
                NumberOfRecordBook         = dto.NumberOfRecordBook,
                PrivilegeId                = dto.PrivilegeId,
                Sex                        = dto.Sex,
                StudentTicketNumber        = dto.StudentTicketNumber,
                UserInfoId                 = uiId
            };

            await _context.AddAsync(student);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<Dtos.Student>> GetAllAsync()
        {
            return await _context.Set<Entities.Student>()
                                 .Include(x => x.UserInfo)
                                 .AsNoTracking()
                                 .Select(s => new Dtos.Student
                                 {
                                     CreatedAt                  = s.CreatedAt,
                                     Id                         = s.Id,
                                     UpdatedAt                  = s.UpdatedAt,
                                     StudentTicketNumber        = s.StudentTicketNumber,
                                     Sex                        = s.Sex,
                                     PrivilegeId                = s.PrivilegeId,
                                     NumberOfRecordBook         = s.NumberOfRecordBook,
                                     MilitaryRegistration       = s.MilitaryRegistration,
                                     ForeignLanguage            = s.ForeignLanguage,
                                     Financing                  = s.Financing,
                                     FormOfEducationId          = s.FormOfEducationId,
                                     EntryDate                  = s.EntryDate,
                                     AcademicGroupId            = s.AcademicGroupId,
                                     AcceleratedFormOfEducation = s.AcceleratedFormOfEducation,
                                     AddressOfResidence         = s.AddressOfResidence,
                                     Chummery                   = s.Chummery,
                                     EducationLevelId           = s.EducationLevelId,
                                     EducationProgramId         = s.EducationProgramId,
                                     EndDate                    = s.EndDate,

                                     DateOfBirth                = s.UserInfo.DateOfBirth,
                                     Email                      = s.UserInfo.Email,
                                     FirstName                  = s.UserInfo.FirstName,
                                     FirstNameEng               = s.UserInfo.FirstNameEng,
                                     LastName                   = s.UserInfo.LastName,
                                     LastNameEng                = s.UserInfo.LastNameEng,
                                     Patronymic                 = s.UserInfo.Patronymic,
                                     PhoneNumber                = s.UserInfo.PhoneNumber
                                 })
                                 .OrderBy(s => s.FirstName)
                                 .ThenBy(s => s.LastName)
                                 .ThenBy(s => s.Patronymic)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Dtos.Student>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.Student>()
                                 .Include(x => x.UserInfo)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(s => new Dtos.Student
                                 {
                                     CreatedAt                  = s.CreatedAt,
                                     Id                         = s.Id,
                                     UpdatedAt                  = s.UpdatedAt,
                                     StudentTicketNumber        = s.StudentTicketNumber,
                                     Sex                        = s.Sex,
                                     PrivilegeId                = s.PrivilegeId,
                                     NumberOfRecordBook         = s.NumberOfRecordBook,
                                     MilitaryRegistration       = s.MilitaryRegistration,
                                     ForeignLanguage            = s.ForeignLanguage,
                                     Financing                  = s.Financing,
                                     FormOfEducationId          = s.FormOfEducationId,
                                     EntryDate                  = s.EntryDate,
                                     AcademicGroupId            = s.AcademicGroupId,
                                     AcceleratedFormOfEducation = s.AcceleratedFormOfEducation,
                                     AddressOfResidence         = s.AddressOfResidence,
                                     Chummery                   = s.Chummery,
                                     EducationLevelId           = s.EducationLevelId,
                                     EducationProgramId         = s.EducationProgramId,
                                     EndDate                    = s.EndDate,

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

        public async Task<Dtos.Student> GetByIdAsync(Guid id)
        {
            var student = await _context.Set<Entities.Student>()
                                        .Include(x => x.UserInfo)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(t => t.Id == id);

            if (student == null)
            {
                return null;
            }

            return new Dtos.Student
            {
                CreatedAt                  = student.CreatedAt,
                Id                         = student.Id,
                UpdatedAt                  = student.UpdatedAt,
                StudentTicketNumber        = student.StudentTicketNumber,
                Sex                        = student.Sex,
                PrivilegeId                = student.PrivilegeId,
                NumberOfRecordBook         = student.NumberOfRecordBook,
                MilitaryRegistration       = student.MilitaryRegistration,
                ForeignLanguage            = student.ForeignLanguage,
                Financing                  = student.Financing,
                FormOfEducationId          = student.FormOfEducationId,
                EntryDate                  = student.EntryDate,
                AcademicGroupId            = student.AcademicGroupId,
                AcceleratedFormOfEducation = student.AcceleratedFormOfEducation,
                AddressOfResidence         = student.AddressOfResidence,
                Chummery                   = student.Chummery,
                EducationLevelId           = student.EducationLevelId,
                EducationProgramId         = student.EducationProgramId,
                EndDate                    = student.EndDate,

                DateOfBirth                = student.UserInfo.DateOfBirth,
                Email                      = student.UserInfo.Email,
                FirstName                  = student.UserInfo.FirstName,
                FirstNameEng               = student.UserInfo.FirstNameEng,
                LastName                   = student.UserInfo.LastName,
                LastNameEng                = student.UserInfo.LastNameEng,
                Patronymic                 = student.UserInfo.Patronymic,
                PhoneNumber                = student.UserInfo.PhoneNumber
            };
        }

        /// <exception cref="NotFoundException"/>
        public async Task<Dtos.EducationDocument> GetEducationDocumentAsync(Guid studentId)
        {
            var student = await _context.Set<Entities.Student>()
                                        .Include(x => x.EducationDocument)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == studentId);

            if (student == null)
            {
                throw new NotFoundException("Student not found.");
            }

            if (student.EducationDocument == null)
            {
                return null;
            }

            return new Dtos.EducationDocument
            {
                CreatedAt        = student.EducationDocument.CreatedAt,
                DateOfIssue      = student.EducationDocument.DateOfIssue,
                IssuingAuthority = student.EducationDocument.IssuingAuthority,
                Number           = student.EducationDocument.Number,
                Series           = student.EducationDocument.Series,
                UpdatedAt        = student.EducationDocument.UpdatedAt
            };
        }

        /// <exception cref="NotFoundException"/>
        public async Task<Dtos.IdentificationCode> GetIdentificationCodeAsync(Guid studentId)
        {
            var student = await _context.Set<Entities.Student>()
                                        .Include(x => x.IdentificationCode)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == studentId);

            if (student == null)
            {
                throw new NotFoundException("Student not found.");
            }

            if (student.IdentificationCode == null)
            {
                return null;
            }

            return new Dtos.IdentificationCode
            {
                CreatedAt        = student.IdentificationCode.CreatedAt,
                DateOfIssue      = student.IdentificationCode.IdentificationCodeDateOfIssue,
                IssuingAuthority = student.IdentificationCode.IdentificationCodeIssuingAuthority,
                Number           = student.IdentificationCode.IdentificationCodeNumber,
                UpdatedAt        = student.IdentificationCode.UpdatedAt
            };
        }

        /// <exception cref="NotFoundException"/>
        public async Task<Dtos.Passport> GetPassportAsync(Guid studentId)
        {
            var student = await _context.Set<Entities.Student>()
                                       .Include(x => x.Passport)
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.Id == studentId);

            if (student == null)
            {
                throw new NotFoundException("Student not found.");
            }

            if (student.Passport == null)
            {
                return null;
            }

            return new Dtos.Passport
            {
                CreatedAt                = student.Passport.CreatedAt,
                PassportDateOfIssue      = student.Passport.PassportDateOfIssue,
                PassportIssuingAuthority = student.Passport.PassportIssuingAuthority,
                MaritalStatus            = student.Passport.MaritalStatus,
                Nationality              = student.Passport.Nationality,
                PassportNumber           = student.Passport.PassportNumber,
                PassportSeries           = student.Passport.PassportSeries,
                PlaceOfBirth             = student.Passport.PlaceOfBirth,
                RegistrationAddress      = student.Passport.RegistrationAddress,
                UpdatedAt                = student.Passport.UpdatedAt
            };
        }

        public async Task<SieveResult<Dtos.Student>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var studentsQuery = _context.Set<Entities.Student>()
                                        .Include(en => en.UserInfo)
                                        .AsNoTracking();

            studentsQuery = _sieveProcessor.Apply(model, studentsQuery, applyPagination: false);

            var result = new SieveResult<Dtos.Student>();
            result.TotalCount = await studentsQuery.CountAsync();

            var someStudents = _sieveProcessor.Apply(model, studentsQuery, applyFiltering: false, applySorting: false);

            result.Result = someStudents.Select(s => new Dtos.Student
            {
                CreatedAt                  = s.CreatedAt,
                Id                         = s.Id,
                UpdatedAt                  = s.UpdatedAt,
                StudentTicketNumber        = s.StudentTicketNumber,
                Sex                        = s.Sex,
                PrivilegeId                = s.PrivilegeId,
                NumberOfRecordBook         = s.NumberOfRecordBook,
                MilitaryRegistration       = s.MilitaryRegistration,
                ForeignLanguage            = s.ForeignLanguage,
                Financing                  = s.Financing,
                FormOfEducationId          = s.FormOfEducationId,
                EntryDate                  = s.EntryDate,
                AcademicGroupId            = s.AcademicGroupId,
                AcceleratedFormOfEducation = s.AcceleratedFormOfEducation,
                AddressOfResidence         = s.AddressOfResidence,
                Chummery                   = s.Chummery,
                EducationLevelId           = s.EducationLevelId,
                EducationProgramId         = s.EducationProgramId,
                EndDate                    = s.EndDate,

                DateOfBirth                = s.UserInfo.DateOfBirth,
                Email                      = s.UserInfo.Email,
                FirstName                  = s.UserInfo.FirstName,
                FirstNameEng               = s.UserInfo.FirstNameEng,
                LastName                   = s.UserInfo.LastName,
                LastNameEng                = s.UserInfo.LastNameEng,
                Patronymic                 = s.UserInfo.Patronymic,
                PhoneNumber                = s.UserInfo.PhoneNumber
            });

            return result;
        }

        public async Task<IEnumerable<StudentInfo>> GetAllWithInfoAsync()
        {
            return await _context.Set<Entities.Student>()
                                 .Include(x => x.UserInfo)
                                 .Include(x => x.AcademicGroup)
                                 .Include(x => x.EducationLevel)
                                 .Include(x => x.FormOfEducation)
                                 .AsNoTracking()
                                 .Select(s => new Dtos.StudentInfo
                                 {
                                     CreatedAt                  = s.CreatedAt,
                                     Id                         = s.Id,
                                     UpdatedAt                  = s.UpdatedAt,
                                     StudentTicketNumber        = s.StudentTicketNumber,
                                     Sex                        = s.Sex,
                                     PrivilegeId                = s.PrivilegeId,
                                     NumberOfRecordBook         = s.NumberOfRecordBook,
                                     MilitaryRegistration       = s.MilitaryRegistration,
                                     ForeignLanguage            = s.ForeignLanguage,
                                     Financing                  = s.Financing,
                                     FormOfEducationId          = s.FormOfEducationId,
                                     EntryDate                  = s.EntryDate,
                                     AcademicGroupId            = s.AcademicGroupId,
                                     AcceleratedFormOfEducation = s.AcceleratedFormOfEducation,
                                     AddressOfResidence         = s.AddressOfResidence,
                                     Chummery                   = s.Chummery,
                                     EducationLevelId           = s.EducationLevelId,
                                     EducationProgramId         = s.EducationProgramId,
                                     EndDate                    = s.EndDate,

                                     DateOfBirth                = s.UserInfo.DateOfBirth,
                                     Email                      = s.UserInfo.Email,
                                     FirstName                  = s.UserInfo.FirstName,
                                     FirstNameEng               = s.UserInfo.FirstNameEng,
                                     LastName                   = s.UserInfo.LastName,
                                     LastNameEng                = s.UserInfo.LastNameEng,
                                     Patronymic                 = s.UserInfo.Patronymic,
                                     PhoneNumber                = s.UserInfo.PhoneNumber,

                                     AcademicGroupCode          = s.AcademicGroup.Code,
                                     EducationLevel             = s.EducationLevel.Name,
                                     FormOfEducation            = s.FormOfEducation.Name
                                 })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<StudentInfo>> GetAllWithInfoAsync(int page, int size)
        {
            return await _context.Set<Entities.Student>()
                                 .Include(x => x.UserInfo)
                                 .Include(x => x.AcademicGroup)
                                 .Include(x => x.EducationLevel)
                                 .Include(x => x.FormOfEducation)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(s => new Dtos.StudentInfo
                                 {
                                     CreatedAt                  = s.CreatedAt,
                                     Id                         = s.Id,
                                     UpdatedAt                  = s.UpdatedAt,
                                     StudentTicketNumber        = s.StudentTicketNumber,
                                     Sex                        = s.Sex,
                                     PrivilegeId                = s.PrivilegeId,
                                     NumberOfRecordBook         = s.NumberOfRecordBook,
                                     MilitaryRegistration       = s.MilitaryRegistration,
                                     ForeignLanguage            = s.ForeignLanguage,
                                     Financing                  = s.Financing,
                                     FormOfEducationId          = s.FormOfEducationId,
                                     EntryDate                  = s.EntryDate,
                                     AcademicGroupId            = s.AcademicGroupId,
                                     AcceleratedFormOfEducation = s.AcceleratedFormOfEducation,
                                     AddressOfResidence         = s.AddressOfResidence,
                                     Chummery                   = s.Chummery,
                                     EducationLevelId           = s.EducationLevelId,
                                     EducationProgramId         = s.EducationProgramId,
                                     EndDate                    = s.EndDate,

                                     DateOfBirth                = s.UserInfo.DateOfBirth,
                                     Email                      = s.UserInfo.Email,
                                     FirstName                  = s.UserInfo.FirstName,
                                     FirstNameEng               = s.UserInfo.FirstNameEng,
                                     LastName                   = s.UserInfo.LastName,
                                     LastNameEng                = s.UserInfo.LastNameEng,
                                     Patronymic                 = s.UserInfo.Patronymic,
                                     PhoneNumber                = s.UserInfo.PhoneNumber,

                                     AcademicGroupCode          = s.AcademicGroup.Code,
                                     EducationLevel             = s.EducationLevel.Name,
                                     FormOfEducation            = s.FormOfEducation.Name
                                 })
                                 .ToListAsync();
        }

        public async Task<SieveResult<StudentInfo>> GetSomeWithInfoAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var studentsQuery = _context.Set<Entities.Student>()
                                        .Include(en => en.UserInfo)
                                        .Include(x => x.AcademicGroup)
                                        .Include(x => x.EducationLevel)
                                        .Include(x => x.FormOfEducation)
                                        .AsNoTracking();

            studentsQuery = _sieveProcessor.Apply(model, studentsQuery, applyPagination: false);

            var result = new SieveResult<Dtos.StudentInfo>();
            result.TotalCount = await studentsQuery.CountAsync();

            var someStudents = _sieveProcessor.Apply(model, studentsQuery, applyFiltering: false, applySorting: false);

            result.Result = someStudents.Select(s => new Dtos.StudentInfo
            {
                CreatedAt                  = s.CreatedAt,
                Id                         = s.Id,
                UpdatedAt                  = s.UpdatedAt,
                StudentTicketNumber        = s.StudentTicketNumber,
                Sex                        = s.Sex,
                PrivilegeId                = s.PrivilegeId,
                NumberOfRecordBook         = s.NumberOfRecordBook,
                MilitaryRegistration       = s.MilitaryRegistration,
                ForeignLanguage            = s.ForeignLanguage,
                Financing                  = s.Financing,
                FormOfEducationId          = s.FormOfEducationId,
                EntryDate                  = s.EntryDate,
                AcademicGroupId            = s.AcademicGroupId,
                AcceleratedFormOfEducation = s.AcceleratedFormOfEducation,
                AddressOfResidence         = s.AddressOfResidence,
                Chummery                   = s.Chummery,
                EducationLevelId           = s.EducationLevelId,
                EducationProgramId         = s.EducationProgramId,
                EndDate                    = s.EndDate,

                DateOfBirth                = s.UserInfo.DateOfBirth,
                Email                      = s.UserInfo.Email,
                FirstName                  = s.UserInfo.FirstName,
                FirstNameEng               = s.UserInfo.FirstNameEng,
                LastName                   = s.UserInfo.LastName,
                LastNameEng                = s.UserInfo.LastNameEng,
                Patronymic                 = s.UserInfo.Patronymic,
                PhoneNumber                = s.UserInfo.PhoneNumber,

                AcademicGroupCode          = s.AcademicGroup.Code,
                EducationLevel             = s.EducationLevel.Name,
                FormOfEducation            = s.FormOfEducation.Name
            });

            return result;
        }

        public async Task<IEnumerable<Dtos.Student>> GetStudentBySpecialty(Guid specialtyId)
        {
            return await _context.Set<Entities.Student>()
                                 .Include(s => s.AcademicGroup)
                                 .Where(student => student.AcademicGroup.SpecialtyId == specialtyId)
                                 .AsNoTracking()
                                 .Select(s => new Dtos.Student
                                 {
                                     CreatedAt                  = s.CreatedAt,
                                     Id                         = s.Id,
                                     UpdatedAt                  = s.UpdatedAt,
                                     StudentTicketNumber        = s.StudentTicketNumber,
                                     Sex                        = s.Sex,
                                     PrivilegeId                = s.PrivilegeId,
                                     NumberOfRecordBook         = s.NumberOfRecordBook,
                                     MilitaryRegistration       = s.MilitaryRegistration,
                                     ForeignLanguage            = s.ForeignLanguage,
                                     Financing                  = s.Financing,
                                     FormOfEducationId          = s.FormOfEducationId,
                                     EntryDate                  = s.EntryDate,
                                     AcademicGroupId            = s.AcademicGroupId,
                                     AcceleratedFormOfEducation = s.AcceleratedFormOfEducation,
                                     AddressOfResidence         = s.AddressOfResidence,
                                     Chummery                   = s.Chummery,
                                     EducationLevelId           = s.EducationLevelId,
                                     EducationProgramId         = s.EducationProgramId,
                                     EndDate                    = s.EndDate,

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
            var student = await _context.Set<Entities.Student>()
                                        .FirstOrDefaultAsync(x => x.Id == id);
            if (student == null)
            {
                throw new NotFoundException("Student not found.");
            }

            var userInfoId = student.UserInfoId;
            
            _context.Remove(student);
            
            await _context.SaveChangesAsync();
            
            await _userInfoService.RemoveAsync(userInfoId);
        }
        
        /// <exception cref="NotFoundException"/>
        public async Task RemoveEducationDocumentAsync(Guid studentId)
        {
            var student = await _context.Set<Entities.Student>()
                                        .Include(x => x.EducationDocument)
                                        .FirstOrDefaultAsync(x => x.Id == studentId);
            if (student == null)
            {
                throw new NotFoundException("Student not found.");
            }
            
            if (student.EducationDocument == null)
            {
                throw new NotFoundException("Student has no education document.");
            }

            _context.Remove(student.EducationDocument);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemoveIdentificationCodeAsync(Guid studentId)
        {
            var student = await _context.Set<Entities.Student>()
                                       .Include(x => x.IdentificationCode)
                                       .FirstOrDefaultAsync(x => x.Id == studentId);
            if (student == null)
            {
                throw new NotFoundException("Student not found.");
            }
            
            if (student.IdentificationCode == null)
            {
                throw new NotFoundException("Student has no identification code.");
            }

            _context.Remove(student.IdentificationCode);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="NotFoundException"/>
        public async Task RemovePassportAsync(Guid studentId)
        {
            var student = await _context.Set<Entities.Student>()
                                        .Include(x => x.Passport)
                                        .FirstOrDefaultAsync(x => x.Id == studentId);
            if (student == null)
            {
                throw new NotFoundException("Student not found.");
            }

            if (student.Passport == null)
            {
                throw new NotFoundException("Student has no passport.");
            }
            
            _context.Remove(student.Passport);
            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task SetEducationDocumentAsync(Guid studentId, Dtos.EducationDocument dto)
        {
            var validator = new Dtos.EducationDocumentValidator();
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

            var student = await _context.Set<Entities.Student>()
                                        .Include(x => x.EducationDocument)
                                        .FirstOrDefaultAsync(x => x.Id == studentId);

            if (student == null)
            {
                throw new NotFoundException("Student not found.");
            }

            if (student.EducationDocument == null)
            {
                student.EducationDocument = new Entities.EducationDocument
                {
                    CreatedAt        = DateTime.UtcNow,
                    DateOfIssue      = dto.DateOfIssue,
                    Id               = Guid.NewGuid(),
                    IssuingAuthority = dto.IssuingAuthority,
                    Number           = dto.Number,
                    Series           = dto.Series,
                    UpdatedAt        = DateTime.UtcNow,
                    Student          = student
                };
            }
            else
            {
                student.EducationDocument.DateOfIssue      = dto.DateOfIssue;
                student.EducationDocument.IssuingAuthority = dto.IssuingAuthority;
                student.EducationDocument.Number           = dto.Number;
                student.EducationDocument.Series           = dto.Series;
                student.EducationDocument.UpdatedAt        = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task SetIdentificationCodeAsync(Guid studentId, Dtos.IdentificationCode dto)
        {
            var validator = new Dtos.IdentificationCodeValidator();
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

            var student = await _context.Set<Entities.Student>()
                                        .Include(x => x.IdentificationCode)
                                        .FirstOrDefaultAsync(x => x.Id == studentId);

            if (student == null)
            {
                throw new NotFoundException("Student not found.");
            }

            if (student.IdentificationCode == null)
            {
                student.IdentificationCode = new Entities.IdentificationCode
                {
                    CreatedAt                          = DateTime.UtcNow,
                    UpdatedAt                          = DateTime.UtcNow,
                    Id                                 = Guid.NewGuid(),
                    IdentificationCodeDateOfIssue      = dto.DateOfIssue,
                    IdentificationCodeIssuingAuthority = dto.IssuingAuthority,
                    IdentificationCodeNumber           = dto.Number,
                    Student                            = student
                };
            }
            else
            {
                student.IdentificationCode.IdentificationCodeDateOfIssue      = dto.DateOfIssue;
                student.IdentificationCode.IdentificationCodeIssuingAuthority = dto.IssuingAuthority;
                student.IdentificationCode.IdentificationCodeNumber           = dto.Number;
                student.IdentificationCode.UpdatedAt                          = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        /// <exception cref="InvalidModelException"/>
        /// <exception cref="NotFoundException"/>
        public async Task SetPassportAsync(Guid studentId, Dtos.Passport dto)
        {
            var validator = new Dtos.PassportValidator();
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

            var student = await _context.Set<Entities.Student>()
                                        .Include(x => x.Passport)
                                        .FirstOrDefaultAsync(x => x.Id == studentId);

            if (student == null)
            {
                throw new NotFoundException("Student not found.");
            }

            if (student.Passport == null)
            {
                student.Passport = new Entities.Passport
                {
                    CreatedAt                = DateTime.UtcNow,
                    UpdatedAt                = DateTime.UtcNow,
                    Id                       = Guid.NewGuid(),
                    MaritalStatus            = dto.MaritalStatus,
                    Nationality              = dto.Nationality,
                    PassportDateOfIssue      = dto.PassportDateOfIssue,
                    PassportIssuingAuthority = dto.PassportIssuingAuthority,
                    PassportNumber           = dto.PassportNumber,
                    PassportSeries           = dto.PassportSeries,
                    PlaceOfBirth             = dto.PlaceOfBirth,
                    RegistrationAddress      = dto.RegistrationAddress,
                    Student                  = student
                };
            }
            else
            {
                student.Passport.MaritalStatus            = dto.MaritalStatus;
                student.Passport.Nationality              = dto.Nationality;
                student.Passport.PassportDateOfIssue      = dto.PassportDateOfIssue;
                student.Passport.PassportIssuingAuthority = dto.PassportIssuingAuthority;
                student.Passport.PassportNumber           = dto.PassportNumber;
                student.Passport.PassportSeries           = dto.PassportSeries;
                student.Passport.PlaceOfBirth             = dto.PlaceOfBirth;
                student.Passport.RegistrationAddress      = dto.RegistrationAddress;
                student.Passport.UpdatedAt                = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        /// <exception cref="ServiceException"/>
        /// <exception cref="NotFoundException"/>
        public async Task UpdateAsync(Dtos.Student dto)
        {
            if (dto.Id.Equals(Guid.Empty))
            {
                throw new InvalidModelException("Property Id failed validation. Error was: Id is empty");
            }

            var validator = new Dtos.StudentValidator();
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

            var student = await _context.Set<Entities.Student>()
                                        .Include(x => x.UserInfo)
                                        .FirstOrDefaultAsync(t => t.Id == dto.Id);

            if (student == null)
            {
                throw new NotFoundException($"Student with id: {dto.Id} not found.");
            }

            if (!await _context.Set<Entities.AcademicGroup>().AnyAsync(d => d.Id == dto.AcademicGroupId))
            {
                throw new InvalidModelException($"Academic group with id: {dto.AcademicGroupId} not found");
            }

            if (dto.EducationProgramId != null && !await _context.Set<Entities.EducationProgram>().AnyAsync(d => d.Id == dto.EducationProgramId))
            {
                throw new InvalidModelException($"Education program with id: {dto.EducationProgramId} not found");
            }

            if (!await _context.Set<Entities.EducationLevel>().AnyAsync(d => d.Id == dto.EducationLevelId))
            {
                throw new InvalidModelException($"Education level with id: {dto.EducationLevelId} not found");
            }

            if (!await _context.Set<Entities.FormOfEducation>().AnyAsync(d => d.Id == dto.FormOfEducationId))
            {
                throw new InvalidModelException($"Form of education with id: {dto.FormOfEducationId} not found");
            }

            if (dto.PrivilegeId != null && !await _context.Set<Entities.Privilege>().AnyAsync(d => d.Id == dto.PrivilegeId))
            {
                throw new InvalidModelException($"Privilege with id: {dto.PrivilegeId} not found");
            }

            student.CreatedAt                  = dto.CreatedAt;
            student.Id                         = dto.Id;
            student.UpdatedAt                  = dto.UpdatedAt;
            student.StudentTicketNumber        = dto.StudentTicketNumber;
            student.Sex                        = dto.Sex;
            student.PrivilegeId                = dto.PrivilegeId;
            student.NumberOfRecordBook         = dto.NumberOfRecordBook;
            student.MilitaryRegistration       = dto.MilitaryRegistration;
            student.ForeignLanguage            = dto.ForeignLanguage;
            student.Financing                  = dto.Financing;
            student.FormOfEducationId          = dto.FormOfEducationId;
            student.EntryDate                  = dto.EntryDate;
            student.AcademicGroupId            = dto.AcademicGroupId;
            student.AcceleratedFormOfEducation = dto.AcceleratedFormOfEducation;
            student.AddressOfResidence         = dto.AddressOfResidence;
            student.Chummery                   = dto.Chummery;
            student.EducationLevelId           = dto.EducationLevelId;
            student.EducationProgramId         = dto.EducationProgramId;
            student.EndDate                    = dto.EndDate;
            
            student.UserInfo.DateOfBirth       = dto.DateOfBirth;
            student.UserInfo.Email             = dto.Email;
            student.UserInfo.FirstName         = dto.FirstName;
            student.UserInfo.FirstNameEng      = dto.FirstNameEng;
            student.UserInfo.LastName          = dto.LastName;
            student.UserInfo.LastNameEng       = dto.LastNameEng;
            student.UserInfo.Patronymic        = dto.Patronymic;
            student.UserInfo.PhoneNumber       = dto.PhoneNumber;
            student.UpdatedAt                  = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
