using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PSO2emergencyToDiscordCore
{
    class EventAdmin
    {
        //水曜日の緊急クエスト取得をする時のイベントハンドラ
        public event EventHandler Download;

        /*
        //緊急クエスト60分前のイベントハンドラ
        public event EventHandler emg60Before;

        //緊急クエスト30分前のイベントハンドラ
        public event EventHandler emg30Before;

        //緊急クエスト発生時のイベントハンドラ
        public event EventHandler emg0Before;
        */

        //緊急クエストの通知イベントハンドラ
        public event EventHandler emgNotify;

        //日付が変わった時のイベントハンドラ
        public event EventHandler newDay;

        //バル・ロドスの日が終わる30分前のイベントハンドラ
        public event EventHandler rodos30Before;

        //緊急クエスト情報
        private List<Event> pso2Event;

        //緊急取得のためのクラス
        public AbstractEventGetter emgGetter;

        //次の緊急クエスト
        Event nextEmg;
        bool notify;
        int nextInterval;
        DateTime nextNofity;

        //次の緊急の取得の時間
        DateTime nextReload;

        //日付が変わった時の通知
        DateTime nextDayNtf;

        //バル・ロドスの日フラグ
        bool rodosDay;
        DateTime rodosNotify;


        private Task EventLoopTask;

        public EventAdmin(AbstractEventGetter emgGetter)
        {
            pso2Event = new List<Event>();
            this.emgGetter = emgGetter;
            getEmgFromNet(emgGetter);
            setDailyPost();
            setNextGetTime();
            setRodosDay();

            this.EventLoopTask = startEventLoop();
        }

        public void setEmgEvent(List<Event> EventList)
        {
            pso2Event.Clear();

            foreach(Event evn in EventList)
            {
                Event tmp;

                if(evn is emgQuest)
                {
                    emgQuest emgTmp = evn as emgQuest;
                    tmp = new emgQuest(emgTmp.eventTime, emgTmp.eventName, emgTmp.live, emgTmp.liveEnable);
                    pso2Event.Add(tmp);
                }
                else
                {
                    if(evn is casino)
                    {
                        //casino cas = evn as casino;
                        tmp = new casino(evn.eventTime);
                        pso2Event.Add(tmp);
                    }
                    else
                    {
                        //エラー
                        System.Console.WriteLine("なにかがおかしい");
                    }
                }
            }

            setNextEmg();
            calcNextNofity();
        }

        public void getEmgFromNet(AbstractEventGetter emgGetter)    //緊急情報の取得
        {
            emgGetter.reloadPSO2Event();
            setEmgEvent(emgGetter.getPSO2Event());
        }

        private void calcNextNofity()   //次の通知の時間を計算
        {
            DateTime dt = DateTime.Now;
            //DateTime dt = new DateTime(2017, 8, 20, 23, 0, 0);
            TimeSpan ts30 = new TimeSpan(0, 30, 0);
            TimeSpan ts60 = new TimeSpan(1, 0, 0);

            if (DateTime.Compare(dt + ts30, nextEmg.eventTime) > 0) //次の通知は緊急発生時
            {
                nextInterval = 0;
                nextNofity = nextEmg.eventTime;
            }
            else
            {
                if (DateTime.Compare(dt, nextEmg.eventTime - ts60) < 0) //次の通知は緊急の1時間前
                {
                    nextInterval = 60;
                    nextNofity = nextEmg.eventTime - ts60;
                }
                else
                {
                    //次の通知は緊急の30分前
                    nextInterval = 30;
                    nextNofity = nextEmg.eventTime - ts30;
                }
            }

            //notify = true;
            logOutput.writeLog(string.Format("次の通知は{0}時{1}分の{2}です。", nextNofity.Hour, nextNofity.Minute,nextEmg.eventName));
        }

        private void setNextEmg()    //次の緊急クエストを更新
        {
            DateTime dt = DateTime.Now;
            notify = false;


            foreach (Event d in pso2Event)
            {
                if (DateTime.Compare(dt, d.eventTime) < 0 && d.GetType().Name == "emgQuest")
                {
                    //emgQuest emg = (emgQuest)d;
                    nextEmg = d;

                    notify = true;
                    logOutput.writeLog(string.Format("次の緊急は{0}時{1}分の\"{2}\"です。", nextEmg.eventTime.Hour, nextEmg.eventTime.Minute, nextEmg.eventName));
                    break;
                }
            }

            if (notify == false)
            {
                nextEmg = new emgQuest(dt - new TimeSpan(1, 0, 0), "なし");
                logOutput.writeLog("通知する緊急クエストがありません。");
            }
        }

        private void setDailyPost()
        {
            //日付が変わった時の通知の日を更新
            nextDayNtf = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0) + new TimeSpan(1, 0, 0, 0);
        }

        private void setNextGetTime()   //次の緊急クエストの取得時間を設定
        {
            int getDays = 7 - ((int)DateTime.Now.DayOfWeek + 4) % 7;
            if (getDays == 7)   //水曜日の時
            {
                DateTime dt1700 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0);   //今日の17:00
                if (DateTime.Compare(DateTime.Now, dt1700) <= 0)
                {
                    getDays = 0;
                }
            }

            nextReload = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0) + new TimeSpan(getDays, 0, 0, 0);
            logOutput.writeLog(string.Format("次の緊急クエストの取得は{0}月{1}日{2}時{3}分です。", nextReload.Month, nextReload.Day, nextReload.Hour, nextReload.Minute));
        }

        //バル・ロドスの通知設定
        private void setRodosDay()
        {
            rodosDay = rodosCalculator.calcRodosDay(DateTime.Now);
            if(rodosDay == true)
            {
                 rodosNotify = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 30, 0);
            }
        }

        public List<Event> getTodayEmg()    //今日の緊急クエスト一覧を取得
        {
            DateTime dt = DateTime.Now;
            DateTime toDay00 = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            DateTime toDay01 = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);

            List<Event> output = new List<Event>();

            foreach (Event ev in pso2Event)
            {
                if (DateTime.Compare(ev.eventTime, toDay00) >= 0 && DateTime.Compare(ev.eventTime, toDay01) < 0)
                {
                    if(ev is emgQuest)
                    {
                        emgQuest tmp = (emgQuest)ev;
                        output.Add(new emgQuest(tmp.eventTime,tmp.eventName,tmp.live,tmp.liveEnable));
                    }

                    if(ev is casino)
                    {
                        casino tmp = (casino)ev;
                        output.Add(new casino(tmp.eventTime));
                    }
                }
            }

            return output;
        }

        private async Task startEventLoop()  //イベントループを非同期で開始
        {
            await Task.Run(() => EventLoop());
        }

        private void EventLoop()    //イベントループ
        {
            while (true)
            {
                DateTime dt = DateTime.Now;


                if ((DateTime.Compare(dt, nextNofity) > 0) && notify == true)   //次の通知の時間を現在時刻が超えた時
                {
                    //通知のイベントを発生
                    emgEventData e = new emgEventData(nextEmg,nextInterval);
                    emgNotify(this,e);

                    if(nextInterval == 0)
                    {
                        setNextEmg();
                    }
                    calcNextNofity();
                }

                if (DateTime.Compare(dt, nextDayNtf) > 0) //日付が変わったら実行される
                {
                    setRodosDay();
                    setDailyPost();
                    //日付が変わった時のイベントを発生させる。
                    List<Event> todayEvent = getTodayEmg();
                    if(todayEvent.Count != 0 || rodosDay == true)
                    {
                        DailyEventList e = new DailyEventList(todayEvent, rodosDay);
                        newDay(this, e);
                    }
                }

                if (DateTime.Compare(dt, nextReload) > 0)   //水曜日17時になったら実行
                {
                    getEmgFromNet(emgGetter);
                    setNextGetTime();
                }

                if (rodosDay && DateTime.Compare(dt, rodosNotify) > 0)  //バル・ロドスの日23時30分の通知
                {
                    
                }

            }
        }
    }
}
