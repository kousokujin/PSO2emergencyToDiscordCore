//using System;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Collections.Generic;
using System.Text;

namespace PSO2emergencyToDiscordCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            printTitle();
            ControllerCore maincontroll = new ConsoleController();
        }

        static void printTitle()
        {
            System.Console.Title = "PSO2emergencyToDiscordCore";
            System.Console.WriteLine("-------------------------------");
            System.Console.WriteLine("PSO2emergencyToDiscordCore");
            System.Console.WriteLine("version {0}",version.getAssemblyVersion());
            System.Console.WriteLine("Copyright (c) 2018 Kousokujin.");
            System.Console.WriteLine("Released under the MIT license.");
            System.Console.WriteLine("-------------------------------");
        }
    }
}
