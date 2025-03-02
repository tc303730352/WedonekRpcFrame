using System;

namespace WeDonekRpc.Model.Server
{
    public class GCBody
    {
        /// <summary>
        /// 回收次数
        /// </summary>
        public int[] Recycle
        {
            get;
            set;
        }
        /// <summary>
        /// 获取指示这是否是压缩 GC 的值
        /// </summary>
        public bool Compacted
        {
            get;
            set;
        }

        /// <summary>
        /// 获取指示这是否是并发 GC (BGC) 的值
        /// </summary>
        public bool Concurrent
        {
            get;
            set;
        }

        /// <summary>
        /// 获取此 GC 回收的代系。 回收代系的同时也回收了更新的代系
        /// </summary>
        public int Generation
        {
            get;
            set;
        }

        /// <summary>
        /// 获取所有代的代系信息
        /// </summary>
        public GCGenerationInfo[] GenerationInfo
        {
            get;
            set;
        }
        /// <summary>
        /// 获取上次垃圾回收发生时的总堆大小
        /// </summary>
        public long HeapSizeBytes
        {
            get;
            set;
        }

        /// <summary>
        /// 获取上次垃圾回收发生时的高内存负载阈值
        /// </summary>
        public long HighMemoryLoadThresholdBytes
        {
            get;
            set;
        }

        /// <summary>
        /// 获取此 GC 的索引。 GC 索引从 1 开始，在 GC 开始时会增加。
        /// 由于此信息是在 GC 结束时更新的，这意味着，获得的后台 GC 信息包含的索引可能小于之前完成的前台 GC 的索引。
        /// </summary>
        public long Index
        {
            get;
            set;
        }

        /// <summary>
        /// 获取上次垃圾回收发生时的内存负载
        /// </summary>
        public long MemoryLoadBytes
        {
            get;
            set;
        }

        /// <summary>
        /// 获取暂停持续时间
        /// </summary>
        public TimeSpan[] PauseDurations
        {
            get;
            set;
        }

        /// <summary>
        /// 获取到目前为止暂停时间在 GC 中的百分比。
        /// 如果暂停时间为 1.2%，则此属性的值为 1.2。 此值的计算方法是：计算到目前为止所有 GC 暂停的总和，并将其除以加载运行时后该进程的总运行时间。
        /// 每次 GC 结束时更新这个正在运行的计数器。 
        /// 它不区分 GCKind。 
        /// 也就是说，在每个 GC 上，计算的值都会进行更新，当你访问此属性时，它将获取最新的计算值。
        /// </summary>
        public double PauseTimePercentage { get; set; }

        /// <summary>
        /// 获取此 GC 观察到的固定对象的数目
        /// </summary>
        public long PinnedObjectsCount { get; set; }

        /// <summary>
        /// 获取此 GC 的已提升字节数
        /// </summary>
        public long PromotedBytes { get; set; }

        /// <summary>
        /// 获取上次垃圾回收发生时垃圾回收器使用的总可用内存
        /// </summary>
        public long TotalAvailableMemoryBytes { get; set; }

        /// <summary>
        /// 获取托管堆的已提交字节总数
        /// </summary>
        public long TotalCommittedBytes { get; set; }
        /// <summary>
        /// 获取此 GC 观察到的已准备好进行终结的对象数
        /// </summary>
        public long FinalizationPendingCount { get; set; }
        /// <summary>
        /// 获取上次垃圾回收发生时的总片段数
        /// </summary>
        public long FragmentedBytes
        {
            get;
            set;
        }

    }
    public class ServiceHeartbeat
    {

        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 运行状态
        /// </summary>
        public RunState RunState
        {
            get;
            set;
        }
        /// <summary>
        /// 远程节点链接状态
        /// </summary>
        public RemoteState[] RemoteState { get; set; }
        public int VerNum { get; set; }
    }
}
