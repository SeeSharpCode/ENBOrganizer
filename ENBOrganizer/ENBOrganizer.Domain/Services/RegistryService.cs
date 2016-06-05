using ENBOrganizer.Util;
using Microsoft.Win32;

namespace ENBOrganizer.Domain.Services
{
    public class RegistryService
    {
        private const string Win32Applications = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        private const string Win64Applications = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

        public string GetInstallPath(string applicationDisplayName)
        {
            string win32InstallPath = GetInstallPath(applicationDisplayName, Win32Applications);

            return !string.IsNullOrWhiteSpace(win32InstallPath) ?  win32InstallPath : GetInstallPath(applicationDisplayName, Win64Applications);
        }

        private string GetInstallPath(string applicationDisplayName, string registryPath)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath))
            {
                foreach (string appKey in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(appKey))
                    {
                        object displayName = subkey.GetValue("DisplayName");

                        if (displayName == null)
                            continue;

                        if (displayName.ToString().EqualsIgnoreCase(applicationDisplayName))
                        {
                            object installLocation = subkey.GetValue("InstallLocation");

                            if (installLocation == null)
                                continue;

                            return installLocation.ToString();
                        }
                    }
                }

                return string.Empty;
            }
        }
    }
}
