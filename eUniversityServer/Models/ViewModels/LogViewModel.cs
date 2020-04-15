using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Models.ViewModels
{
    public class LogViewModel
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public string Message { get; set; }

        public string LogLevel { get; set; }

        public string StackTrace { get; set; }

        public DateTime DateTime { get; set; }
    }
}
