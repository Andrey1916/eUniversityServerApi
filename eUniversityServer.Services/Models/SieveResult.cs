using System;
using System.Collections.Generic;
using System.Text;

namespace eUniversityServer.Services.Models
{
    public class SieveResult<T>
    {
        public int TotalCount { get; set; }

        public IEnumerable<T> Result { get; set; } 
    }
}
