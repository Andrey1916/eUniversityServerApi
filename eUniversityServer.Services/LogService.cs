using eUniversityServer.Services.Dtos;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Models;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities = eUniversityServer.DAL.Entities;

namespace eUniversityServer.Services
{
    public class LogService : ILogService
    {
        private readonly DbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public LogService(DbContext context, SieveProcessor sieveProcessor)
        {
            this._context        = context ?? throw new NullReferenceException(nameof(context));
            this._sieveProcessor = sieveProcessor ?? throw new NullReferenceException(nameof(sieveProcessor));
        }

        public async Task<IEnumerable<Log>> GetAllAsync()
        {
            return await _context.Set<Entities.Log>()
                                 .AsNoTracking()
                                 .Select(log => new Dtos.Log
                                 {
                                     DateTime   = log.DateTime,
                                     Id         = log.Id,
                                     LogLevel   = log.LogLevel.ToString(),
                                     Message    = log.Message,
                                     StackTrace = log.StackTrace,
                                     UserId     = log.UserId
                                 })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Entities.Log>()
                                 .OrderBy(log => log.DateTime)
                                 .Skip(page * size)
                                 .Take(size)
                                 .AsNoTracking()
                                 .Select(log => new Dtos.Log
                                 {
                                     DateTime   = log.DateTime,
                                     Id         = log.Id,
                                     LogLevel   = log.LogLevel.ToString(),
                                     Message    = log.Message,
                                     StackTrace = log.StackTrace,
                                     UserId     = log.UserId
                                 })
                                 .ToListAsync();
        }

        public async Task<Log> GetByIdAsync(Guid id)
        {
            var log = await _context.Set<Entities.Log>()
                                    .FindAsync(id);

            if (log == null)
                return null;

            return new Dtos.Log
            {
                DateTime   = log.DateTime,
                Id         = log.Id,
                LogLevel   = log.LogLevel.ToString(),
                Message    = log.Message,
                StackTrace = log.StackTrace,
                UserId     = log.UserId
            };
        }

        public async Task<SieveResult<Log>> GetSomeAsync(SieveModel model)
        {
            if (model == null)
                return null;

            var logsQuery = _context.Set<Entities.Log>().AsNoTracking();

            logsQuery = _sieveProcessor.Apply(model, logsQuery, applyPagination: false);

            var result = new SieveResult<Log>();
            result.TotalCount = await logsQuery.CountAsync();

            var someLogs = await _sieveProcessor.Apply(model, logsQuery, applyFiltering: false, applySorting: false).ToListAsync();

            result.Result = someLogs.Select(log => new Dtos.Log
            {
                DateTime   = log.DateTime,
                Id         = log.Id,
                LogLevel   = log.LogLevel.ToString(),
                Message    = log.Message,
                StackTrace = log.StackTrace,
                UserId     = log.UserId
            });

            return result;
        }
    }
}