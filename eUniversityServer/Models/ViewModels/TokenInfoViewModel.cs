using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eUniversityServer.Models.ViewModels
{
    public class TokenInfoViewModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int Expire { get; set; }
    }
}
