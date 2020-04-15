using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class StructuralUnit
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string FullName { get; set; }

        public string FullNameEng { get; set; }

        public string ShortName { get; set; }

        public string Chief { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class StructuralUnitValidator : AbstractValidator<StructuralUnit>
    {
        public StructuralUnitValidator()
        {
            this.RuleFor(x => x.Code).MaximumLength(16);

            this.RuleFor(x => x.FullName).NotEmpty()
                                         .MaximumLength(512);

            this.RuleFor(x => x.FullNameEng).NotEmpty()
                                            .MaximumLength(512);

            this.RuleFor(x => x.ShortName).MaximumLength(512);

            this.RuleFor(x => x.Chief).MaximumLength(512);
        }
    }
}
