using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Logger;
using eUniversityServer.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eUniversityServer.Controllers
{
    public abstract class CrudControllerBase<TDto, TCreateBinding, TUpdateBinding, TView> : ControllerBase where TUpdateBinding : IBindingModel
    {
        protected readonly IMapper _mapper;
        protected readonly Logger _logger;
        protected readonly IService<TDto> _service;

        protected CrudControllerBase(IMapper mapper, IService<TDto> service, Logger logger)
        {
            this._mapper  = mapper ?? throw new NullReferenceException(nameof(mapper));
            this._service = service ?? throw new NullReferenceException(nameof(service));
            this._logger  = logger ?? throw new NullReferenceException(nameof(logger));
        }


        protected async Task<ActionResult<IEnumerable<TView>>> Get()
        {
            try
            {
                var allDtos = await _service.GetAllAsync();
                var items   = _mapper.Map<IEnumerable<TDto>, List<TView>>(allDtos);

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

        protected async Task<ActionResult<TView>> Get(Guid id)
        {
            if (id.Equals(Guid.Empty))
                return BadRequest("Id is empty");

            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null)
                    return NotFound();

                var viewItem = _mapper.Map<TDto, TView>(item);
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

        protected async Task<ActionResult<IEnumerable<TView>>> Get(int page, int size)
        {
            if (page < 0 || size < 0)
                return BadRequest("Page or size are less then zero");

            try
            {
                var items = await _service.GetAllAsync(page, size);

                var viewItems = _mapper.Map<IEnumerable<TDto>, IEnumerable<TView>>(items);
                return Ok(viewItems);
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

        protected async Task<ActionResult<Guid>> Post(TCreateBinding model)
        {
            if (!ModelState.IsValid)
            {
                var typeName = typeof(TCreateBinding).ToString();

                 await _logger.Error($"Invalid { typeName.Remove(typeName.Length - 12) } model.");
                return this.ValidationProblem(ModelState);
            }

            try
            {
                var item = _mapper.Map<TCreateBinding, TDto>(model);
                var id = await _service.AddAsync(item);
                //TODO: Type of new entity
                _logger.Info($"Successfully added new { typeof(TDto).ToString() } with id: { id.ToString() }");

                return Ok(id);
            }
            catch (InvalidModelException imex)
            {
                 await _logger.Error(imex.Message);
                return this.BadRequest(imex.Message);
            }
            catch (ServiceException aex)
            {
                 await _logger.Error(aex.Message, true);
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                 await _logger.Error(ex.Message, true);
                return this.StatusCode(500, "Something went wrong");
            }
        }

        protected async Task<ActionResult> Put(Guid id, TUpdateBinding model)
        {
            if (id.Equals(Guid.Empty) || !id.Equals(model.Id))
                return BadRequest("Id is empty or not equals to the id in the model");

            if (!ModelState.IsValid)
            {
                var typeName = typeof(TUpdateBinding).ToString();

                 await _logger.Error($"Invalid { typeName.Remove(typeName.Length - 12) } model.");
                return ValidationProblem(ModelState);
            }

            try
            {
                var item = _mapper.Map<TUpdateBinding, TDto>(model);
                await _service.UpdateAsync(item);

                _logger.Info($"Successfully updated { typeof(TDto).ToString() } with id: { id.ToString() }");

                return Ok();
            }
            catch (NotFoundException nfex)
            {
                 await _logger.Error(nfex.Message);
                return this.NotFound(nfex.Message);
            }
            catch (InvalidModelException imex)
            {
                 await _logger.Error(imex.Message);
                return this.BadRequest(imex.Message);
            }
            catch (ServiceException aex)
            {
                 await _logger.Error(aex.Message, true);
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                 await _logger.Error(ex.Message, true);
                return this.StatusCode(500, "Something went wrong");
            }
        }

        protected async Task<ActionResult> Delete(Guid id)
        {
            if (id.Equals(Guid.Empty))
                return BadRequest("Id is empty");

            try
            {
                await _service.RemoveAsync(id);

                await _logger.Info($"Successful deleted { typeof(TDto).ToString() } with id: { id.ToString() }");

                return Ok();
            }
            catch (NotFoundException nfex)
            {
                await _logger.Error(nfex.Message);
                return this.NotFound(nfex.Message);
            }
            catch (ServiceException aex)
            {
                await _logger.Error(aex.Message);
                return StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                 await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}