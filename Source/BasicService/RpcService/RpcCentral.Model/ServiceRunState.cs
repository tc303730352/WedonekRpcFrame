using SqlSugar;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Model
{
    public class ServiceRunState : IEquatable<ServiceRunState>
    {
        /// <summary>
        /// 服务节点Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }

        /// <summary>
        /// 链接数
        /// </summary>
        public int ConNum
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
        /// 线程数
        /// </summary>
        public int ThreadNum { get; set; }
        /// <summary>
        /// 获取当前活动的计时器数
        /// </summary>
        public long TimerNum { get; set; }
        /// <summary>
        /// GC信息
        /// </summary>
        [SqlSugar.SugarColumn(IsJson = true)]
        public GCBody GCBody { get; set; }

        /// <summary>
        /// 线程池状态
        /// </summary>
        [SugarColumn(IsJson = true)]
        public ThreadPoolState ThreadPool
        {
            get;
            set;
        }

        public DateTime SyncTime { get; set; }
        public override bool Equals (object obj)
        {
            if (obj is ServiceRunState i)
            {
                return i.ServerId == this.ServerId;
            }
            return false;
        }

        public bool Equals (ServiceRunState other)
        {
            if (other == null)
            {
                return false;
            }
            return other.ServerId == this.ServerId;
        }

        public override int GetHashCode ()
        {
            return this.ServerId.GetHashCode();
        }
    }
}
