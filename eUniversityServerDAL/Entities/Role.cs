using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace eUniversityServer.DAL.Entities
{
    public class Role : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public ICollection<UserRoles> UserRoles { get; set; }

        public ICollection<RolePermissions> RolePermissions { get; set; }
    }
}
