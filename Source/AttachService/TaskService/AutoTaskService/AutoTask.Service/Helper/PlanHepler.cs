using WeDonekRpc.Helper;
using RpcTaskModel;
using RpcTaskModel.TaskPlan.Model;

namespace AutoTask.Service.Helper
{
    internal class PlanHepler
    {
        private static readonly int[] _Week = Enum.GetValues(typeof(TaskSpaceWeek)).ConvertAll(a => (int)a);

        public static DateTime GetNextTime (TaskPlanBasic plan, DateTime time)
        {
            if (plan.DayRate == TaskDayRate.执行一次)
            {
                return _GetNextTime(plan, time);
            }
            else
            {
                DateTime next = time.AddSeconds(plan.DayTimeSpan.Value);
                if (next == HeartbeatTimeHelper.CurrentDate)
                {
                    if (plan.DayEndSpan.HasValue)
                    {
                        int sec = (int)( next - HeartbeatTimeHelper.CurrentDate ).TotalSeconds;
                        if (sec <= plan.DayEndSpan.Value)
                        {
                            return next;
                        }
                    }
                    else
                    {
                        return next;
                    }
                }
                if (plan.DayBeginSpan.Value != 0)
                {
                    next = _GetNextTime(plan, time.Date);
                    return next.AddSeconds(plan.DayBeginSpan.Value);
                }
                else if (plan.ExecRate == TaskExecRate.每天 && plan.ExecSpace == 1)
                {
                    return next;
                }
                else
                {
                    return _GetNextTime(plan, time.Date);
                }
            }
        }
        public static DateTime GetExecTime (TaskPlanBasic plan, DateTime? execTime)
        {
            if (plan.PlanType == TaskPlanType.只执行一次)
            {
                return plan.ExecTime.Value;
            }
            DateTime date = _GetExecDate(plan, execTime);
            if (plan.DayRate == TaskDayRate.执行一次)
            {
                date = date.AddSeconds(plan.DayTimeSpan.Value);
                if (date < DateTime.Now)
                {
                    return _GetNextTime(plan, date);
                }
                return date;
            }
            else if (date != HeartbeatTimeHelper.CurrentDate)
            {
                return date.AddSeconds(plan.DayBeginSpan.Value);
            }
            else
            {
                int sec = (int)( DateTime.Now - date ).TotalSeconds;
                if (sec <= plan.DayBeginSpan)
                {
                    return date.AddSeconds(plan.DayBeginSpan.Value);
                }
                sec -= plan.DayBeginSpan.Value;
                sec = plan.DayBeginSpan.Value + ( plan.DaySpaceNum.Value * (int)Math.Ceiling((double)sec / plan.DaySpaceNum.Value) );
                if (!plan.DayEndSpan.HasValue || sec <= plan.DayEndSpan.Value)
                {
                    return date.AddSeconds(sec);
                }
                date = date.AddSeconds(plan.DayBeginSpan.Value);
                return _GetNextTime(plan, date);
            }
        }
        private static DateTime _GetNextDate (TaskPlanBasic plan)
        {
            DateTime now = HeartbeatTimeHelper.CurrentDate;
            DateTime date = new DateTime(now.Year, now.Month, plan.SpaceDay.Value);
            if (date >= now)
            {
                return date;
            }
            return date.AddMonths(1);
        }
        private static DateTime _GetNextWeekDate (TaskPlanBasic plan, DateTime now)
        {
            DateTime date = new DateTime(now.Year, now.Month, 1);
            TaskSpaceWeek week = _GetWeek(date.DayOfWeek);
            if (week != plan.SpaceWeek.Value)
            {
                int add = plan.SpaceWeek.Value - week;
                if (add > 0)
                {
                    date = date.AddDays(add);
                }
                else
                {
                    date = date.AddDays(7 + add);
                }
            }
            if (plan.SpeceNum.Value != 1)
            {
                return date.AddDays(( plan.SpeceNum.Value - 1 ) * 7);
            }
            return date;
        }
        private static DateTime _GetExecWeek (TaskPlanBasic plan, DateTime date)
        {
            if (plan.BeginDate > date)
            {
                date = plan.BeginDate;
            }
            TaskSpaceWeek week = _GetWeek(date.DayOfWeek);
            if (week == plan.SpaceWeek.Value)
            {
                return date;
            }
            int val = (int)plan.SpaceWeek.Value;
            int w = (int)week;
            double dou = Math.Log(val, 2);
            if (dou % 1 == 0)
            {
                int sp = (int)dou - w;
                return date.AddDays(sp);
            }
            else if (week == TaskSpaceWeek.星期天)
            {
                int min = (int)Math.Log(_Week.Find(a => ( a & val ) == a), 2);
                return date.AddDays(min);
            }
            int num = _Week.Find(a => ( a & val ) == a && a > w);
            if (num == 0)
            {
                num = (int)Math.Log(_Week.Find(a => ( a & val ) == a), 2);
            }
            return date.AddDays(num - w);
        }
        private static DateTime _GetNextWeek (TaskPlanBasic plan, DateTime date)
        {
            int val = (int)plan.SpaceWeek.Value;
            double dou = Math.Log(val, 2);
            if (dou % 1 == 0)
            {
                return date.AddDays(plan.ExecSpace.Value * 7);
            }
            int[] list = _Week.FindAll(c => ( c & val ) == c);
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return date.AddDays((int)Math.Log(list[0], 2) + ( ( plan.ExecSpace.Value - 1 ) * 7 ));
            }
            int week = (int)Math.Log((int)_GetWeek(date.DayOfWeek), 2);
            int num = (int)Math.Log(list.Find(a => a > week, list[0]), 2) - week;
            if (num > 0)
            {
                return date.AddDays(num);
            }
            else
            {
                return date.AddDays(num + ( plan.ExecSpace.Value * 7 ));
            }
        }
        private static DateTime _GetNextWeek (TaskPlanBasic plan, DateTime date, int add)
        {
            int val = (int)plan.SpaceWeek.Value;
            double dou = Math.Log(val, 2);
            if (dou % 1 == 0)
            {
                return date.AddDays(add * 7);
            }
            int min = _Week.Find(a => ( a & val ) == a);
            int week = (int)_GetWeek(date.DayOfWeek);
            if (min == week)
            {
                return date.AddDays(add * 7);
            }
            min = (int)Math.Log(min, 2) - (int)Math.Log(week, 2);
            return date.AddDays(min + ( add * 7 ));
        }
        private static DateTime _GetExecDate (TaskPlanBasic plan, DateTime? execTime)
        {
            if (plan.ExecRate == TaskExecRate.每周)
            {
                DateTime date = _GetExecWeek(plan, HeartbeatTimeHelper.CurrentDate);
                if (!execTime.HasValue)
                {
                    return date;
                }
                DateTime time = execTime.Value;
                if (time.DayOfWeek != date.DayOfWeek)
                {
                    int add = _ToWeekNum(date.DayOfWeek, time.DayOfWeek);
                    time = time.AddDays(add);
                }
                int week = plan.ExecSpace.Value - ( ( date - time ).Days / 7 );
                if (week <= 1)
                {
                    return date;
                }
                return _GetNextWeek(plan, date, week);
            }
            else if (plan.ExecRate == TaskExecRate.每天)
            {
                if (!execTime.HasValue)
                {
                    return HeartbeatTimeHelper.CurrentDate;
                }
                DateTime date = HeartbeatTimeHelper.CurrentDate;
                int day = plan.ExecSpace.Value - ( date - execTime.Value.Date ).Days;
                if (day <= 1)
                {
                    return date;
                }
                return date.AddDays(day);
            }
            else if (plan.SpaceType == TaskSpaceType.天)
            {
                DateTime date = _GetNextDate(plan);
                if (!execTime.HasValue)
                {
                    return date;
                }
                int month = plan.ExecSpace.Value - ( date.Month - execTime.Value.Month );
                if (month <= 1)
                {
                    return date;
                }
                return date.AddMonths(month);
            }
            else
            {
                DateTime date = _GetNextWeekDate(plan, HeartbeatTimeHelper.CurrentDate);
                if (!execTime.HasValue)
                {
                    return date;
                }
                int month = plan.ExecSpace.Value - ( date.Month - execTime.Value.Month );
                if (month <= 1)
                {
                    return date;
                }
                date = date.AddMonths(month);
                return _GetNextWeekDate(plan, date);
            }
        }
        private static int _ToWeekNum (DayOfWeek one, DayOfWeek two)
        {
            return ( one == DayOfWeek.Saturday ? 7 : (int)one ) - ( two == DayOfWeek.Sunday ? 7 : (int)two );
        }

        private static DateTime _GetNextTime (TaskPlanBasic plan, DateTime time)
        {
            if (plan.ExecRate == TaskExecRate.每周)
            {
                return _GetNextWeek(plan, time);
            }
            else if (plan.ExecRate == TaskExecRate.每天)
            {
                return time.AddDays(plan.ExecSpace.Value);
            }
            else if (plan.SpaceType == TaskSpaceType.天)
            {
                return time.AddMonths(plan.ExecSpace.Value);
            }
            else
            {
                time = time.AddMonths(plan.ExecSpace.Value);
                return _GetNextWeekDate(plan, time);
            }
        }
        private static TaskSpaceWeek _GetWeek (DayOfWeek week)
        {
            if (week == DayOfWeek.Sunday)
            {
                return TaskSpaceWeek.星期天;
            }
            int num = (int)week;
            return (TaskSpaceWeek)Math.Pow(2, num);
        }
    }
}
