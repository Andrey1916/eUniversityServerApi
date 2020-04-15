using eUniversityServer.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Interfaces
{
    public interface ISpecialtyService : IService<Specialty>
    {
        Task<IEnumerable<Dtos.Student>> GetStudentsAsync(Guid groupId);
    }
}
