using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using eUniversityServer.DAL.Enums;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Logger
{
    public abstract class Logger
    {
        protected abstract Task WriteLog(DateTime dateTime, Level level, string message = "", string stackTrace = "");
        protected abstract Task WriteLog(DateTime dateTime, Level level, Guid userId, string message = "", string stackTrace = "");

        private async Task WriteLog(Level level, Guid userId, string message, bool showStackTrace)
        {
            var now = DateTime.UtcNow;
            string stackTrace = "";

            if (showStackTrace)
            {
                stackTrace = GetStackTrace();
            }

            await WriteLog(now, level, userId, message, stackTrace);
        }

        private async Task WriteLog(Level level, string message, bool showStackTrace)
        {
            var now = DateTime.UtcNow;
            string stackTrace = "";

            if (showStackTrace)
            {
                stackTrace = GetStackTrace();
            }

            await WriteLog(now, level, message, stackTrace);
        }

        #region Levels

        public async Task Trace(string message, bool showStackTrace = false)
        {
            await WriteLog(Level.Trace, message, showStackTrace);
        }

        public async Task Trace(string message, Guid userId, bool showStackTrace = false)
        {
            await WriteLog(Level.Trace, userId, message, showStackTrace);
        }


        public async Task Debug(string message, bool showStackTrace = false)
        {
            await WriteLog(Level.Debug, message, showStackTrace);
        }

        public async Task Debug(string message, Guid userId, bool showStackTrace = false)
        {
            await WriteLog(Level.Debug, userId, message, showStackTrace);
        }


        public async Task Info(string message)
        {
            await WriteLog(Level.Info, message, false);
        }

        public async Task Info(string message, Guid userId)
        {
            await WriteLog(Level.Info, userId, message, false);
        }


        public async Task Warning(string message, bool showStackTrace = false)
        {
            await WriteLog(Level.Warning, message, showStackTrace);
        }

        public async Task Warning(string message, Guid userId, bool showStackTrace = false)
        {
            await WriteLog(Level.Warning, userId, message, showStackTrace);
        }


        public async Task Error(string message, bool showStackTrace = false)
        {
            await WriteLog(Level.Error, message, showStackTrace);
        }

        public async Task Error(string message, Guid userId, bool showStackTrace = false)
        {
            await WriteLog(Level.Error, userId, message, showStackTrace);
        }


        public async Task Fatal(string message, bool showStackTrace = false)
        {
            await WriteLog(Level.Fatal, message, showStackTrace);
        }

        public async Task Fatal(string message, Guid userId, bool showStackTrace = false)
        {
            await WriteLog(Level.Fatal, userId, message, showStackTrace);
        }

        #endregion

        private string GetStackTrace() => new StackTrace(2, true).ToString();

    }
}
