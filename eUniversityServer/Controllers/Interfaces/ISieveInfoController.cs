using eUniversityServer.Models.BindingModels;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Controllers
{
    interface ISieveInfoController<TDto, TDtoInfo, TCreateBinding, TUpdateBinding, TView, TViewInfo> : ICrudInfoController<TDto, TDtoInfo, TCreateBinding, TUpdateBinding, TView, TViewInfo>
        where TUpdateBinding : IBindingModel 
        where TDtoInfo : TDto
        where TViewInfo : TView
    {
        Task<ActionResult<SieveResponseModel<TViewInfo>>> GetWithMoreInfo(SieveModel sieveModel);
    }
}
