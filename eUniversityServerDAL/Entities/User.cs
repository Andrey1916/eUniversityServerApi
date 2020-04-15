using eUniversityServer.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace eUniversityServer.DAL.Entities
{
    public class User : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }

        public ICollection<Log> Logs { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
    }
}
