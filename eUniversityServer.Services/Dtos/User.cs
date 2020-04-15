using FluentValidation;
using System;
using System.Collections.Generic;

namespace eUniversityServer.Services.Dtos
{
    public class User : UserInfo
    {
        public IEnumerable<Dtos.Role> Roles { get; set; }
    }

    public class UserWithToken : User
    {
        public TokenInfo TokenInfo { get; set; }
    }

    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
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
