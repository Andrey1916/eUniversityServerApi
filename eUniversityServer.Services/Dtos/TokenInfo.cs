using System;
using System.Collections.Generic;
using System.Text;

namespace eUniversityServer.Services.Dtos
{
    public class TokenInfo
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int Expire { get; set; }
    }
}
