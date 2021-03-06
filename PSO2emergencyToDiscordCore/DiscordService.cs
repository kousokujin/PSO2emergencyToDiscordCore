﻿//using System;
//using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace PSO2emergencyToDiscordCore
{
    class DiscordService : AbstractService
    {
        public DiscordService(string url,HttpClient cl) : base(url, Encoding.UTF8,cl)
        {

        }

        override public Task<string> sendService(string text)
        {
            jsoncontent jc = new jsoncontent();
            jc.content = text;
            string data = JsonConvert.SerializeObject(jc, Formatting.Indented);

            StringContent sc = new StringContent(data, encode, "application/json");
            Task<string> t = AsyncHttpPOST(sc);

            logOutput.writeLog("Discordに投稿「{0}」",text);
            return t;
        }
    }

    class jsoncontent
    {
        [JsonProperty("content")]
        public string content { get; set; }
    }
}
