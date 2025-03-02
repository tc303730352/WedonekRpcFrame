namespace WeDonekRpc.Model.Server
{
    public class ThreadPoolState
    {
        public int ThreadCount { get; set; }
        public long CompletedWorkItemCount { get; set; }
        public long PendingWorkItemCount { get; set; }
        public int MaxWorker { get; set; }
        public int MaxCompletionPort { get; set; }
        public int MinWorker { get; set; }
        public int MinCompletionPort { get; set; }
        public int AvailableCompletionPort { get; set; }
        public int AvailableWorker { get; set; }
    }
    /// <summary>
    /// 远程系统状态
    /// </summary>
    public class RunState
    {

        /// <summary>
        /// 链接数
        /// </summary>
        public int ConNum
        {
            get;
            set;
        }
        /// <summary>
        /// 线程池状态
        /// </summary>
        public ThreadPoolState ThreadPool
        {
            get;
            set;
        }
        /// <summary>
        /// 占用内存
        /// </summary>
        public long WorkMemory
        {
            get;
            set;
        }
        /// <summary>
        /// CPU占用率
        /// </summary>
        public short CpuRate { get; set; }
        /// <summary>
        /// Cpu运行时间
        /// </summary>
        public long CpuRunTime { get; set; }
        /// <summary>
        /// 获取尝试锁定监视器时出现争用的次数
        /// </summary>
        public long LockContentionCount { get; set; }
        /// <summary>
        /// GC信息
        /// </summary>
        public GCBody GCBody { get; set; }
        /// <summary>
        /// 线程数
        /// </summary>
        public int ThreadNum { get; set; }
        /// <summary>
        /// 获取当前活动的计时器数
        /// </summary>
        public long TimerNum { get; set; }
    }
}
