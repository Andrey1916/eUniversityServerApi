using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class Passport
    {
        public string PassportSeries { get; set; }

        public long? PassportNumber { get; set; }

        public string PassportIssuingAuthority { get; set; }

        public DateTime? PassportDateOfIssue { get; set; }

        public string Nationality { get; set; }

        public string RegistrationAddress { get; set; }

        public string MaritalStatus { get; set; }

        public string PlaceOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class PassportValidator : AbstractValidator<Passport>
    {
        public PassportValidator()
        {
            this.RuleFor(x => x.PassportSeries).NotEmpty()
                                               .MaximumLength(2);

            this.RuleFor(x => x.PassportNumber).GreaterThanOrEqualTo(0);

            this.RuleFor(x => x.PassportIssuingAuthority).NotEmpty()
                                                         .MaximumLength(512);
        }
    }
}
