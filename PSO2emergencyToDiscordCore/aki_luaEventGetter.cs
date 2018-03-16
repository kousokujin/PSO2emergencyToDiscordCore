using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
//using System.Collections;
using Newtonsoft.Json;

namespace PSO2emergencyToDiscordCore
{
    class aki_luaEventGetter : AbstractEventGetter
    {
        public aki_luaEventGetter(string url,HttpClient cl) : base(url, Encoding.UTF8,cl)
        {
            base.pso2EventBuffer = new List<Event>();
        }

        public override void reloadPSO2Event()
        {
            //取得する緊急クエストの日数を計算
            DateTime dt = DateTime.Now;

            int getDays = 7 - ((int)dt.DayOfWeek + 4) % 7;    //この先の緊急を取得する日数

            if (getDays == 7)   //水曜日の時
            {
                DateTime dt1630 = new DateTime(dt.Year, dt.Month, dt.Day, 17, 00, 0);   //今日の17:00
                if (DateTime.Compare(dt, dt1630) <= 0)
                {
                    getDays = 0;
                }
            }

            //バッファの初期化など
            if(base.pso2EventBuffer.Count != 0)
            {
                base.pso2EventBuffer.Clear();
            }

            //緊急クエスト取得成功・失敗の結果
            bool getOK = true;

            //緊急クエストの取得
            for(int i = 0; i <= getDays; i++)
            {
                DateTime getEmgTime = dt + new TimeSpan(i, 0, 0, 0);

                //JSONを生成
                sendjson_eventgetter jsonData = new sendjson_eventgetter();
                jsonData.EventDate = getEmgTime.ToString("yyyyMMdd");
                string data = JsonConvert.SerializeObject(jsonData, Formatting.Indented);

                //HTTPによる取得
                StringContent sc = new StringContent(data,encode, "application/json");
                Task<HttpResponseMessage> resultHTTP = AsyncHttpPOST(sc);
                resultHTTP.Wait();

                //結果をstringにする
                Task<string> resultStrTask;
                try
                {
                    resultStrTask = resultHTTP.Result.Content.ReadAsStringAsync();
                }
                catch(System.NullReferenceException)    //緊急クエストの取得に失敗
                {
                    break;
                }
                resultStrTask.Wait();
                string result = resultStrTask.Result;

                //Jsonをパース
                List<JsonPSO2Event> Jsonresult = new List<JsonPSO2Event>();
                Jsonresult = JsonConvert.DeserializeObject<List<JsonPSO2Event>>(result);

                //バッファに格納
                foreach(JsonPSO2Event ev in Jsonresult)
                {
                    DateTime emgDT = new DateTime(DateTime.Now.Year, ev.Month, ev.Date, ev.Hour, ev.Minute, 0);
                    bool live = false;
                    string livename = "";

                    if(ev.EventType == "緊急")
                    {
                        emgQuest emg;
                        if (live == true)
                        {
                            emg = new emgQuest(emgDT, ev.EventName, livename);
                            live = false;
                            livename = "";
                        }
                        else
                        {
                            emg = new emgQuest(emgDT, ev.EventName);
                        }
                        base.pso2EventBuffer.Add(emg);
                    }

                    if(ev.EventType == "ライブ")
                    {
                        live = true;
                        livename = ev.EventName;
                    }

                    if(ev.EventType == "カジノイベント")
                    {
                        casino cas = new casino(emgDT);
                        base.pso2EventBuffer.Add(cas);
                    }
                }
            }

            if (getOK == true)
            {
                logOutput.writeLog("緊急クエストの情報を取得しました。緊急クエストは以下の通りです。\n" + outputBufferEmg());
            }

        }
    }

    class sendjson_eventgetter
    {
        [JsonProperty("EventDate")]
        public string EventDate { get; set; }

    }

    class JsonPSO2Event
    {
        public string EventName { get; set; }
        public string EventType { get; set; }
        public int Month { get; set; }
        public int Date { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
    }
}
