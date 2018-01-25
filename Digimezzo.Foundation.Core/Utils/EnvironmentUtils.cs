using System;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace Digimezzo.Foundation.Core.Utils
{
    public static class EnvironmentUtils
    {
        /// <summary>
        /// Gets the "friendly" Windows version using WMI 
        /// </summary>
        /// <returns>The "friendly" Windows version</returns>
        public static string GetFriendlyWindowsVersion()
        {
            object nameObject = null;
            string name = null;

            try
            {
                nameObject = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                              select x.GetPropertyValue("Caption")).FirstOrDefault();

                name = nameObject != null ? nameObject.ToString() : "Unknown";
            }
            catch (Exception)
            {
                name = "Unknown";
            }
            
            return name;
        }

        /// <summary>
        /// Gets the internal Windows version using Environment.OSVersion
        /// </summary>
        /// <returns>the internal Windows version</returns>
        public static string GetInternalWindowsVersion()
        {
            return Environment.OSVersion.VersionString;
        }

        /// <summary>
        /// Gets if the Operating System is Windows 10
        /// </summary>
        /// <returns>True if Windows 10</returns>
        public static bool IsWindows10()
        {
            // IMPORTANT: Windows 8.1. and Windows 10 will ONLY admit their real version if your program's manifest 
            // claims to be compatible. Otherwise they claim to be Windows 8. See the first comment on:
            // https://msdn.microsoft.com/en-us/library/windows/desktop/ms724833%28v=vs.85%29.aspx

            bool returnValue = false;

            // Get Operating system information
            OperatingSystem os = Environment.OSVersion;

            // Get the Operating system version information
            Version vi = os.Version;

            // Pre-NT versions of Windows are PlatformID.Win32Windows. We're not interested in those.

            if (os.Platform == PlatformID.Win32NT)
            {
                if (vi.Major == 10)
                {
                    returnValue = true;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Uses the process name to determine if there is exactly 1 instance of that process running.
        /// </summary>
        /// <param name="processName">The process name</param>
        /// <returns>True if there is exactly 1 instance running</returns>
        public static bool IsSingleInstance(string processName)
        {
            Process[] pName = Process.GetProcessesByName(processName);

            if ((pName.Length > 1 | pName.Length == 0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
