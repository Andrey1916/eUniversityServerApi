using eUniversityServer.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eUniversityServer.Services.Dtos
{
    public class Log
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public string Message { get; set; }

        public string LogLevel { get; set; }

        public string StackTrace { get; set; }

        public DateTime DateTime { get; set; }
    }
}
