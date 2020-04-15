using eUniversityServer.Models.BindingModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Controllers
{
    public interface ICrudInfoController<TDto, TDtoInfo, TCreateBinding, TUpdateBinding, TView, TViewInfo> : ISieveController<TDto, TCreateBinding, TUpdateBinding, TView>
        where TUpdateBinding : IBindingModel
        where TDtoInfo : TDto
        where TViewInfo : TView
    {
        Task<ActionResult<IEnumerable<TViewInfo>>> GetWithMoreInfo();

        Task<ActionResult<IEnumerable<TViewInfo>>> GetWithMoreInfo(int page, int size);
    }
}
