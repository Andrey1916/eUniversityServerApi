using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class FormOfEducation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class FormOfEducationValidator : AbstractValidator<FormOfEducation>
    {
        public FormOfEducationValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty()
                                     .MaximumLength(512);
        }
    }
}
