using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSO2emergencyToDiscordCore
{
    class Program
    {
        static botController bot;
        static EventAdmin admin;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Console.OutputEncoding;

            printTitle();

            HttpClient hc = new HttpClient();
            string geturl = "https://akakitune87.net/api/v4/pso2emergency";
            string discordUrl = "https://discordapp.com/api/webhooks/348322898089345032/yzcePYWS5nxgRIMNTKKgFPxgOTnEQY9aPY3FXyj5VR_hnO_aivZciwAjgO0EORUUBIPF";

            aki_luaEventGetter getPSO2 = new aki_luaEventGetter(geturl, hc);
            admin = new EventAdmin(getPSO2);
            AbstractService service = new DiscordService(discordUrl, hc);
            bot = new botController(service, admin);

            bool end = false;
            do
            {
                string command = System.Console.ReadLine();
                commandRun(command);
                end = (command == "stop");

            } while (end == false);
        }

        static void printTitle()
        {
            System.Console.WriteLine("-------------------------------");
            System.Console.WriteLine("PSO2emergencyToDiscordCore");
            System.Console.WriteLine("version 1.0.0.0");
            System.Console.WriteLine("Copyright (c) 2017 Kousokujin.");
            System.Console.WriteLine("Released under the MIT license.");
            System.Console.WriteLine("-------------------------------");
        }

        static void commandRun(string command)
        {
            string[] commandData = command.Split(' ');

            if(commandData[0] == "post")
            {
                bot.ToServicePOST(commandData[1]);
            }
        }
    }
}
