using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
//using System.Collections;

namespace PSO2emergencyToDiscordCore
{
    abstract class AbstractEventGetter : HttpSocket,IAsyncPOST
    {

        public List<Event> pso2EventBuffer;  //取得した緊急クエストを入れるバッファ

        public AbstractEventGetter(string url,Encoding enc,HttpClient cl) : base(url,enc,cl)
        {

            pso2EventBuffer = new List<Event>();
        }

        public async Task<string> AsyncHttpPOST(StringContent content)
        {
            if (url != null)
            {
                try
                {
                    HttpResponseMessage respons = await hc.PostAsync(url, content);
                    string resMes = await respons.Content.ReadAsStringAsync();
                    return resMes;

                }
                catch(HttpRequestException)
                {
                    logOutput.writeLog("POSTに失敗しました。");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public List<Event> getPSO2Event()
        {
            return pso2EventBuffer;
        }

        public abstract void reloadPSO2Event();

        public async Task AsyncReloadEvent()
        {
            await Task.Run(() => reloadPSO2Event());
        }

        public string outputBufferEmg()
        {
            string output = "";

            int count = 0;
            foreach(Event ev in pso2EventBuffer)
            {
                if (ev is emgQuest)
                {
                    emgQuest emg = (emgQuest)ev;
                    if (emg.liveEnable == true)
                    {
                        DateTime livetime = emg.eventTime - new TimeSpan(0,30,0);
                        
                        output += string.Format("({0:00}/{1:00} {2:00}:{3:00}){4}\n", livetime.Month, livetime.Day, livetime.Hour,livetime.Minute,emg.live);
                        output += string.Format("({0:00}/{1:00} {2:00}:{3:00}){4}", emg.eventTime.Month, emg.eventTime.Day, emg.eventTime.Hour, emg.eventTime.Minute, emg.eventName);
                    }
                    else
                    {
                        output += string.Format("({0:00}/{1:00} {2:00}:{3:00}){4}", ev.eventTime.Month, ev.eventTime.Day, ev.eventTime.Hour, ev.eventTime.Minute, ev.eventName);
                    }
                }
                else
                {
                    output += string.Format("({0:00}/{1:00} {2:00}:{3:00}){4}", ev.eventTime.Month, ev.eventTime.Day, ev.eventTime.Hour, ev.eventTime.Minute, ev.eventName);
                }

                if(count != pso2EventBuffer.Count-1)
                {
                    output += "\n";
                }

                count++;
                //output += Environment.NewLine;
            }

            return output;
        }


    }
}
