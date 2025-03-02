using System.Runtime.InteropServices;
using WeDonekRpc.Helper;
using WeDonekRpc.Model.Server;

namespace RpcStore.RemoteModel.RunState.Model
{
    /// <summary>
    /// 运行状态
    /// </summary>
    public class ServerRunState
    {
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 进程编号
        /// </summary>
        public int Pid
        {
            get;
            set;
        }
        /// <summary>
        /// 进程名
        /// </summary>
        public string PName { get; set; }
        /// <summary>
        /// 获取关联进程正在其上运行的计算机的名称
        /// </summary>
        public string MachineName
        {
            get;
            set;
        }
        /// <summary>
        /// 取得執行應用程式的 .NET 安裝名稱。
        /// </summary>
        public string Framework { get; set; }
        /// <summary>
        /// 操作系统类型
        /// </summary>
        public OSType OSType { get; set; }
        /// <summary>
        /// 取得目前應用程式執行所在的平台架構
        /// </summary>
        public Architecture OSArchitecture { get; set; }

        /// <summary>
        /// 取得字串，描述應用程式執行所在的作業系統
        /// </summary>
        public string OSDescription { get; set; }
        /// <summary>
        /// 取得字串，描述應用程式執行所在的作業系統
        /// </summary>
        public Architecture ProcessArchitecture { get; set; }

        /// <summary>
        /// 逻辑处理器数
        /// </summary>
        public int ProcessorCount { get; set; }

        /// <summary>
        /// 取得執行應用程式的平台
        /// </summary>
        public string RuntimeIdentifier { get; set; }

        /// <summary>
        /// 用户的身份标识
        /// </summary>
        public string RunUserIdentity { get; set; }

        /// <summary>
        /// 运行的所属用户组
        /// </summary>
        public string[] RunUserGroups { get; set; }

        /// <summary>
        /// 是否管理员或Root身份运行
        /// </summary>
        public bool RunIsAdmin { get; set; }

        /// <summary>
        /// 是否是小端
        /// </summary>
        public bool IsLittleEndian { get; set; }

        /// <summary>
        /// 系统启动时间
        /// </summary>
        public DateTime SystemStartTime { get; set; }
        /// <summary>
        /// 链接数
        /// </summary>
        public int ConNum { get; set; }
        /// <summary>
        /// 工作内存
        /// </summary>
        public long WorkMemory { get; set; }
        /// <summary>
        /// CPU占用时间
        /// </summary>
        public long CpuRunTime { get; set; }
        /// <summary>
        /// Cpu使用率
        /// </summary>
        public short CpuRate { get; set; }
        /// <summary>
        /// 获取尝试锁定监视器时出现争用的次数
        /// </summary>
        public long LockContentionCount { get; set; }
        /// <summary>
        /// 线程数
        /// </summary>
        public int ThreadNum { get; set; }
        /// <summary>
        /// 活动的Timer数
        /// </summary>
        public long TimerNum { get; set; }
        /// <summary>
        /// GC信息
        /// </summary>
        public GCBody GCBody { get; set; }
        /// <summary>
        /// 线程状态
        /// </summary>
        public ThreadPoolState ThreadPool { get; set; }
        /// <summary>
        /// 数据最后同步时间
        /// </summary>
        public DateTime SyncTime { get; set; }
        /// <summary>
        /// 进程启动时间
        /// </summary>
        public DateTime StartTime { get; set; }
    }
}
