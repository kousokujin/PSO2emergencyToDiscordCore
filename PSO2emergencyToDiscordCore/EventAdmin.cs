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

        //緊急クエスト60分前のイベントハンドラ
        public event EventHandler emg60Before;

        //緊急クエスト30分前のイベントハンドラ
        public event EventHandler emg30Before;

        //緊急クエスト発生時のイベントハンドラ
        public event EventHandler emg0Before;

        //日付が変わった時のイベントハンドラ
        public event EventHandler newDay;

        //バル・ロドスの日が終わる30分前のイベントハンドラ
        public event EventHandler rodos30Before;

        //緊急クエスト情報
        private List<Event> pso2Event;

        private Task EventLoopTask;

        public EventAdmin(List<Event> EventList)
        {
            setEvent(EventList);
            this.EventLoopTask = startEventLoop();
        }

        public void setEvent(List<Event> EventList)
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
        }

        private async Task startEventLoop()  //イベントループを非同期で開始
        {
            await Task.Run(() => EventLoop());
        }

        private void EventLoop()
        {

        }
    }
}
