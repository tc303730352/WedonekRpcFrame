using WeDonekRpc.Model.Server;

namespace RpcStore.Model.DB
{
    [SqlSugar.SugarTable("ServerEnvironment")]
    public class ServerEnvironmentModel
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        public long ServerId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否为64位系统
        /// </summary>
        public bool Is64BitOperatingSystem { get; set; }

        /// <summary>
        /// 当前进程是否被授权执行
        /// </summary>
        public bool IsPrivilegedProcess { get; set; }

        /// <summary>
        /// 当前进程是否为64位进程
        /// </summary>
        public bool Is64BitProcess { get; set; }

        /// <summary>
        /// 当前平台标识符和版本号
        /// </summary>
        public string OSVersion { get; set; }

        /// <summary>
        /// 进程的退出代码。
        /// </summary>
        public int ExitCode { get; set; }

        /// <summary>
        /// 此进程的命令行
        /// </summary>
        public string CommandLine { get; set; }

        /// <summary>
        /// 操作系统内存页中的字节数
        /// </summary>
        public int SystemPageSize { get; set; }

        /// <summary>
        /// 获取与当前用户关联的网络域名
        /// </summary>
        public string UserDomainName { get; set; }

        /// <summary>
        /// 该值指示当前进程是否在用户交互中运行
        /// </summary>
        public bool UserInteractive { get; set; }
        /// <summary>
        /// 获取与当前线程关联的人员的用户名
        /// </summary>
        public string UserName { get; set; }
        //
        /// <summary>
        /// 获取一个版本，该版本由的主要、次要、内部版本号和修订号组成
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 逻辑驱动
        /// </summary>
        [SqlSugar.SugarColumn(IsJson = true)]
        public string[] LogicalDrives { get; set; }


        [SqlSugar.SugarColumn(IsJson = true)]
        public Dictionary<string, string> EnvironmentVariables { get; set; }

        /// <summary>
        /// 主模块
        /// </summary>
        [SqlSugar.SugarColumn(IsJson = true)]
        public ProcModule MainModule { get; set; }
        /// <summary>
        /// 模块列表
        /// </summary>
        [SqlSugar.SugarColumn(IsJson = true)]
        public ProcModule[] Modules { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public DateTime SyncTime { get; set; }
    }
}
