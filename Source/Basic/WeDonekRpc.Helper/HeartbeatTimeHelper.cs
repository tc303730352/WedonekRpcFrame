using System;
using System.Threading;

namespace WeDonekRpc.Helper
{
    public class HeartbeatTimeHelper
    {
        private static readonly Timer _SyncTime;
        static HeartbeatTimeHelper ()
        {
            DateTime now = DateTime.Now;
            MonthOneDay = DateTime.Parse(now.ToString("yyyy-MM-01 00:00:00"));
            _HeartbeatTime = (int)( now - _BeginTime ).TotalSeconds;
            _Minutes = (int)( now - now.Date ).TotalMinutes;
            _Hour = now.Hour;
            _CurrentDateSpan = Tools.GetTimeSpan(now.Date);
            _CurrentDate = now.Date;
            _DayOfWeek = (int)now.DayOfWeek;
            _SyncTime = new Timer(_Init, null, 1000, 1000);
        }

        private static readonly DateTime _BeginTime = DateTime.Now.Date;


        private static void _Init (object state)
        {
            DateTime now = DateTime.Now;
            TimeSpan span = now - _BeginTime;
            _HeartbeatTime = (int)span.TotalSeconds;
            if (_HeartbeatTime % 60 == 0)
            {
                _Minutes = (int)( now - now.Date ).TotalMinutes;
            }
            if (now.Day == 1 && now.Month != MonthOneDay.Month)
            {
                MonthOneDay = DateTime.Parse(now.ToString("yyyy-MM-01 00:00:00"));
            }
            if (now.Hour != _Hour)
            {
                MonthOneDay = DateTime.Parse(now.ToString("yyyy-MM-01 00:00:00"));
                _DayOfWeek = (int)now.DayOfWeek;
                _Hour = now.Hour;
                _CurrentDate = now.Date;
                _CurrentDateSpan = Tools.GetTimeSpan(now.Date);
            }

        }
        public static DateTime MonthOneDay
        {
            get;
            private set;
        }


        private static volatile int _HeartbeatTime = 0;


        public static int HeartbeatTime
        {
            get => _HeartbeatTime;
            private set => _HeartbeatTime = value;
        }

        public static int DayOfWeek => _DayOfWeek;

        public static int Hour => _Hour;
        /// <summary>
        /// 当前日期
        /// </summary>
        public static DateTime CurrentDate => _CurrentDate;
        public static long CurrentDateSpan => _CurrentDateSpan;
        public static int Minutes => _Minutes;

        private static DateTime _CurrentDate;
        private static long _CurrentDateSpan;
        private static volatile int _DayOfWeek = (int)DateTime.Now.DayOfWeek;

        private static volatile int _Hour = 0;
        private static volatile int _Minutes = 0;
        public static DateTime GetTime ()
        {
            return _BeginTime.AddSeconds(_HeartbeatTime);
        }
        public static int GetTime (DateTime time)
        {
            return time == DateTime.MaxValue ? int.MaxValue : (int)( time - _BeginTime ).TotalSeconds;
        }
        public static DateTime GetTime (int timeSpan)
        {
            return _BeginTime.AddSeconds(timeSpan);
        }
    }
}
