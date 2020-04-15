using eUniversityServer.DAL.Enums;
using eUniversityServer.DAL.Interfaces;
using System;

namespace eUniversityServer.DAL.Entities
{
    public class Log : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }
        public User User { get; set; }

        public string Message { get; set; }

        public Level LogLevel { get; set; }

        public string StackTrace { get; set; }

        public DateTime DateTime { get; set; }
    }
}
