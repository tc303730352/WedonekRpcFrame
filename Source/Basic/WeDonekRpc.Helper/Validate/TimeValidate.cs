using System;

namespace WeDonekRpc.Helper.Validate
{
    public class TimeAttrSet
    {
        public TimeAttrSet ()
        {
            this._IsNow = true;
        }
        public TimeAttrSet (int timestamp, TimeFormat format)
        {
            this._IsNow = true;
            this._Format = format;
            this._Timestamp = timestamp;
        }
        public TimeAttrSet (int timestamp, TimeFormat format, bool isDate)
        {
            this._IsNow = true;
            this._IsDate = isDate;
            this._Format = format;
            this._Timestamp = timestamp;
        }
        public TimeAttrSet (int timestamp)
        {
            this._IsNow = true;
            this._Timestamp = timestamp;
        }
        public TimeAttrSet (DateTime time)
        {
            this._Time = time;
        }
        public TimeAttrSet (string time)
        {
            this._Time = DateTime.Parse(time);
        }
        private readonly TimeFormat _Format = TimeFormat.日;
        private readonly DateTime _Time = DateTime.MinValue;

        private int _Timestamp = 0;

        private bool _IsNow = false;
        private readonly bool _IsDate = true;

        public DateTime Time
        {
            get
            {
                if (this._IsNow)
                {
                    DateTime now = this._IsDate ? DateTime.Now.Date : DateTime.Now;
                    if (this._Format == TimeFormat.秒)
                    {
                        return now.AddSeconds(this._Timestamp);
                    }
                    else if (this._Format == TimeFormat.月)
                    {
                        return now.AddMonths(this._Timestamp);
                    }
                    else if (this._Format == TimeFormat.日)
                    {
                        return now.AddDays(this._Timestamp);
                    }
                    else if (this._Format == TimeFormat.分)
                    {
                        return now.AddMinutes(this._Timestamp);
                    }
                    else if (this._Format == TimeFormat.小时)
                    {
                        return now.AddHours(this._Timestamp);
                    }
                    else
                    {
                        return now.AddYears(this._Timestamp);
                    }
                }
                return this._Time;
            }
        }
        public int Timestamp { get => this._Timestamp; set => this._Timestamp = value; }
        public bool IsNow { get => this._IsNow; set => this._IsNow = value; }

        public TimeFormat Format => this._Format;
    }
    public enum TimeFormat
    {
        年 = 5,
        月 = 4,
        日 = 3,
        小时 = 2,
        分 = 1,
        秒 = 0
    }
    /// <summary>
    /// 时间格式验证
    /// </summary>
    public class TimeValidate : ValidateAttr
    {
        private readonly TimeAttrSet _MinTime = new TimeAttrSet(DateTime.MinValue);

        private readonly TimeAttrSet _MaxTime = new TimeAttrSet(DateTime.MaxValue);

        public DateTime MinTime => this._MinTime.Time;
        public DateTime MaxTime => this._MaxTime.Time;
        public string Show
        {
            get;
            private set;
        }
        /// <summary>
        /// 时间验证
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="min">最小时间: yyyy-MM-dd HH:mm:ss</param>
        /// <param name="max">最大时间: yyyy-MM-dd HH:mm:ss</param>
        public TimeValidate (string error, string min, string max) : base(error)
        {
            this._MinTime = new TimeAttrSet(min);
            this._MaxTime = new TimeAttrSet(max);
            this.Show = string.Format("时间范围固定为：{0} 到 {1} 之间", min, max);
        }

        /// <summary>
        /// 时间验证
        /// </summary>
        /// <param name="error">错误信息</param>
        public TimeValidate (string error) : base(error)
        {
            this._MinTime = new TimeAttrSet();
            this.Show = "最小时间限定为当前时间，时间格式：yyyy-MM-dd";
        }
        /// <summary>
        /// 时间验证
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="min">最小时间</param>
        public TimeValidate (string error, int min) : base(error)
        {
            this._MinTime = new TimeAttrSet(min);
            this.Show = string.Format("最小时间为当天+{0}天后的时间，时间格式：yyyy-MM-dd", min);
        }
        /// <summary>
        /// 时间验证
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="format">时间格式</param>
        /// <param name="minTime">最小时间</param>
        public TimeValidate (string error, TimeFormat format, int minTime) : base(error)
        {
            this._MinTime = new TimeAttrSet(minTime, format);
            this.Show = string.Format("最小时间为当前时间加{0}{1}后的时间，时间格式： yyyy-MM-dd", minTime, format.ToString());
        }
        public TimeValidate (string error, TimeFormat format, int minTime, bool isDate) : base(error)
        {
            this._MinTime = new TimeAttrSet(minTime, format);
            this.Show = string.Format("最小时间为当前时间加{0}{1}后的时间，时间格式为：{2}", minTime, format.ToString(), isDate ? "yyyy-MM-dd" : "yyyy-MM-dd hh:mm:ss.ms");
        }
        /// <summary>
        /// 时间验证
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="isDate">是否是日期格式</param>
        /// <param name="minTime">最小时间：时间搓</param>
        public TimeValidate (string error, string maxTime) : base(error)
        {
            this._MaxTime = new TimeAttrSet(maxTime);
            this.Show = string.Format("最小时间不限，最大时间为当前时间加{0}天后的时间，时间格式为：yyyy-MM-dd", maxTime);
        }
        public TimeValidate (string error, int min, int max, TimeFormat format) : base(error)
        {
            this._MinTime = new TimeAttrSet(min, format);
            this._MaxTime = new TimeAttrSet(max, format);
            this.Show = string.Format("最小时间为当前时间加{0}{2}后的时间，最大时间为当前时间加{1}{2}后的时间，时间格式为：yyyy-MM-dd", min, max, format.ToString());
        }
        public TimeValidate (string error, int min, int max, TimeFormat format, bool isDate) : base(error)
        {
            this._MinTime = new TimeAttrSet(min, format);
            this._MaxTime = new TimeAttrSet(max, format);
            this.Show = string.Format("最小时间为当前时间加{0}{2}后的时间，最大时间为当前时间加{1}{2}后的时间，时间格式为：{3}", min, max, format.ToString(), isDate ? "yyyy-MM-dd" : "yyyy-MM-dd hh:mm:ss.ms");
        }
        /// <summary>
        /// 时间验证
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="maxTime">最大时间：时间搓</param>
        /// <param name="isDate">是否是日期格式</param>
        public TimeValidate (string error, int maxTime, TimeFormat format) : base(error)
        {
            this._MaxTime = new TimeAttrSet(maxTime, format);
        }
        public TimeValidate (string error, int maxTime, TimeFormat format, bool isDate) : base(error)
        {
            this._MaxTime = new TimeAttrSet(maxTime, format, isDate);
        }

        protected override bool _CheckAttr (object source, Type type, object data)
        {
            if (data == null)
            {
                return false;
            }
            return DataValidate.CheckTime(type, data, this);
        }
    }
}
