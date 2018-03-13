using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSO2emergencyToDiscordCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Console.OutputEncoding;

            HttpClient hc = new HttpClient();
            string geturl = "https://akakitune87.net/api/v4/pso2emergency";
            aki_luaEventGetter getPSO2 = new aki_luaEventGetter(geturl, hc);
            getPSO2.reloadPSO2Event();

            EventAdmin admin = new EventAdmin(getPSO2.pso2EventBuffer);
            System.Console.ReadKey();
        }
    }
}
