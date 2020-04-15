using eUniversityServer.DAL.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class AcademicDiscipline
    {
        public Guid Id { get; set; }

        public Guid SpecialtyId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid CurriculumId { get; set; }

        public Guid LecturerId { get; set; }

        public Guid? AssistantId { get; set; }

        public string FullName { get; set; }

        public string ShortName { get; set; }

        public SemesterType Semester { get; set; }

        public int NumberOfCredits { get; set; }

        public AttestationType Attestation { get; set; }

        public IndividualWork TypeOfIndividualWork { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class AcademicDisciplineInfo : AcademicDiscipline
    {
        public string SpecialtyName { get; set; }

        public string DepartmentName { get; set; }

        public string LecturerName { get; set; }

        public string AssistantName { get; set; }
    }

    public class AcademicDisciplineValidator : AbstractValidator<AcademicDiscipline>
    {
        public AcademicDisciplineValidator()
        {
            this.RuleFor(x => x.FullName).MaximumLength(512)
                                         .NotEmpty();

            this.RuleFor(x => x.ShortName).MaximumLength(512)
                                          .NotEmpty();

            this.RuleFor(x => x.NumberOfCredits).NotEmpty();
        }
    }
}
