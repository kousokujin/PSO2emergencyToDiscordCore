using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PSO2emergencyToDiscordCore
{
    static class ConvertFromCSV
    {
        //csvファイルからListの２次元配列を返す
        public static List<List<string>> getConvertCSV(string filename)
        {
            if(File.Exists(filename) == false)
            {
                logOutput.writeLog("{0}というファイルは存在しません。", filename);
                
                //空き配列を返す
                List<List<string>> output = new List<List<string>>();
                return output;
            }

            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    List<List<string>> output = new List<List<string>>();

                    while(sr.EndOfStream == false)
                    {
                        List<string> col = new List<string>();

                        string line = sr.ReadLine();
                        string[] val = line.Split(",");

                        foreach(string s in val)
                        {
                            col.Add(s);
                        }

                        output.Add(col);
                    }

                    return output;
                }
            }
            catch (IOException)
            {
                logOutput.writeLog("ファイル{0}を開くのに失敗しました。",filename);

                //空き配列を返す
                List<List<string>> output = new List<List<string>>();
                return output;
            }
        }
    }
}
