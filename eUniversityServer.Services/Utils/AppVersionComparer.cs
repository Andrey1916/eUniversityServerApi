using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace eUniversityServer.Services.Utils
{
    public class AppVersionComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            var regex = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)\.(?<build>\d+)(?:\.(?<revision>\d+))?", RegexOptions.Singleline);
            var xVersionMatch = regex.Match(x);
            var yVersionMatch = regex.Match(y);

            if (!xVersionMatch.Success)
                throw new Exception("First string is not a valid app version");

            if (!yVersionMatch.Success)
                throw new Exception("Second string is not a valid app version");

            // compare major versions
            int xMajor = Convert.ToInt32(xVersionMatch.Groups["major"].Value);
            int yMajor = Convert.ToInt32(yVersionMatch.Groups["major"].Value);

            if (xMajor != yMajor)
            {
                return xMajor > yMajor ? 1 : -1;
            }

            // compare minor versions
            int xMinor = Convert.ToInt32(xVersionMatch.Groups["minor"].Value);
            int yMinor = Convert.ToInt32(yVersionMatch.Groups["minor"].Value);

            if (xMinor != yMinor)
            {
                return xMinor > yMinor ? 1 : -1;
            }

            // compare minor versions
            int xBuild = Convert.ToInt32(xVersionMatch.Groups["build"].Value);
            int yBuild = Convert.ToInt32(yVersionMatch.Groups["build"].Value);

            if (xBuild != yBuild)
            {
                return xBuild > yBuild ? 1 : -1;
            }

            int xRevision = Convert.ToInt32(xVersionMatch.Groups["revision"]?.Value ?? "-1");
            int yRevision = Convert.ToInt32(yVersionMatch.Groups["revision"]?.Value ?? "-1");

            // end with minor versions comparing, because revisions not found
            if (xRevision < 0 || yRevision < 0)
                return 0;

            // compare revisions
            if (xRevision != yRevision)
            {
                return xRevision > yRevision ? 1 : -1;
            }
            return 0;
        }
    }
}
