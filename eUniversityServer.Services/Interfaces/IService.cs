using eUniversityServer.Services.Models;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Interfaces
{
    public interface IService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(int page, int size);

        Task<SieveResult<T>> GetSomeAsync(SieveModel model);

        Task<T> GetByIdAsync(Guid id);

        Task<Guid> AddAsync(T dto);

        Task UpdateAsync(T dto);

        Task RemoveAsync(Guid id);
    }
}
