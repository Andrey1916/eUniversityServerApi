using eUniversityServer.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eUniversityServer.Services.Dtos
{
    public class Permission
    {
        public Guid Id { get; set; }

        public TargetModifier TargetModifier { get; set; }

        public AccessModifier AccessModifier { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
