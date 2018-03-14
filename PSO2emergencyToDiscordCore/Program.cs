using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace PSO2emergencyToDiscordCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            printTitle();
            string discordUrl = "https://discordapp.com/api/webhooks/348322898089345032/yzcePYWS5nxgRIMNTKKgFPxgOTnEQY9aPY3FXyj5VR_hnO_aivZciwAjgO0EORUUBIPF";

            //List<string> command = new List<string>();

            AbstractController maincontroll = new Controller(discordUrl);
        }

        static void printTitle()
        {
            System.Console.WriteLine("-------------------------------");
            System.Console.WriteLine("PSO2emergencyToDiscordCore");
            System.Console.WriteLine("version 1.0.0.0");
            System.Console.WriteLine("Copyright (c) 2018 Kousokujin.");
            System.Console.WriteLine("Released under the MIT license.");
            System.Console.WriteLine("-------------------------------");
        }
    }
}
