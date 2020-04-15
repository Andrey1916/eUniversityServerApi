using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class EducationProgram
    {
        public Guid Id { get; set; }

        public Guid SpecialtyId { get; set; }

        public Guid EducationLevelId { get; set; }

        public string ShortName { get; set; }

        public short? DurationOfEducation { get; set; }

        public string Language { get; set; }

        public DateTime? ApprovalYear { get; set; }

        public string Guarantor { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class EducationProgramInfo : EducationProgram
    {
        public string SpecialtyName { get; set; }

        public string EducationLevel { get; set; }
    }

    public class EducationProgramValidator : AbstractValidator<EducationProgram>
    {
        public EducationProgramValidator()
        {
            this.RuleFor(x => x.DurationOfEducation).GreaterThan((short)0);

            this.RuleFor(x => x.Language).MaximumLength(32);

            this.RuleFor(x => x.Guarantor).MaximumLength(512);
        }
    }
}
