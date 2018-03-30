using System;
using System.Collections.Generic;
using System.Text;

namespace PSO2emergencyToDiscordCore
{
    static class myFunction //クラスにするほどでもない関数群
    {
        static public long convertToUnixTime(DateTime time)    //UNIX時間に変換
        {
            DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime target = time.ToUniversalTime();
            TimeSpan ts = target - UNIX_EPOCH;

            return (long)ts.Seconds;
        }

        static public string getAssemblyVersion()
        {
            System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(
                System.Reflection.Assembly.GetExecutingAssembly().Location);

            string version = ver.ProductVersion;

            return version;
        }
    }
}
