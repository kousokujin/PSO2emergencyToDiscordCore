using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace PSO2emergencyToDiscordCore
{
    abstract class Event
    {
        public DateTime eventTime;
        public string eventName;

        public Event(DateTime time,string evnt)
        {
            this.eventTime = time;
            this.eventName = evnt;
        }
    }

    class emgQuest : Event  //緊急クエスト
    {

        public string live;
        public bool liveEnable;

        public emgQuest(DateTime time, string evant) : base(time, evant)
        {
            liveEnable = false;
            live = "";
        }

        public emgQuest(DateTime time,string Event,string liveName) : base(time, Event)
        {
            live = liveName;
            liveEnable = true;
        }

        public emgQuest(DateTime time,string Event,string livenName,bool liveEnable) : base(time, Event)
        {
            live = livenName;
            this.liveEnable = liveEnable;

        }
    }

    class casino : Event    //カジノイベント
    {
        public casino(DateTime time) : base(time,"カジノイベント"){

        }
    }
}
