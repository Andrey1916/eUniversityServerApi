using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class EducationDocument
    {
        public string Series { get; set; }

        public long? Number { get; set; }

        public string IssuingAuthority { get; set; }

        public DateTime? DateOfIssue { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class EducationDocumentValidator : AbstractValidator<EducationDocument>
    {
        public EducationDocumentValidator()
        {
            this.RuleFor(x => x.IssuingAuthority).NotEmpty()
                                                 .MaximumLength(512);

            this.RuleFor(x => x.Number).GreaterThanOrEqualTo(0);

            this.RuleFor(x => x.Series).NotEmpty();
        }
    }
}
