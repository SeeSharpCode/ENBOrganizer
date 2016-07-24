using Octokit;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ENBOrganizer.App
{
    public static class UpdateService
    {
        public static async Task<bool> IsUpdateAvailable()
        {
            GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("ENBOrganizer"));
            Release latestRelease = await gitHubClient.Repository.Release.GetLatest("SeeSharpCode", "ENBOrganizer");

            Version latestReleaseVersion;
            Version.TryParse(latestRelease.TagName.Replace("v", string.Empty), out latestReleaseVersion);

            return latestReleaseVersion > Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}
