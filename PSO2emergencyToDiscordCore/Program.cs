using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSO2emergencyToDiscordCore
{
    class Program
    {
        static void Main(string[] args)
        {
            logOutput.init("log.txt");
            logOutput.writeLog("起動しました。");
            string url = "https://discordapp.com/api/webhooks/348322898089345032/yzcePYWS5nxgRIMNTKKgFPxgOTnEQY9aPY3FXyj5VR_hnO_aivZciwAjgO0EORUUBIPF";

            HttpClient hc = new HttpClient();
            DiscordService discord = new DiscordService(url, hc);
            Task<HttpResponseMessage> t = discord.sendService("testmessage");

            t.Wait();
            //System.Console.WriteLine(t.Result.ToString());
            System.Console.ReadKey();
        }
    }
}
