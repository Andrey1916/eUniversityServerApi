using eUniversityServer.DAL.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class Teacher : UserInfo
    {
        public Guid DepartmentId { get; set; }

        public string Position { get; set; }

        public string ScientificDegree { get; set; }

        public string AcademicRank { get; set; }

        public Employment TypeOfEmployment { get; set; }
    }

    public class TeacherInfo : Teacher
    {
        public string DepartmentName { get; set; }
    }

    public class TeacherValidator : AbstractValidator<Teacher>
    {
        public TeacherValidator()
        {
            this.RuleFor(x => x.Position).MaximumLength(256);

            this.RuleFor(x => x.ScientificDegree).MaximumLength(256);

            this.RuleFor(x => x.AcademicRank).MaximumLength(256);


            this.RuleFor(x => x.FirstName).MaximumLength(256)
                                             .NotEmpty();

            this.RuleFor(x => x.LastName).MaximumLength(256)
                                             .NotEmpty();

            this.RuleFor(x => x.Patronymic).MaximumLength(256);

            this.RuleFor(x => x.FirstNameEng).MaximumLength(256)
                                             .NotEmpty();

            this.RuleFor(x => x.LastNameEng).MaximumLength(256)
                                             .NotEmpty();

            this.RuleFor(x => x.PhoneNumber).MaximumLength(16);

            this.RuleFor(x => x.Email).MaximumLength(512);

        }
    }
}
