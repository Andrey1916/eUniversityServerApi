using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Controllers
{
    public abstract class SieveControllerBase<TDto, TCreateBinding, TUpdateBinding, TView> : CrudControllerBase<TDto, TCreateBinding, TUpdateBinding, TView> where TUpdateBinding : IBindingModel
    {
        public SieveControllerBase(IMapper mapper, IService<TDto> service, Logger logger) : base(mapper, service, logger)
        { }

        protected async Task<ActionResult<SieveResponseModel<TView>>> Get(SieveModel sieveModel)
        {
            try
            {
                var someDtos = await _service.GetSomeAsync(sieveModel);
                var items = _mapper.Map<IEnumerable<TDto>, List<TView>>(someDtos.Result);

                return Ok(
                    new SieveResponseModel<TView>
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
    }
}
