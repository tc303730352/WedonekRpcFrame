using System.Text;
using WeDonekRpc.Helper;
using RpcTaskModel;
using RpcTaskModel.TaskPlan.Model;

namespace AutoTask.Collect
{
    public static class LinqHelper
    {
        private static string _FormatDaySpaceNum (int num, TaskDaySpaceType spaceType)
        {
            if (spaceType == TaskDaySpaceType.秒)
            {
                return num.ToString();
            }
            else if (spaceType == TaskDaySpaceType.分)
            {
                return ( num / 60 ).ToString();
            }
            else
            {
                return ( num / 3600 ).ToString();
            }
        }
        private static void _LoadExecRate (StringBuilder str, TaskPlanSet plan)
        {
            if (plan.ExecRate == TaskExecRate.每天)
            {
                if (plan.ExecSpace.Value == 1)
                {
                    _ = str.Append("在每天的");
                }
                else
                {
                    _ = str.AppendFormat("在每{0}天的", plan.ExecSpace.Value);
                }
            }
            else if (plan.ExecRate == TaskExecRate.每周)
            {
                StringBuilder week = new StringBuilder();
                Array weeks = Enum.GetValues(typeof(TaskSpaceWeek));
                foreach (TaskSpaceWeek i in weeks)
                {
                    if (( i & plan.SpaceWeek.Value ) == i)
                    {
                        _ = week.Append(",");
                        _ = week.Append(i.ToString());
                    }
                }
                if (plan.ExecSpace.Value == 1)
                {
                    _ = str.AppendFormat("在每个{0}的", week.ToString());
                }
                else
                {
                    _ = str.AppendFormat("每 {0} 周在 {1}的", plan.ExecSpace.Value, week.ToString());
                }
            }
            else
            {
                if (plan.SpaceType == TaskSpaceType.天)
                {
                    if (plan.ExecSpace.Value == 1)
                    {
                        _ = str.AppendFormat("在每月第 {0} 天的", plan.SpaceDay.Value);
                    }
                    else
                    {
                        _ = str.AppendFormat("在每 {0} 个月于当月第 {1} 的", plan.ExecSpace.Value, plan.SpaceDay.Value);
                    }
                }
                else
                {
                    _ = str.AppendFormat("在每 {0} 个月于 第{1}个 {2} 的", plan.ExecSpace.Value, Tools.GetChinaNum(plan.SpeceNum.Value), plan.SpaceWeek.Value.ToString());
                }
            }
        }
        public static string GetPlanShow (this TaskPlanSet plan)
        {
            if (plan.PlanType == TaskPlanType.只执行一次)
            {
                return string.Format("在 {0} 执行一次", plan.ExecTime.Value.ToString("yyyy年MM月dd日 HH小时mm分ss秒"));
            }
            StringBuilder str = new StringBuilder();
            _LoadExecRate(str, plan);
            if (plan.DayRate == TaskDayRate.执行一次)
            {
                _ = str.Append(Tools.FormatSecond(plan.DayTimeSpan.Value));
                _ = str.Append(" 执行。");
            }
            else
            {
                _ = str.AppendFormat(" {0} 和 {1} 之间，每 {2} {3}执行一次。",
                        Tools.FormatSecond(plan.DayBeginSpan.Value),
                        Tools.FormatSecond(plan.DayEndSpan.Value),
                        _FormatDaySpaceNum(plan.DaySpaceNum.Value, plan.DaySpaceType),
                        plan.DaySpaceType.ToString());
            }
            if (plan.EndDate.HasValue)
            {
                _ = str.AppendFormat("将在 {0} 到 {1} 之间使用计划。",
                        plan.BeginDate.ToString("yyyy/MM/dd"),
                        plan.EndDate.Value.ToString("yyyy/MM/dd"));
            }
            else
            {
                _ = str.AppendFormat("将从 {0} 开始使用计划。",
                      plan.BeginDate.ToString("yyyy/MM/dd"));
            }
            return str.ToString();
        }
        public static string GetPlanOnlyNo (this TaskPlanSet plan)
        {
            if (plan.PlanType == TaskPlanType.只执行一次)
            {
                return plan.ExecTime.Value.ToString("yyyyMMddHHmmss").GetMd5();
            }
            List<string> list = new List<string>(11)
            {
                    ((int)plan.ExecRate).ToString()
            };
            if (plan.ExecRate == TaskExecRate.每周)
            {
                list.Add(plan.ExecSpace.Value.ToString());
                list.Add(plan.SpaceWeek.Value.ToString());
            }
            else if (plan.ExecRate == TaskExecRate.每天)
            {
                list.Add(plan.ExecSpace.Value.ToString());
            }
            else
            {
                list.Add(( (int)plan.SpaceType ).ToString());
                if (plan.SpaceType == TaskSpaceType.天)
                {
                    list.Add(plan.SpaceDay.Value.ToString());
                }
                else
                {
                    list.Add(plan.SpeceNum.Value.ToString());
                    list.Add(plan.SpaceWeek.Value.ToString());
                }
            }
            list.Add(( (int)plan.DayRate ).ToString());
            if (plan.DayRate == TaskDayRate.执行一次)
            {
                list.Add(plan.DayTimeSpan.Value.ToString());
            }
            else
            {
                list.Add(plan.DayBeginSpan.Value.ToString());
                list.Add(plan.DayEndSpan.Value.ToString());
                list.Add(plan.DaySpaceNum.Value.ToString());
                list.Add(( (int)plan.DaySpaceType ).ToString());
            }
            list.Add(plan.BeginDate.ToString("yyyyMMddHHmmss"));
            if (plan.EndDate.HasValue)
            {
                list.Add(plan.EndDate.Value.ToString("yyyyMMddHHmmss"));
            }
            return list.Join("_").GetMd5();
        }
    }
}
