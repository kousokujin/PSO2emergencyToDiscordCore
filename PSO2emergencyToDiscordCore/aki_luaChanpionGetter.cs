using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PSO2emergencyToDiscordCore
{
    //覇者の紋章関連クラス
    class aki_luaChanpionGetter : AbstractChanpionGetter
    {
        public aki_luaChanpionGetter(string url,HttpClient hc) : base(url, Encoding.UTF8, hc)
        {

        }

        public override List<string> chanpion()
        {

            Task<string> POSTresult;

            try
            {
                POSTresult = AsyncHttpGET();
                POSTresult.Wait();
            }
            catch (System.NullReferenceException)
            {
                List<string> emptyList = new List<string>();
                return emptyList;
            }

            string resultStr = POSTresult.Result;

            //JSONをパース
            JsonChanpion JsonResult = JsonConvert.DeserializeObject<JsonChanpion>(resultStr);

            logOutput.writeLog("覇者の紋章キャンペーンは以下の通りです。\n{0}", chanpionListStr(JsonResult.TargetList));

            return JsonResult.TargetList;
        }

        public string chanpionListStr(List<string> lst)
        {
            string output = "";
            
            foreach(string st in lst)
            {
                output += string.Format("{0}\n",st);
            }

            return output;
        }
    }

    class JsonChanpion
    {
        [JsonProperty("UpdateTime")]
        public string UpdateTime;

        [JsonProperty("TargetList")]
        public List<string>　TargetList;
    }
}
