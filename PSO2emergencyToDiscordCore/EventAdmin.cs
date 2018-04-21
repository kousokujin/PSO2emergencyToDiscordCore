using System;
using System.Collections.Generic;
//using System.Text;
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

        //覇者の紋章通知のイベントハンドラ
        public event EventHandler chpNotify;

        //緊急クエスト情報
        private List<Event> pso2Event;

        //覇者の紋章キャンペーンリスト
        private List<string> chanpionList;

        //覇者の紋章通知時間リスト
        private List<DateTime> chpTimeList;
        private int nextChpTimeIndex;

        //緊急取得のためのクラス
        public AbstractEventGetter emgGetter;

        //覇者の紋章取得クラス
        public AbstractChanpionGetter chanpionGetter;

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

        //デバッグ用
        //public event EventHandler debugEvent;
        //public bool debug;

        public EventAdmin(AbstractEventGetter emgGetter,AbstractChanpionGetter chanpion)
        {
            pso2Event = new List<Event>();
            chpTimeList = new List<DateTime>();
            nextChpTimeIndex = 0;

            this.emgGetter = emgGetter;
            this.chanpionGetter = chanpion;
            getEmgFromNet();
            getChanpionFromNet();
            //setChpTimeList("chp.csv");
            setDailyPost();
            setRodosDay();

            this.EventLoopTask = startEventLoop();

            //debug = false;
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

            //pso2Event.Sort((a, b) => (a.eventTime - b.eventTime).Seconds);
            setNextEmg();
            calcNextNofity();
        }

        public void getEmgFromNet()    //緊急情報の取得
        {
            emgGetter.reloadPSO2Event();
            setEmgEvent(emgGetter.getPSO2Event());
            setNextGetTime();
        }

        public void addEmg(emgQuest emg)    //緊急クエストの追加
        {
            pso2Event.Add(emg);

            pso2Event.Sort((a, b) => (int)(a.eventTime - b.eventTime).TotalSeconds);

            string emgname = myFunction.getLiveEmgStr(emg);
            logOutput.writeLog("緊急クエスト「{0}」を{1}に追加しました。",emgname,emg.eventTime.ToString("MM/dd HH:mm"));
            setNextEmg();
            calcNextNofity();
        }

        public List<Event> getPSO2Event()
        {
            return pso2Event;
        }

        public void delPSO2Event(int index)
        {
            if (pso2Event.Count > index)
            {
                pso2Event.RemoveAt(index);
            }
        }

        public void getChanpionFromNet()
        {
            chanpionList = chanpionGetter.chanpion();
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

            if (notify == true)
            {
                logOutput.writeLog(string.Format("次の通知は{0}時{1}分の{2}です。", nextNofity.Hour, nextNofity.Minute, nextEmg.eventName));
            }
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

        //csvファイルから覇者の紋章キャンペーン通知時間リストを設定
        public void setChpTimeList(string filename)
        {
            chpTimeList.Clear();


            List<List<string>> csvList = ConvertFromCSV.getConvertCSV(filename);

            foreach(List<string> line in csvList)
            {
                int week;
                int hour;
                int min;
                int sec;

                bool intEnable = int.TryParse(line[0], out week);
                bool hourEnable = int.TryParse(line[1], out hour);
                bool minEnable = int.TryParse(line[2], out min);
                bool secEnable = int.TryParse(line[3], out sec);

                if(week > 7)
                {
                    break;
                }

                if(intEnable && hourEnable && minEnable && secEnable)
                {
                    if (week == 7)  //毎日
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            DateTime tmp = calcChpTime(i, hour, min, sec);
                            chpTimeList.Add(tmp);
                        }
                    }
                    else
                    {
                        DateTime tmp = calcChpTime(week, hour, min, sec);
                        chpTimeList.Add(tmp);
                    }
                }
            }

            chpTimeList.Sort();
            setNextChpNotify();
        }

        private DateTime calcChpTime(int week,int hour,int min,int sec)
        {
            DateTime now = DateTime.Now;
            DateTime tmp = new DateTime(now.Year, now.Month, now.Day, hour, min, sec);
            int nowWeek = (int)now.DayOfWeek;

            if (week > nowWeek)
            {
                tmp += new TimeSpan(week - nowWeek, 0, 0, 0);
            }

            if (week < nowWeek)
            {
                tmp += new TimeSpan(7 - nowWeek + week, 0, 0, 0);
            }

            if (week == nowWeek)
            {
                TimeSpan d = now - tmp;

                if (d.Seconds >= 0)  //正だったら来週
                {
                    tmp += new TimeSpan(7, 0, 0, 0);
                }
            }

            return tmp;
        }

        private void setNextChpNotify()    //次の覇者の紋章キャンペーン通知時間を設定
        {
            int index = 0;

            foreach(DateTime d in chpTimeList)
            {
                TimeSpan ts = DateTime.Now - d;

                if (ts.Seconds < 0)
                {
                    nextChpTimeIndex = index;
                    break;
                }

                index++;
            }
        }

        public string getChpList()
        {
            string output = "";
            int i = 0;

            foreach (DateTime t in chpTimeList)
            {
                output += t.ToString("MM/dd HH:mm:ss");

                if (chpTimeList.Count != i)
                {
                    output += "\n";
                }
                i++;
            }

            return output;
        }

        //バル・ロドスの通知設定
        private void setRodosDay()
        {
            rodosDay = rodosCalculator.calcRodosDay(DateTime.Now);
            if(rodosDay == true)
            {
                 rodosNotify = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 30, 0);
            }
            else
            {
                DateTime next = rodosCalculator.nextRodosDay(DateTime.Now);
                rodosNotify = new DateTime(next.Year, next.Month, next.Day, 23, 30, 0);
            }
        }

        public List<Event> getTodayEmg()    //今日の緊急クエスト一覧を取得
        {
            DateTime dt = DateTime.Now;
            TimeSpan OneDay = new TimeSpan(24, 0, 0);

            DateTime toDay00 = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            DateTime toDay01 = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0) + OneDay;

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
                    getEmgFromNet();
                    getChanpionFromNet();   //めんどいので覇者の紋章も一緒に取得
                    DailyEventList e = new DailyEventList(getTodayEmg(), rodosDay);
                    Download(this,e);
                }

                if (rodosDay == true && DateTime.Compare(dt, rodosNotify) > 0)  //バル・ロドスの日23時30分の通知
                {
                    EventData e = new EventData(2);
                    rodos30Before(this, e);
                    rodosDay = false;
                }

                if(chpTimeList.Count != 0 && DateTime.Compare(dt,chpTimeList[nextChpTimeIndex]) > 0)  //覇者の紋章通知
                {
                    chanpionList e = new chanpionList(chanpionList);
                    chpNotify(this, e);

                    chpTimeList[nextChpTimeIndex] += new TimeSpan(7, 0, 0, 0);  //一週間後
                    chpTimeList.Sort();
                    setNextChpNotify();

                }

                /*
                if(debug == true)   //デバッグ用
                {
                }
                */

                System.Threading.Thread.Sleep(10);

            }
        }
    }
}
