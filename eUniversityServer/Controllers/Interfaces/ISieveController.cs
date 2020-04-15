using eUniversityServer.Models.BindingModels;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Controllers
{
    public interface ISieveController<TDto, TCreateBinding, TUpdateBinding, TView> : ICrudController<TDto, TCreateBinding, TUpdateBinding, TView> where TUpdateBinding : IBindingModel
    {
        Task<ActionResult<SieveResponseModel<TView>>> Get(SieveModel sieveModel);
    }
}
