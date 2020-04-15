using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class Specialty
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string GroupsCode { get; set; }

        public string Discipline { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class SpecialtyValidator : AbstractValidator<Specialty>
    {
        public SpecialtyValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty()
                                     .MaximumLength(512);

            this.RuleFor(x => x.Code).NotEmpty()
                                     .MaximumLength(32);

            this.RuleFor(x => x.GroupsCode).NotEmpty()
                                           .MaximumLength(16);

            this.RuleFor(x => x.Discipline).MaximumLength(512);
        }
    }
}
