using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class Curriculum
    {
        public Guid Id { get; set; }

        public Guid SpecialtyId { get; set; }

        public Guid StructuralUnitId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid FormOfEducationId { get; set; }

        public Guid? EducationProgramId { get; set; }

        public Guid EducationLevelId { get; set; }

        public string ShortName { get; set; }

        public int? YearOfAdmission { get; set; }

        public DateTime? DateOfApproval { get; set; }

        public string ScheduleOfEducationProcess { get; set; }

        public string ListOfApprovals { get; set; }

        public string OrderOfApprovals { get; set; }

        public string ProtocolOfAcademicCouncilOfUnit { get; set; }

        public string ProtocolOfAcademicCouncilOfUniversity { get; set; }

        public string SpecialtyGuarantor { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class CurriculumInfo : Curriculum
    {
        public string SpecialtyName { get; set; }

        public string StructuralUnitName { get; set; }

        public string DepartmentName { get; set; }

        public string FormOfEducation { get; set; }

        public string EducationLevel { get; set; }
    }

    public class CurriculumValidator : AbstractValidator<Curriculum>
    {
        public CurriculumValidator()
        {
            this.RuleFor(x => x.SpecialtyGuarantor).MaximumLength(512);
        }
    }
}
