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
            string discordUrl = "";

            aki_luaEventGetter getPSO2 = new aki_luaEventGetter(geturl, hc);
            EventAdmin admin = new EventAdmin(getPSO2);
            DiscordService service = new DiscordService(discordUrl, hc);
            botController bot = new botController(service, admin);

            bot.ToServicePOST("テスト投稿\n改行テスト");

            System.Console.ReadKey();
        }
    }
}
