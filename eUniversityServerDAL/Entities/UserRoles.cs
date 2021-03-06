﻿using System;

namespace eUniversityServer.DAL.Entities
{
    public class UserRoles
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid RoleId { get; set; }

        public Role Role { get; set; }
    }
}
