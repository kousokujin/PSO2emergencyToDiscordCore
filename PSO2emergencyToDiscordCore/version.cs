using System;
using System.Collections.Generic;
using System.Text;

namespace PSO2emergencyToDiscordCore
{
    static class version
    {
        static public string getAssemblyVersion()
        {
            System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(
                System.Reflection.Assembly.GetExecutingAssembly().Location);

            string version = ver.ProductVersion;

            return version;
        }
    }
}
