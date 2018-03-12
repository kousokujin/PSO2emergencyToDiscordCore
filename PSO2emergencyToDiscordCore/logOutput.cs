using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PSO2emergencyToDiscordCore
{
    class logOutput
    {
        static StreamWriter writer;
        private static string filename;
        private static DateTime dt;
        private static string date;
        private static string time;

        public static void writeLog(string str)
        {
            dt = DateTime.Now;
            // enc = Encoding.GetEncoding("UTF-8");

            date = dt.ToString("yyyy/MM/dd");
            time = dt.ToString("HH:mm:ss");
            string text = string.Format("[{0}{1}]{2}", date, time, str);
            writer.WriteLine(text);
            System.Console.WriteLine(text);
        }

        public static void init(string name)
        {
            filename = name;
            writer = new System.IO.StreamWriter(new FileStream(filename,FileMode.Create));

        }
    }
}
