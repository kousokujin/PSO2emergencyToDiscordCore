//using System;
//using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSO2emergencyToDiscordCore
{
    abstract class AbstractService : HttpSocket,IAsyncPOST
    {
        public AbstractService(string url,Encoding enc,HttpClient cl) : base(url,enc,cl)
        {

        }

        public async Task<string> AsyncHttpPOST(StringContent content)
        {
            if (url != null)
            {
                //エラー処理どうしよ
                try
                {
                    HttpResponseMessage respons = await hc.PostAsync(url, content);
                    string resMes = await respons.Content.ReadAsStringAsync();

                    return resMes;
                }
                catch(HttpRequestException)
                {
                    logOutput.writeLog("投稿に失敗しました。");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public abstract Task<string> sendService(string str);
    }
}
