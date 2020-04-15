using eUniversityServer.Services.Models;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Interfaces
{
    public interface IServiceMoreInfo<TDto, TDtoInfo> : IService<TDto> where TDtoInfo : TDto
    {
        Task<IEnumerable<TDtoInfo>> GetAllWithInfoAsync();

        Task<IEnumerable<TDtoInfo>> GetAllWithInfoAsync(int page, int size);

        Task<SieveResult<TDtoInfo>> GetSomeWithInfoAsync(SieveModel model);
    }
}
