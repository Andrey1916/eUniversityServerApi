using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Interfaces
{
    public interface IAcademicGroupService : IServiceMoreInfo<Dtos.AcademicGroup, Dtos.AcademicGroupInfo>
    {
        Task<IEnumerable<Dtos.Student>> GetStudentsAsync(Guid groupId);
    }
}
