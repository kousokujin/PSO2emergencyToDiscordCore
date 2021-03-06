﻿using System;
using System.Collections.Generic;
//using System.Text;
//using System.Net.Http;
using System.Text.RegularExpressions;

namespace PSO2emergencyToDiscordCore
{
    class botController
    {
        private AbstractService service;
        private EventAdmin admin;

        public bool rodos;
        public bool chp;    //覇者の通知

        public botController(AbstractService service, EventAdmin admin,bool rodos = true,bool chp = false)
        {
            this.service = service;
            this.admin = admin;
            this.rodos = rodos; //バル・ロドス通知
            this.chp = chp;

            registEvent();
        }

        private void registEvent()
        {
            admin.emgNotify += new EventHandler(this.emgNotiy);
            admin.newDay += new EventHandler(this.newDayPOST);
            admin.Download += new EventHandler(this.newDayPOST);    //めんどいから日付が変わった時と同じ
            admin.rodos30Before += new EventHandler(this.RodosBefore30);
            admin.chpNotify += new EventHandler(this.chpEvent);
            
        }

        public void ToServicePOST(string text)
        {
            service.sendService(text);
        }

        /*
        private string generateEmgArrStr(List<Event> evn)
        {
            string output = "";
            
            foreach(Event e in evn)
            {
                if (e is emgQuest)
                {
                    string eventStr = getLiveEmgStr((emgQuest)e);
                    output += string.Format("{0} {1}\n", e.eventTime.ToString("HH:mm"), eventStr);
                }
            }

            return output;
        }
        */

        public string getServicedUrl()
        {
            return service.getUrl();
        }

        /*
        private string getLiveEmgStr(emgQuest e, string section = "->")   //クーナライブがある時に使う
        {
            if (e.liveEnable == true)
            {
                if (Regex.IsMatch(e.live, "^クーナスペシャルライブ「.*」") == true) //他のライブの時は無理
                {
                    //もっといい方法がありそう
                    string str = Regex.Replace(e.live, "^クーナスペシャルライブ「", "");
                    str = Regex.Replace(str, "」$", "");
                    return string.Format("{0}{1}{2}", str, section, e.eventName);
                }
                else
                {
                    return string.Format("{0}{1}{2}", e.live,section,e.eventName);
                }
            }

            return e.eventName;
        }
        */

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
                    string eventName = myFunction.getLiveEmgStr((emgQuest)tmp.emgData);
                    postStr = string.Format("【{0}分前】{1} {2}", tmp.interval, tmp.emgData.eventTime.ToString("HH:mm"), eventName);
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
                    postStr += myFunction.generateEmgArrStr(tmp.emgList);
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

        private void chpEvent(object sender,EventArgs e)    //覇者の紋章キャンペーンイベント
        {
            if(e is chanpionList)
            {
                chanpionList lst = (chanpionList)e;

                if (lst.chpTarget.Count != 0 && chp == true)
                {
                    string str = "今週の覇者の紋章キャンペーンは以下の通りです。\n";
                    int i = 0;

                    foreach (string s in lst.chpTarget)
                    {
                        str += s;

                        if(i != lst.chpTarget.Count)
                        {
                            str += "\n";
                        }

                        i++;
                    }

                    ToServicePOST(str);
                }
            }
        }

        private void debugEvent(object sender,EventArgs e)
        {
            System.Console.WriteLine("debug Event");
        }
    }
}
