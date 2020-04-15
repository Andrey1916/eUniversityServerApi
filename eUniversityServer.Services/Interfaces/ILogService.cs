using eUniversityServer.Services.Dtos;
using eUniversityServer.Services.Models;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Interfaces
{
    public interface ILogService
    {
        Task<IEnumerable<Log>> GetAllAsync();

        Task<IEnumerable<Log>> GetAllAsync(int page, int size);

        Task<SieveResult<Log>> GetSomeAsync(SieveModel model);

        Task<Log> GetByIdAsync(Guid id);
    }
}
