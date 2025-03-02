using SqlSugar;

namespace DataBasicCli.RpcService
{
    /// <summary>
    /// 服务节点状态
    /// </summary>
    [SugarTable("ServerRunState", TableDescription = "服务节点状态表")]
    public class ServerRunStateModel
    {
        /// <summary>
        /// 服务节点Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "服务节点ID")]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 进程编号
        /// </summary>
        [SugarColumn(ColumnDescription = "进程编号")]
        public int Pid
        {
            get;
            set;
        }
        /// <summary>
        /// 进程名
        /// </summary>
        [SugarColumn(ColumnDescription = "进程名", Length = 50)]
        public string PName { get; set; }
        /// <summary>
        /// 获取关联进程正在其上运行的计算机的名称
        /// </summary>
        [SugarColumn(ColumnDescription = "关联进程正在其上运行的计算机的名称", Length = 50)]
        public string MachineName
        {
            get;
            set;
        }
        /// <summary>
        /// 取得執行應用程式的 .NET 安裝名稱。
        /// </summary>
        [SugarColumn(ColumnDescription = ".NET 版本", Length = 50, ColumnDataType = "varchar")]
        public string Framework { get; set; }
        /// <summary>
        /// 操作系统类型
        /// </summary>
        [SugarColumn(ColumnDescription = "操作系统类型")]
        public byte OSType { get; set; }
        /// <summary>
        /// 取得目前應用程式執行所在的平台架構
        /// </summary>
        [SugarColumn(ColumnDescription = "取得目前應用程式執行所在的平台架構")]
        public byte OSArchitecture { get; set; }

        /// <summary>
        /// 取得字串，描述應用程式執行所在的作業系統
        /// </summary>
        [SugarColumn(ColumnDescription = "描述應用程式執行所在的作業系統", Length = 150, ColumnDataType = "varchar")]
        public string OSDescription { get; set; }
        /// <summary>
        /// 取得字串，描述應用程式執行所在的作業系統
        /// </summary>
        [SugarColumn(ColumnDescription = "描述應用程式執行所在的作業系統")]
        public byte ProcessArchitecture { get; set; }
        /// <summary>
        /// 逻辑处理器数
        /// </summary>
        [SugarColumn(ColumnDescription = "逻辑处理器数")]
        public int ProcessorCount { get; set; }
        /// <summary>
        /// 取得執行應用程式的平台
        /// </summary>
        [SugarColumn(ColumnDescription = "取得執行應用程式的平台", Length = 50, ColumnDataType = "varchar")]
        public string RuntimeIdentifier { get; set; }
        /// <summary>
        /// 用户的身份标识
        /// </summary>
        [SugarColumn(ColumnDescription = "用户的身份标识", Length = 50, ColumnDataType = "varchar")]
        public string RunUserIdentity { get; set; }
        /// <summary>
        /// 运行的所属用户组
        /// </summary>
        [SugarColumn(ColumnDescription = "运行的所属用户组", Length = 200, ColumnDataType = "varchar")]
        public string RunUserGroups { get; set; }
        /// <summary>
        /// 是否管理员或Root身份运行
        /// </summary>
        [SugarColumn(ColumnDescription = "是否管理员或Root身份运行")]
        public bool RunIsAdmin { get; set; }
        /// <summary>
        /// 是否小字节
        /// </summary>
        [SugarColumn(ColumnDescription = "是否小字节")]
        public bool IsLittleEndian { get; set; }
        /// <summary>
        /// 系统启动时间
        /// </summary>
        [SugarColumn(ColumnDescription = "系统启动时间")]
        public DateTime SystemStartTime { get; set; }
        /// <summary>
        /// 链接数
        /// </summary>
        [SugarColumn(ColumnDescription = "链接数")]
        public int ConNum { get; set; }
        /// <summary>
        /// 工作内存
        /// </summary>
        [SugarColumn(ColumnDescription = "工作内存")]
        public long WorkMemory { get; set; }
        /// <summary>
        /// CPU占用时间
        /// </summary>
        [SugarColumn(ColumnDescription = "CPU占用时间")]
        public long CpuRunTime { get; set; }
        /// <summary>
        /// Cpu使用率
        /// </summary>
        [SugarColumn(ColumnDescription = "Cpu使用率")]
        public short CpuRate { get; set; }

        /// <summary>
        /// 获取尝试锁定监视器时出现争用的次数
        /// </summary>
        [SugarColumn(ColumnDescription = "获取尝试锁定监视器时出现争用的次数")]
        public long LockContentionCount { get; set; }
        /// <summary>
        /// 线程数
        /// </summary>
        [SugarColumn(ColumnDescription = "线程数")]
        public int ThreadNum { get; set; }

        /// <summary>
        /// 获取当前活动的计时器数
        /// </summary>
        [SugarColumn(ColumnDescription = "获取当前活动的计时器数")]
        public long TimerNum { get; set; }
        /// <summary>
        /// GC信息
        /// </summary>
        [SugarColumn(ColumnDescription = "GC信息", IsNullable = true)]
        public string GCBody { get; set; }

        /// <summary>
        /// 线程池状态
        /// </summary>
        [SugarColumn(ColumnDescription = "线程池状态", IsNullable = true)]
        public string ThreadPool
        {
            get;
            set;
        }

        /// <summary>
        /// 数据最后同步时间
        /// </summary>
        [SugarColumn(ColumnDescription = "数据最后同步时间")]
        public DateTime SyncTime { get; set; }
        /// <summary>
        /// 进程启动时间
        /// </summary>
        [SugarColumn(ColumnDescription = "进程启动时间")]
        public DateTime StartTime { get; set; }
    }
}
