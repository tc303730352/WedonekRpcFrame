namespace RpcTaskModel
{
    public enum AutoTaskStatus
    {
        未知=-1,
        起草 = 0,
        启用 = 1,
        停用 = 2,
        错误 = 3
    }
    public enum TaskStep
    {
        停止执行 = 0,
        继续下一步 = 1,
        执行指定步骤 = 2
    }
    public enum TaskLogRange
    {
        关闭日志 = 0,
        记录错误 = 2,
        记录成功 = 4,
        全部 = 6
    }

    public enum TaskSendType
    {
        指令 = 0,
        Http = 1,
        广播 = 2
    }
    public enum TaskPlanType
    {
        循环任务 = 0,
        只执行一次 = 1
    }
    /// <summary>
    /// 任务执行周期
    /// </summary>
    public enum TaskExecRate
    {
        每天 = 0,
        每周 = 1,
        每月 = 2
    }
    /// <summary>
    /// 每天频率
    /// </summary>
    public enum TaskDayRate
    {
        执行一次 = 0,
        间隔执行 = 1
    }
    /// <summary>
    /// 间隔周期
    /// </summary>
    public enum TaskSpaceWeek
    {
        星期一 = 2,
        星期二 = 4,
        星期三 = 8,
        星期四 = 16,
        星期五 = 32,
        星期六 = 64,
        星期天 = 128
    }
    /// <summary>
    /// 天间隔类型
    /// </summary>
    public enum TaskDaySpaceType
    {
        小时 = 0,
        分 = 1,
        秒 = 2
    }
    /// <summary>
    /// 任务月间隔类型
    /// </summary>
    public enum TaskSpaceType
    {
        天 = 0,
        周 = 1
    }
}
