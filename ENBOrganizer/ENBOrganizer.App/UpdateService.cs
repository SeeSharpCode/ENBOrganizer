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
            try
            {
                GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("ENBOrganizer"));
                Release latestRelease = await gitHubClient.Repository.Release.GetLatest("SeeSharpCode", "ENBOrganizer");

                Version latestReleaseVersion = new Version(FormatReleaseVersionNumber(latestRelease.TagName));
                Version currentVersion = GetCurrentAssemblyVersion();

                return latestReleaseVersion > currentVersion;
            }
            catch (Exception)
            {
                // Don't inform the user of failure to check for updates.
                return false;
            }
        }

        private static string FormatReleaseVersionNumber(string tagName)
        {
            return tagName.Replace("v", "");
        }

        private static Version GetCurrentAssemblyVersion()
        {
            Version unformattedVerson = Assembly.GetExecutingAssembly().GetName().Version;

            return new Version(unformattedVerson.Major.ToString() + "." + unformattedVerson.Minor.ToString());
        }
    }
}
