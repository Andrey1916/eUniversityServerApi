using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class UserInfo
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public string FirstNameEng { get; set; }

        public string LastNameEng { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class UserInfoValidator : AbstractValidator<UserInfo>
    {
        public UserInfoValidator()
        {
            this.RuleFor(x => x.FirstName).MaximumLength(256)
                                             .NotEmpty();

            this.RuleFor(x => x.LastName).MaximumLength(256)
                                             .NotEmpty();

            this.RuleFor(x => x.Patronymic).MaximumLength(256)
                                             .NotEmpty();

            this.RuleFor(x => x.FirstNameEng).MaximumLength(256)
                                             .NotEmpty();

            this.RuleFor(x => x.LastNameEng).MaximumLength(256)
                                             .NotEmpty();

            this.RuleFor(x => x.PhoneNumber).MaximumLength(16);

            this.RuleFor(x => x.Email).MaximumLength(512);

        }
    }
}
