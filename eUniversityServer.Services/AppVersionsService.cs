using Amazon.S3;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Services.Models;
using eUniversityServer.Services.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eUniversityServer.Services
{
    public class AppVersionsService : IAppVersionsService
    {
        const string S3_BUCKET = "euniversity-apps-releases";
        const string S3_ACCESS_KEY = "AKIAJ53OYFIFQZR2IJDQ";
        const string S3_SECRET_ACCESS_KEY = "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";

        public async Task<IEnumerable<AppInfo>> GetAllFilesAsync()
        {
            using (var s3Client = new AmazonS3Client(S3_ACCESS_KEY, S3_SECRET_ACCESS_KEY, Amazon.RegionEndpoint.EUCentral1))
            {
                var objects = await s3Client.ListObjectsAsync(S3_BUCKET);

                var appFiles = new List<AppInfo>();
                var regex = new Regex(@"^(?<appName>[^\s-]+)-(?<appVersion>[\d\.]+)?$");

                foreach (var item in objects.S3Objects)
                {
                    var match = regex.Match(Path.GetFileNameWithoutExtension(item.Key));
                    if (!match.Success)
                        continue;

                    string name = match.Groups["appName"].Value;
                    string version = match.Groups["appVersion"].Value;

                    appFiles.Add(new AppInfo
                    {
                        Key = item.Key,
                        AppName = name,
                        AppVersion = version
                    });
                }

                return appFiles;
            }
        }
        public async Task<IEnumerable<AppInfo>> GetAllFilesAsync(string name)
        {
            var files = await GetAllFilesAsync();
            return files.Where(x => x.AppName == name);
        }

        public async Task<IEnumerable<AppInfo>> GetLatestFilesAsync()
        {
            var files = await GetAllFilesAsync();
            return files.GroupBy(x => x.AppName)
                        .Select(x => x.OrderByDescending(a => a.AppVersion, new AppVersionComparer())
                                      .First());
        }

        public async Task<IEnumerable<AppInfo>> GetLatestFilesAsync(string name)
        {
            var files = await GetAllFilesAsync();
            return files.GroupBy(x => x.AppName)
                        .Where(x => x.Key == name)
                        .Select(x => x.OrderByDescending(a => a.AppVersion, new AppVersionComparer())
                                      .First());
        }

        public async Task<MemoryStream> GetFileFromBucketAsync(string key)
        {
            using (var s3Client = new AmazonS3Client(S3_ACCESS_KEY, S3_SECRET_ACCESS_KEY, Amazon.RegionEndpoint.EUCentral1))
            {
                var response = await s3Client.GetObjectAsync(S3_BUCKET, key);

                var memoryStream = new MemoryStream();
                using (Stream responseStream = response.ResponseStream)
                {
                    responseStream.CopyTo(memoryStream);
                }
                return memoryStream;
            }
        }
    }
}
