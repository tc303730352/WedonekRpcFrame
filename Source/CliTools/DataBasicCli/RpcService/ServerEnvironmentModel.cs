using SqlSugar;

namespace DataBasicCli.RpcService
{

    [SqlSugar.SugarTable("ServerEnvironment", TableDescription = "服务节点环境配置")]
    [SugarIndex("IX_ServerId", "ServerId", OrderByType.Asc, true)]
    public class ServerEnvironmentModel
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识ID")]
        public long Id
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "服务节点id")]
        public long ServerId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否为64位系统
        /// </summary>
        [SugarColumn(ColumnDescription = "是否为64位系统")]
        public bool Is64BitOperatingSystem { get; set; }

        /// <summary>
        /// 当前进程是否被授权执行
        /// </summary>
        [SugarColumn(ColumnDescription = "当前进程是否被授权执行")]
        public bool IsPrivilegedProcess { get; set; }

        /// <summary>
        /// 当前进程是否为64位进程
        /// </summary>
        [SugarColumn(ColumnDescription = "当前进程是否为64位进程")]
        public bool Is64BitProcess { get; set; }

        /// <summary>
        /// 当前平台标识符和版本号
        /// </summary>
        [SugarColumn(ColumnDescription = "当前平台标识符和版本号", Length = 100, ColumnDataType = "varchar")]
        public string OSVersion { get; set; }

        /// <summary>
        /// 进程的退出代码。
        /// </summary>
        [SugarColumn(ColumnDescription = "进程的退出代码")]
        public int ExitCode { get; set; }

        /// <summary>
        /// 此进程的命令行
        /// </summary>
        [SugarColumn(ColumnDescription = "此进程的命令行", Length = 1000, ColumnDataType = "varchar")]
        public string CommandLine { get; set; }

        /// <summary>
        /// 操作系统内存页中的字节数
        /// </summary>
        [SugarColumn(ColumnDescription = "操作系统内存页中的字节数")]
        public int SystemPageSize { get; set; }

        /// <summary>
        /// 获取与当前用户关联的网络域名
        /// </summary>
        [SugarColumn(ColumnDescription = "获取与当前用户关联的网络域名", Length = 50)]
        public string UserDomainName { get; set; }

        /// <summary>
        /// 该值指示当前进程是否在用户交互中运行
        /// </summary>
        [SugarColumn(ColumnDescription = "该值指示当前进程是否在用户交互中运行")]
        public bool UserInteractive { get; set; }
        /// <summary>
        /// 获取与当前线程关联的人员的用户名
        /// </summary>
        [SugarColumn(ColumnDescription = "获取与当前线程关联的人员的用户名", Length = 50)]
        public string UserName { get; set; }
        //
        /// <summary>
        /// 获取一个版本，该版本由的主要、次要、内部版本号和修订号组成
        /// </summary>
        [SugarColumn(ColumnDescription = "该版本号由的主要、次要、内部版本号和修订号组成", Length = 50)]
        public string Version { get; set; }

        /// <summary>
        /// 逻辑驱动
        /// </summary>
        [SugarColumn(ColumnDescription = "逻辑驱动", Length = 200)]
        public string LogicalDrives { get; set; }


        [SugarColumn(ColumnDescription = "环境变量")]
        public string EnvironmentVariables { get; set; }

        /// <summary>
        /// 主模块
        /// </summary>
        [SugarColumn(ColumnDescription = "主模块信息", ColumnDataType = "varchar", IsNullable = true, Length = 1500)]
        public string MainModule { get; set; }
        /// <summary>
        /// 模块列表
        /// </summary>
        [SugarColumn(ColumnDescription = "加载的模块列表", ColumnDataType = "text")]
        public string Modules { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        [SugarColumn(ColumnDescription = "同步时间")]
        public DateTime SyncTime { get; set; }
    }
}
