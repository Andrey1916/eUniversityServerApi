using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class IdentificationCode
    {
        public long? Number { get; set; }

        public string IssuingAuthority { get; set; }

        public DateTime? DateOfIssue { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class IdentificationCodeValidator : AbstractValidator<IdentificationCode>
    {
        public IdentificationCodeValidator()
        {
            this.RuleFor(x => x.Number).GreaterThan(0);

            this.RuleFor(x => x.IssuingAuthority).NotEmpty()
                                                 .MaximumLength(512);
        }
    }
}
