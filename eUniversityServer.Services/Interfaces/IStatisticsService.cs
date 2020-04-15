using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eUniversityServer.Services.Models.Statistics;

namespace eUniversityServer.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<IEnumerable<StudentScoreForDiscipline>> GetLatestScoresForDisciplineAsync(Guid disciplineId, Guid groupId);
        Task<IEnumerable<SpecialtyStudentsDispersion>> GetStudentsDispersionBySpecialtyAsync(Guid specialtyId);
    }
}