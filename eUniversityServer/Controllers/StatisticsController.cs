using AutoMapper;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Logger;
using eUniversityServer.Services.Models.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/statistics")]
    public sealed class StatisticsController : ControllerBase
    {
        private readonly Logger _logger;

        private readonly IMapper _mapper;

        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IMapper mapper, Logger logger, IStatisticsService statisticsService)
        {
            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
            _logger = logger ?? throw new NullReferenceException(nameof(logger));
            _statisticsService = statisticsService ?? throw new NullReferenceException(nameof(statisticsService));
        }


        [HttpGet("students-dispersion/{specialtyId}")]
        public async Task<ActionResult<IEnumerable<SpecialtyStudentsDispersionViewModel>>> GetSpecialtyStudentsDispersion(Guid specialtyId)
        {
            if (specialtyId.Equals(Guid.Empty))
                return BadRequest("Specialty id is empty");

            try
            {
                var result = await _statisticsService.GetStudentsDispersionBySpecialtyAsync(specialtyId);
                var students = _mapper.Map<IEnumerable<SpecialtyStudentsDispersion>, IEnumerable<SpecialtyStudentsDispersionViewModel>>(result);

                return Ok(students);
            }
            catch (ServiceException ex)
            {
                return StatusCode((int)ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet("last-scores/{disciplineId}/{groupId}")]
        public async Task<ActionResult<IEnumerable<SpecialtyStudentsDispersionViewModel>>> GetStudentsLastScores(Guid disciplineId, Guid groupId)
        {
            if (disciplineId.Equals(Guid.Empty))
                return BadRequest("Discipline id is empty");

            if (groupId.Equals(Guid.Empty))
                return BadRequest("Group id is empty");

            try
            {
                var result = await _statisticsService.GetLatestScoresForDisciplineAsync(disciplineId, groupId);
                var scores = _mapper.Map<IEnumerable<StudentScoreForDiscipline>, IEnumerable<StudentScoreForDisciplineViewModel>>(result);

                return Ok(scores);
            }
            catch (ServiceException ex)
            {
                return StatusCode((int)ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
