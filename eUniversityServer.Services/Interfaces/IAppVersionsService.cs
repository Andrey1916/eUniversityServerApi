using eUniversityServer.Services.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Interfaces
{
    public interface IAppVersionsService
    {
        Task<IEnumerable<AppInfo>> GetAllFilesAsync();

        Task<IEnumerable<AppInfo>> GetAllFilesAsync(string name);

        Task<IEnumerable<AppInfo>> GetLatestFilesAsync();

        Task<IEnumerable<AppInfo>> GetLatestFilesAsync(string name);

        Task<MemoryStream> GetFileFromBucketAsync(string key);
    }
}
