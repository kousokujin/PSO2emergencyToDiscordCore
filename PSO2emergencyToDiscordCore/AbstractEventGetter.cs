using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections;

namespace PSO2emergencyToDiscordCore
{
    abstract class AbstractEventGetter : IAsyncHttp
    {
        public string url;
        protected Encoding encode;
        HttpClient hc;

        public List<Event> pso2EventBuffer;  //取得した緊急クエストを入れるバッファ

        public AbstractEventGetter(string url,Encoding enc,HttpClient cl)
        {
            this.url = url;
            this.encode = enc;
            setHTTPClient(cl);

            pso2EventBuffer = new List<Event>();
        }

        public void setHTTPClient(HttpClient hc)
        {
            this.hc = hc;
        }

        public string getUrl()
        {
            return url;
        }

        public async Task<HttpResponseMessage> AsyncHttpPOST(StringContent content)
        {
            if (url != null)
            {
                var respons = await hc.PostAsync(url, content);
                return respons;
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

        public async void AsyncReloadEvent()
        {
            await Task.Run(() => reloadPSO2Event());
        }

        public string outputBufferEmg()
        {
            string output = "";

            int count = 0;
            foreach(Event ev in pso2EventBuffer)
            {
                output += string.Format("({0:00}/{1:00} {2:00}:{3:00}){4}",ev.eventTime.Month,ev.eventTime.Day,ev.eventTime.Hour,ev.eventTime.Minute,ev.eventName);
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
