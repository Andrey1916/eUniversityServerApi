using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Models.Statistics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities = eUniversityServer.DAL.Entities;

namespace eUniversityServer.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly DbContext _context;

        public StatisticsService(DbContext context)
        {
            _context = context ?? throw new NullReferenceException(nameof(context));
        }


        public async Task<IEnumerable<SpecialtyStudentsDispersion>> GetStudentsDispersionBySpecialtyAsync(Guid specialtyId)
        {
            var group = await _context.Set<Entities.Specialty>().AnyAsync(g => g.Id == specialtyId);
            if (!group)
            {
                throw new NotFoundException("Specialty not found");
            }

            int since = DateTime.UtcNow.Year - 10;
            var data = await _context.Set<Entities.Student>()
                                     .Include(x => x.AcademicGroup)
                                     .Where(s => s.AcademicGroup != null && s.AcademicGroup.SpecialtyId == specialtyId && s.EntryDate.Year >= since)
                                     .Select(x => new
                                     {
                                         x.NumberOfRecordBook,
                                         x.EntryDate,
                                         x.EndDate,
                                         x.Sex
                                     })
                                     .AsNoTracking()
                                     .GroupBy(x => new { x.EntryDate, x.Sex })
                                     .GroupBy(x => x.Key.EntryDate)
                                     .ToListAsync();

            return data.Select(x => new SpecialtyStudentsDispersion
            {
                Year = x.Key.Year,
                SpecialtyId = specialtyId,
                CountOfFemales = x.Count(s => s.Key.Sex == DAL.Enums.SexType.Female),
                CountOfMales = x.Count(s => s.Key.Sex == DAL.Enums.SexType.Male)
            });
        }

        public async Task<IEnumerable<StudentScoreForDiscipline>> GetLatestScoresForDisciplineAsync(Guid disciplineId, Guid groupId)
        {
            var discipline = await _context.Set<Entities.AcademicDiscipline>().AnyAsync(g => g.Id == disciplineId);
            if (!discipline)
            {
                throw new NotFoundException("Discipline not found");
            }

            var group = await _context.Set<Entities.AcademicGroup>().AnyAsync(g => g.Id == groupId);
            if (!discipline)
            {
                throw new NotFoundException("Academic group not found");
            }

            var latestSemester = await _context.Set<Entities.RatingForDiscipline>()
                                               .Include(x => x.ExamsGradesSpreadsheet)
                                               .Where(x => x.AcademicGroupId == groupId)
                                               .OrderByDescending(x => x.ExamsGradesSpreadsheet.SemesterNumber)
                                               .AsNoTracking()
                                               .Select(x => x.ExamsGradesSpreadsheet.SemesterNumber)
                                               .FirstOrDefaultAsync();

            if (latestSemester == 0)
                return default;

            var averageScore = await _context.Set<Entities.RatingForDiscipline>()
                                             .Include(x => x.ExamsGradesSpreadsheet)
                                             .Where(x => x.AcademicGroupId == groupId && x.ExamsGradesSpreadsheet.SemesterNumber == latestSemester)
                                             .GroupBy(x => x.StudentId)
                                             .Select(x => new
                                             {
                                                 StudentId = x.Key,
                                                 AverageScore = (float)x.Average(s => s.Score)
                                             })
                                             .AsNoTracking()
                                             .ToListAsync();

            var scores = await _context.Set<Entities.RatingForDiscipline>()
                                       .Include(x => x.Student)
                                       .Include(x => x.AcademicGroup)
                                       .Include(x => x.AcademicGroup.Students)
                                            .ThenInclude(x => x.UserInfo)
                                       .Include(x => x.AcademicGroup.Students)
                                            .ThenInclude(x => x.FormOfEducation)
                                       .Where(x => x.AcademicDisciplineId == disciplineId && x.AcademicGroupId == groupId)
                                       .OrderByDescending(x => x.Date)
                                       .AsNoTracking()
                                       .ToListAsync();

            var latestDate = scores.FirstOrDefault()?.Date;
            if (latestDate == null)
                return default;

            var latestScores = scores.Where(x => x.Date == latestDate);

            var studentsScore = new List<StudentScoreForDiscipline>();
            foreach (var item in latestScores)
            {
                var score = averageScore.FirstOrDefault(x => x.StudentId == item.StudentId);

                studentsScore.Add(new StudentScoreForDiscipline
                {
                    StudentId = item.StudentId,
                    ScoreId = item.Id,
                    FirstName = item.Student.UserInfo.FirstName,
                    LastName = item.Student.UserInfo.LastName,
                    Score = item.Score,
                    Financing = item.Student.Financing,
                    FormOfEducationId = item.Student.FormOfEducationId,
                    FormOfEducation = item.Student.FormOfEducation.Name,
                    Grade = item.AcademicGroup.Grade,
                    Sex = item.Student.Sex,
                    Residence = item.Student.AddressOfResidence,
                    StudentNumber = item.Student.NumberOfRecordBook,
                    Rating = score == null ? 0f : score.AverageScore * 0.9f
                });
            }
            return studentsScore;
        }
    }
}
