using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace PSO2emergencyToDiscordCore
{
    class botController
    {
        private AbstractService service;
        private EventAdmin admin;

        public bool rodos;

        public botController(AbstractService service, EventAdmin admin,bool rodos = true)
        {
            this.service = service;
            this.admin = admin;
            this.rodos = rodos; //バル・ロドス通知

            registEvent();
        }

        private void registEvent()
        {
            admin.emgNotify += new EventHandler(this.emgNotiy);
            admin.newDay += new EventHandler(this.newDayPOST);
            admin.Download += new EventHandler(this.newDayPOST);    //めんどいから日付が変わった時と同じ
            admin.rodos30Before += new EventHandler(this.RodosBefore30);
            
        }

        public void ToServicePOST(string text)
        {
            service.sendService(text);
        }

        private string generateEmgArrStr(List<Event> evn)
        {
            string output = "";
            
            foreach(Event e in evn)
            {
                output += string.Format("{0} {1}\n", e.eventTime.ToString("HH:mm"), e.eventName);
            }

            return output;
        }

        public string getServicedUrl()
        {
            return service.getUrl();
        }

        //-----イベント------
        private void emgNotiy(object sender, EventArgs e)    //緊急通知が来た時のイベント
        {
            if (e is emgEventData)
            {
                emgEventData tmp = (emgEventData)e;
                string postStr = "";
                if (tmp.interval == 0)
                {
                    postStr = string.Format("【緊急開始】{0} {1}", tmp.emgData.eventTime.ToString("HH:mm"), tmp.emgData.eventName);
                }
                else
                {
                    postStr = string.Format("【{0}分前】{1} {2}", tmp.interval, tmp.emgData.eventTime.ToString("HH:mm"), tmp.emgData.eventName);
                }

                ToServicePOST(postStr);
            }
        }

        private void newDayPOST(object sender,EventArgs e)  //日付が変わった時のイベント
        {
            if(e is DailyEventList)
            {
                System.Console.WriteLine("Download");
                DailyEventList tmp = (DailyEventList)e;
                string postStr = "";

                if (tmp.emgList.Count != 0)
                {
                    postStr = string.Format("{0}月{1}日の緊急クエストは以下の通りです。\n",DateTime.Now.Month,DateTime.Now.Day);
                    postStr += generateEmgArrStr(tmp.emgList);
                }

                if(tmp.rodosDay == true && rodos == true)
                {
                    postStr += "本日はデイリーオーダー「バル・ロドス討伐(VH)」の日です。";
                }

                if (postStr != "")
                {
                    ToServicePOST(postStr);
                }
            }
        }

        private void RodosBefore30(object sender,EventArgs e)   //バル・ロドスが終わる30分前イベント
        {
            if(rodos == true)
            {
                DateTime next = rodosCalculator.nextRodosDay(DateTime.Now + new TimeSpan(24,0,0));
                string postStr = string.Format("デイリーオーダー「バル・ロドス討伐(VH)」の日があと30分で終わります。オーダーは受注しましたか？\n次回のバル・ロドス討伐(VH)の日は{0}月{1}日です。",next.Month,next.Day);
                ToServicePOST(postStr);
            }
        }

        private void debugEvent(object sender,EventArgs e)
        {
            System.Console.WriteLine("debug Event");
        }
    }
}
