using eUniversityServer.DAL;
using eUniversityServer.DAL.Entities;
using eUniversityServer.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Logger
{
    public class DbLogger : Logger
    {
        private readonly DbContext context;

        public DbLogger(DbContext context)
        {
            this.context = context ?? throw new NullReferenceException(nameof(context));
        }

        protected override async Task WriteLog(DateTime dateTime, Level level, string message = "", string stackTrace = "")
        {
            var log = new Log
            {
                DateTime = dateTime,
                Id = Guid.NewGuid(),
                LogLevel = level               
            };

            if (!string.IsNullOrWhiteSpace(message))
            {
                log.Message = message;
            }

            if (!string.IsNullOrWhiteSpace(stackTrace))
            {
                log.StackTrace = stackTrace;
            }

            await context.Set<Log>().AddAsync(log);
            await context.SaveChangesAsync();
        }

        protected override async Task WriteLog(DateTime dateTime, Level level, Guid userId, string message = "", string stackTrace = "")
        {
            var log = new Log
            {
                DateTime = dateTime,
                Id = Guid.NewGuid(),
                LogLevel = level,
                UserId = userId
            };

            if (!string.IsNullOrWhiteSpace(message))
            {
                log.Message = message;
            }

            if (!string.IsNullOrWhiteSpace(stackTrace))
            {
                log.StackTrace = stackTrace;
            }

            await context.Set<Log>().AddAsync(log);
            await context.SaveChangesAsync();
        }
    }
}
