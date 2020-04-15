using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class EducationLevel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class EducationLevelValidator : AbstractValidator<EducationLevel>
    {
        public EducationLevelValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty()
                                     .MaximumLength(512);

            this.RuleFor(x => x.Number).GreaterThanOrEqualTo(0);
        }
    }
}
