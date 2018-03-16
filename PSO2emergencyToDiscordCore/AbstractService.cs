//using System;
//using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSO2emergencyToDiscordCore
{
    abstract class AbstractService : IAsyncHttp
    {
        public string url;
        public Encoding encode;
        HttpClient hc;

        public AbstractService(string url,Encoding enc,HttpClient cl)
        {
            this.url = url;
            this.encode = enc;
            this.hc = new HttpClient();
            setHTTPClient(cl);
        }

        public string getUrl()
        {
            return url;
        }

        public void setHTTPClient(HttpClient hc)
        {
            this.hc = hc;
        }

        public async Task<HttpResponseMessage> AsyncHttpPOST(StringContent content)
        {
            if (url != null)
            {
                //エラー処理どうしよ
                try
                {
                    var respons = await hc.PostAsync(url, content);
                    return respons;
                }
                catch(HttpRequestException)
                {
                    logOutput.writeLog("投稿に失敗しました。");
                    return null;
                }
            }
            else{
                return null;
            }
        }

        public abstract Task<HttpResponseMessage> sendService(string str);
    }
}
