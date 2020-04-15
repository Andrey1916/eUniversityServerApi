using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Controllers
{
    public abstract class CrudInfoControllerBase<TDto, TDtoInfo, TCreateBinding, TUpdateBinding, TView, TViewInfo> : CrudControllerBase<TDto, TCreateBinding, TUpdateBinding, TView>
                                                                                                                     where TUpdateBinding : IBindingModel
                                                                                                                     where TDtoInfo : TDto
                                                                                                                     where TViewInfo : TView
    {
        protected new readonly IServiceMoreInfo<TDto, TDtoInfo> _service;

        protected CrudInfoControllerBase(IMapper mapper, IServiceMoreInfo<TDto, TDtoInfo> service, Logger logger) : base(mapper, service, logger)
        {
            this._service = service ?? throw new NullReferenceException(nameof(service));
        }

        protected async Task<ActionResult<IEnumerable<TViewInfo>>> GetWithMoreInfo()
        {
            try
            {
                var allDtos = await _service.GetAllWithInfoAsync();
                var items = _mapper.Map<IEnumerable<TDtoInfo>, List<TViewInfo>>(allDtos);

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

        protected async Task<ActionResult<IEnumerable<TViewInfo>>> GetWithMoreInfo(int page, int size)
        {
            try
            {
                var allDtos = await _service.GetAllWithInfoAsync(page, size);
                var items = _mapper.Map<IEnumerable<TDtoInfo>, List<TViewInfo>>(allDtos);

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
    }
}
