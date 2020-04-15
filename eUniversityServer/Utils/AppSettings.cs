using eUniversityServer.Services.Utils;
using Microsoft.IdentityModel.Tokens;

namespace eUniversityServer.Utils
{
    public class AppSettings : IAppSettings
    {
        public string Secret { get; set; }

        public string SendGridkey { get; set; }

        /// <summary>
        /// Life time in minutes
        /// </summary>
        public int AccessTokenLifeTime { get; set; }

        public int RefreshTokenLifeTime { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
