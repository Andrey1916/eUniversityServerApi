using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace eUniversityServer.DAL.Entities
{
    public class RolePermissions
    {
        public Guid RoleId { get; set; }

        public Role Role { get; set; }

        public Guid PermissionId { get; set; }

        public Permission Permission { get; set; }
    }
}
