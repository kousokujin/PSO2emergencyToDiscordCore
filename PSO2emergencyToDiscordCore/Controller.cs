using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace PSO2emergencyToDiscordCore
{
    class Controller:AbstractController
    {
        HttpClient hc;
        AbstractEventGetter emgGetter;
        EventAdmin admin;
        botController bot;
        AbstractService service;

        string discordurl;
        string HttpGetUrl;

        Controller(List<string> lst) : base(lst)
        {
            init();
            loop();
        }

        private void init()
        {
            HttpGetUrl = "https://akakitune87.net/api/v4/pso2emergency";
            discordurl = "https://discordapp.com/api/webhooks/348322898089345032/yzcePYWS5nxgRIMNTKKgFPxgOTnEQY9aPY3FXyj5VR_hnO_aivZciwAjgO0EORUUBIPF";

            hc = new HttpClient();
            emgGetter = new aki_luaEventGetter(HttpGetUrl, hc);
            service = new DiscordService(discordurl, hc);
            admin = new EventAdmin(emgGetter);
            bot = new botController(service, admin);

        }

        public override void commandProcess(string command, string[] args)
        {
            if(command == "stop")
            {
                end = true;
            }
        }

    }
}
