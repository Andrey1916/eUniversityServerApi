using eUniversityServer.DAL.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class Student : UserInfo
    {
        public Guid? PrivilegeId { get; set; }

        public Guid AcademicGroupId { get; set; }

        public Guid? EducationProgramId { get; set; }

        public Guid FormOfEducationId { get; set; }

        public Guid EducationLevelId { get; set; }

        public SexType Sex { get; set; }

        public long StudentTicketNumber { get; set; }

        public Financing Financing { get; set; }

        public string AddressOfResidence { get; set; }

        public int NumberOfRecordBook { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ForeignLanguage { get; set; }

        public bool? MilitaryRegistration { get; set; }

        public string Chummery { get; set; }

        public bool AcceleratedFormOfEducation { get; set; }
    }

    public class StudentInfo : Student
    {
        public string AcademicGroupCode { get; set; }

        public string FormOfEducation { get; set; }

        public string EducationLevel { get; set; }
    }

    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            this.RuleFor(x => x.AddressOfResidence).MaximumLength(512);

            this.RuleFor(x => x.StudentTicketNumber).GreaterThanOrEqualTo(0);

            this.RuleFor(x => x.NumberOfRecordBook).GreaterThanOrEqualTo(0);

            this.RuleFor(x => x.ForeignLanguage).MaximumLength(512);

            this.RuleFor(x => x.Chummery).MaximumLength(512);
        }
    }
}
