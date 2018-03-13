using System;
using System.Collections.Generic;
using System.Text;

namespace PSO2emergencyToDiscordCore
{
    class EventData : EventArgs
    {
        /*
         * eventType
         * 0 : 次の緊急クエスト
         * 1 : その日の緊急クエストの一覧
         * 2 : バル・ロドス23時20分
         */
        int eventType;

        public EventData(int type)
        {
            this.eventType = type;
        }
    }

    class emgEventData : EventData
    {
        Event emgData;
        int interval;

        public emgEventData(Event emg,int interval)    : base(0)
        {
            emgData = emg;
            this.interval = interval;
        }
    }

    class DailyEventList : EventData
    {
        List<Event> emgList;
        bool rodosDay;

        public DailyEventList(List<Event> lst,bool rodos) : base(1)
        {
            rodosDay = rodos;
            emgList = lst;
        }
    }

    
}
