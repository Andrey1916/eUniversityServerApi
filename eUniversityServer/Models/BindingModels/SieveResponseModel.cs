using System.Collections.Generic;

namespace eUniversityServer.Models.BindingModels
{
    public class SieveResponseModel<T>
    {
        public IEnumerable<T> Result { get; set; }

        public int TotalCount { get; set; }
    }
}