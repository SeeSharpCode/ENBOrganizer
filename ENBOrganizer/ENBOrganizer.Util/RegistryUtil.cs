using Microsoft.Win32;

namespace ENBOrganizer.Util
{
    public static class RegistryUtil
    {
        private const string Win32Applications = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        private const string Win64Applications = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

        public static bool TryGetInstallPath(string applicationDisplayName, out string installPath)
        {
            string win32InstallPath = GetInstallPath(applicationDisplayName, Win32Applications);

            installPath = !string.IsNullOrWhiteSpace(win32InstallPath) ?  win32InstallPath : GetInstallPath(applicationDisplayName, Win64Applications);

            return !string.IsNullOrWhiteSpace(installPath);
        }

        private static string GetInstallPath(string applicationDisplayName, string registryPath)
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
