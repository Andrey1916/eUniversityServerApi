using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Dtos
{
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public IEnumerable<Permission> Permissions { get; set; }
    }

    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty()
                                     .MaximumLength(96);
        }
    }
}
