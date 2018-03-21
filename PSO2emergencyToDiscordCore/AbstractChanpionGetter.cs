using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSO2emergencyToDiscordCore
{
    abstract class AbstractChanpionGetter : HttpSocket,IAsyncGET
    {
        public AbstractChanpionGetter(string url,Encoding enc,HttpClient cl) : base(url, enc, cl)
        {

        }

        public async Task<string> AsyncHttpGET()
        {
            try
            {
                HttpResponseMessage mes = await hc.GetAsync(url);
                string resMes = await mes.Content.ReadAsStringAsync();

                return resMes;
            }
            catch (HttpRequestException)
            {
                logOutput.writeLog("覇者の紋章キャンペーン情報の取得に失敗しました。");
                return null;
            }
        }

        public abstract List<string> chanpion();

        public async Task<List<string>> AsyncChanpion() //非同期
        {
            List<string> res = await Task.Run(() => {
                return chanpion();
                });

            return res;
        }
    }
}
