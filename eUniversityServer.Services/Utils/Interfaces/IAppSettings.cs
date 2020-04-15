using System;
using System.Collections.Generic;
using System.Text;

namespace eUniversityServer.Services.Utils
{
    public interface IAppSettings
    {
        string Secret { get; }

        /// <summary>
        /// Life time in minutes
        /// </summary>
        int AccessTokenLifeTime { get; }

        int RefreshTokenLifeTime { get; }

        string Issuer { get; }

        string Audience { get; }
    }
}
