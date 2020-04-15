using AutoMapper;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Dtos;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Logger;
using eUniversityServer.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eUniversityServer.Models.BindingModels;

namespace eUniversityServer.Controllers
{
    [Authorize(Policy = AppPolicies.AdministratorsOnly)]
    [ApiController]
    [Route("api/logs")]
    public class LogController : ControllerBase
    {
        private readonly Logger _logger;
        private readonly ILogService _service;
        private readonly IMapper _mapper;

        public LogController(ILogService service, Logger logger, IMapper mapper)
        {
            _service = service ?? throw new NullReferenceException(nameof(service));
            _logger  = logger ?? throw new NullReferenceException(nameof(logger));
            _mapper  = mapper ?? throw new NullReferenceException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogViewModel>>> Get()
        {
            try
            {
                var allDtos = await _service.GetAllAsync();
                var items = _mapper.Map<IEnumerable<Log>, List<LogViewModel>>(allDtos);

                return Ok(items);
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

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<LogViewModel>>> Get([FromQuery]SieveModel sieveModel)
        {
            try
            {
                var someDtos = await _service.GetSomeAsync(sieveModel);
                var items = _mapper.Map<IEnumerable<Log>, List<LogViewModel>>(someDtos.Result);

                return Ok(
                    new SieveResponseModel<LogViewModel>
                    {
                        TotalCount  = someDtos.TotalCount,
                        Result      = items
                    }
                    );
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

        [HttpGet("{id}")]
        public async Task<ActionResult<LogViewModel>> Get(Guid id)
        {
            if (id.Equals(Guid.Empty))
                return this.BadRequest("Id is empty");

            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null)
                    return NotFound();

                var viewItem = _mapper.Map<Log, LogViewModel>(item);
                return Ok(viewItem);
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