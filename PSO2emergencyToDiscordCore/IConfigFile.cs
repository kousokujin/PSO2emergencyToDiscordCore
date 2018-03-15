using System;
using System.Collections.Generic;
using System.Text;

namespace PSO2emergencyToDiscordCore
{
    /*
     * コンフィグファイルを設定するインターフェース
     */
    interface IConfigFile
    {
        bool saveConfig();
        bool loadConfig(out object obj);
        string getFilename();
    }
}
