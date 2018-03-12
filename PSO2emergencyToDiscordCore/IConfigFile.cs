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
        void saveConfig(object obj,string typeStr);
        object loadConfig();
        string getFilename();
    }
}
