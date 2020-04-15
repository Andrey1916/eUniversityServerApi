using eUniversityServer.DAL.Enums;
using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace eUniversityServer.DAL.Entities
{
    public class Permission : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public TargetModifier TargetModifier { get; set; }

        public AccessModifier AccessModifier { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public ICollection<RolePermissions> RolePermissions { get; set; }
    }
}