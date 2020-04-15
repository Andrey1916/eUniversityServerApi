using AutoMapper;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Logger;
using eUniversityServer.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eUniversityServer.Controllers
{
    [ApiController]
    [Route("api/versions")]
    public class AppVersionsController : ControllerBase
    {
        protected readonly Logger _logger;

        protected readonly IMapper _mapper;

        protected readonly IAppVersionsService _appVersionsService;

        public AppVersionsController(IAppVersionsService appVersionsService, IMapper mapper, Logger logger)
        {
            _appVersionsService = appVersionsService ?? throw new NullReferenceException(nameof(appVersionsService));
            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
            _logger = logger ?? throw new NullReferenceException(nameof(logger));
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppVersionViewModel>>> Get()
        {
            try
            {
                var files = await _appVersionsService.GetAllFilesAsync();
                var versions = _mapper.Map<IEnumerable<AppInfo>, IEnumerable<AppVersionViewModel>>(files);

                return Ok(versions);
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

        [HttpGet("{appName}")]
        public async Task<ActionResult<IEnumerable<AppVersionViewModel>>> Get(string appName)
        {
            try
            {
                var files = await _appVersionsService.GetAllFilesAsync(appName);
                var versions = _mapper.Map<IEnumerable<AppInfo>, IEnumerable<AppVersionViewModel>>(files);

                return Ok(versions);
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

        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<AppVersionViewModel>>> GetLatest()
        {
            try
            {
                var files = await _appVersionsService.GetLatestFilesAsync();
                var versions = _mapper.Map<IEnumerable<AppInfo>, IEnumerable<AppVersionViewModel>>(files);

                return Ok(versions);
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

        [HttpGet("latest/{appName}")]
        public async Task<ActionResult<IEnumerable<AppVersionViewModel>>> GetLatest(string appName)
        {
            try
            {
                var files = await _appVersionsService.GetLatestFilesAsync(appName);
                var versions = _mapper.Map<IEnumerable<AppInfo>, IEnumerable<AppVersionViewModel>>(files);

                return Ok(versions);
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

        [HttpGet("download/{key}")]
        public async Task<ActionResult<IEnumerable<AppVersionViewModel>>> GetFile(string key)
        {
            try
            {
                var fileStream = await _appVersionsService.GetFileFromBucketAsync(key);
                return File(fileStream, "application/octet-stream");
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
