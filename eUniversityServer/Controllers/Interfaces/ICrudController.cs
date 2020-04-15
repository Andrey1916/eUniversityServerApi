using eUniversityServer.Models.BindingModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Controllers
{
    public interface ICrudController<TDto, TCreateBinding, TUpdateBinding, TView> where TUpdateBinding : IBindingModel
    {
        Task<ActionResult<IEnumerable<TView>>> Get();

        Task<ActionResult<TView>> Get(Guid id);

        Task<ActionResult<IEnumerable<TView>>> Get(int page, int size);

        Task<ActionResult<Guid>> Post([FromBody] TCreateBinding model);

        Task<ActionResult> Put(Guid id, [FromBody] TUpdateBinding model);

        Task<ActionResult> Delete(Guid id);
    }
}
