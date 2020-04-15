using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Text;

using Entities = eUniversityServer.DAL.Entities;

namespace eUniversityServer.Services.Utils
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(IOptions<SieveOptions> options, ISieveCustomFilterMethods sieveCustomFilterMethods) : base(options, sieveCustomFilterMethods)
        { }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            this.AcademicDisciplineConfiguration(mapper);
            this.AcademicGroupConfiguration(mapper);
            this.CurriculumConfiguration(mapper);
            this.DepartmentConfiguration(mapper);
            this.EducationDocumentConfiguration(mapper);
            this.EducationLevelConfiguration(mapper);
            this.EducationProgramConfiguration(mapper);
            this.ExamsGradesSpreadsheetConfiguration(mapper);
            this.FormOfEducationConfiguration(mapper);
            this.IdentificationCodeConfiguration(mapper);
            this.LogConfiguration(mapper);
            this.PassportConfiguration(mapper);
            this.RatingForDisciplineConfiguration(mapper);
            this.RoleConfiguration(mapper);
            this.SpecialtyConfiguration(mapper);
            this.StructuralUnitConfiguration(mapper);
            this.StudentConfiguration(mapper);
            this.TeacherConfiguration(mapper);
            this.UserConfiguration(mapper);

            return mapper;
        }

        private void AcademicDisciplineConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.AcademicDiscipline>(p => p.DepartmentId)
                  .CanFilter();

            mapper.Property<Entities.AcademicDiscipline>(p => p.FullName)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.AcademicDiscipline>(p => p.NumberOfCredits)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.AcademicDiscipline>(p => p.Semester)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.AcademicDiscipline>(p => p.ShortName)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.AcademicDiscipline>(p => p.TypeOfIndividualWork)
                .CanFilter()
                .CanSort();
        }

        private void AcademicGroupConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.AcademicGroup>(p => p.Captain)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.AcademicGroup>(p => p.Code)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.AcademicGroup>(p => p.Curator)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.AcademicGroup>(p => p.Grade)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.AcademicGroup>(p => p.Number)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.AcademicGroup>(p => p.UIN)
                .CanFilter()
                .CanSort();
        }

        private void CurriculumConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.Curriculum>(p => p.DateOfApproval)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Curriculum>(p => p.ListOfApprovals)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Curriculum>(p => p.OrderOfApprovals)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Curriculum>(p => p.ProtocolOfAcademicCouncilOfUnit)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Curriculum>(p => p.ProtocolOfAcademicCouncilOfUniversity)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Curriculum>(p => p.ScheduleOfEducationProcess)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Curriculum>(p => p.SpecialtyGuarantor)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Curriculum>(p => p.YearOfAdmission)
                .CanFilter()
                .CanSort();
        }

        private void DepartmentConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.Department>(p => p.FullName)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Department>(p => p.Chief)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Department>(p => p.Code)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Department>(p => p.FullNameEng)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Department>(p => p.ShortName)
                .CanFilter()
                .CanSort();
        }

        private void EducationDocumentConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.EducationDocument>(p => p.IssuingAuthority)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.EducationDocument>(p => p.Number)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.EducationDocument>(p => p.Series)
                .CanFilter()
                .CanSort();
        }

        private void EducationLevelConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.EducationLevel>(p => p.Name)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.EducationLevel>(p => p.Number)
                .CanFilter()
                .CanSort();
        }

        private void EducationProgramConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.EducationProgram>(p => p.ApprovalYear)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.EducationProgram>(p => p.DurationbOfEducation)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.EducationProgram>(p => p.Guarantor)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.EducationProgram>(p => p.Language)
                .CanFilter()
                .CanSort();
        }

        private void ExamsGradesSpreadsheetConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.ExamsGradesSpreadsheet>(p => p.ExamDate)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.ExamsGradesSpreadsheet>(p => p.ExamsSpreadsheetAttestationType)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.ExamsGradesSpreadsheet>(p => p.ExamsSpreadsheetType)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.ExamsGradesSpreadsheet>(p => p.SemesterNumber)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.ExamsGradesSpreadsheet>(p => p.SpreadsheetNumber)
                .CanFilter()
                .CanSort();
        }

        private void FormOfEducationConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.FormOfEducation>(p => p.Name)
                .CanFilter()
                .CanSort();
        }

        private void IdentificationCodeConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.IdentificationCode>(p => p.IdentificationCodeDateOfIssue)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.IdentificationCode>(p => p.IdentificationCodeIssuingAuthority)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.IdentificationCode>(p => p.IdentificationCodeNumber)
                .CanFilter()
                .CanSort();
        }

        private void LogConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.Log>(p => p.DateTime)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Log>(p => p.LogLevel)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Log>(p => p.Message)
                .CanFilter()
                .CanSort();
        }

        private void PassportConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.Passport>(p => p.MaritalStatus)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Passport>(p => p.Nationality)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Passport>(p => p.PassportDateOfIssue)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Passport>(p => p.PassportIssuingAuthority)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Passport>(p => p.PassportNumber)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Passport>(p => p.PassportSeries)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Passport>(p => p.PlaceOfBirth)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Passport>(p => p.RegistrationAddress)
                .CanFilter()
                .CanSort();
        }

        private void RatingForDisciplineConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.RatingForDiscipline>(p => p.Date)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.RatingForDiscipline>(p => p.Score)
                .CanFilter()
                .CanSort();
        }
        
        private void RoleConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.Role>(p => p.Name)
                .CanFilter()
                .CanSort();
        }

        private void SpecialtyConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.Specialty>(p => p.Code)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Specialty>(p => p.Discipline)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Specialty>(p => p.GroupsCode)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Specialty>(p => p.Name)
                .CanFilter()
                .CanSort();
        }

        private void StructuralUnitConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.StructuralUnit>(p => p.Chief)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.StructuralUnit>(p => p.Code)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.StructuralUnit>(p => p.FullName)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.StructuralUnit>(p => p.FullNameEng)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.StructuralUnit>(p => p.ShortName)
                .CanFilter()
                .CanSort();
        }

        private void StudentConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.Student>(p => p.AcceleratedFormOfEducation)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.AddressOfResidence)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.Chummery)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.EndDate)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.EntryDate)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.Financing)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.ForeignLanguage)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.MilitaryRegistration)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.NumberOfRecordBook)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.Sex)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.StudentTicketNumber)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.Privilege.Name)
                .CanFilter()
                .CanSort();

            //===UserInfo==========

            mapper.Property<Entities.Student>(p => p.UserInfo.DateOfBirth)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.UserInfo.Email)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.UserInfo.EmailConfirmed)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.UserInfo.FirstName)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.UserInfo.FirstNameEng)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.UserInfo.LastName)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.UserInfo.LastNameEng)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.UserInfo.Patronymic)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Student>(p => p.UserInfo.PhoneNumber)
                .CanFilter()
                .CanSort();
        }

        private void TeacherConfiguration(SievePropertyMapper mapper)
        {
            mapper.Property<Entities.Teacher>(p => p.AcademicRank)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Teacher>(p => p.Position)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Teacher>(p => p.ScientificDegree)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Teacher>(p => p.TypeOfEmployment)
                .CanFilter()
                .CanSort();

            //===UserInfo==========

            mapper.Property<Entities.Teacher>(p => p.UserInfo.DateOfBirth)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Teacher>(p => p.UserInfo.Email)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Teacher>(p => p.UserInfo.EmailConfirmed)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Teacher>(p => p.UserInfo.FirstName)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Teacher>(p => p.UserInfo.FirstNameEng)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Teacher>(p => p.UserInfo.LastName)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Teacher>(p => p.UserInfo.LastNameEng)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Teacher>(p => p.UserInfo.Patronymic)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.Teacher>(p => p.UserInfo.PhoneNumber)
                .CanFilter()
                .CanSort();
        }

        private void UserConfiguration(SievePropertyMapper mapper)
        {
            //===UserInfo==========
            mapper.Property<Entities.User>(p => p.UserInfo.DateOfBirth)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.User>(p => p.UserInfo.Email)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.User>(p => p.UserInfo.EmailConfirmed)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.User>(p => p.UserInfo.FirstName)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.User>(p => p.UserInfo.FirstNameEng)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.User>(p => p.UserInfo.LastName)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.User>(p => p.UserInfo.LastNameEng)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.User>(p => p.UserInfo.Patronymic)
                .CanFilter()
                .CanSort();

            mapper.Property<Entities.User>(p => p.UserInfo.PhoneNumber)
                .CanFilter()
                .CanSort();
        }
    }
}
