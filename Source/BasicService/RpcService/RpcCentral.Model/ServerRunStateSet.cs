using System.Runtime.InteropServices;
using WeDonekRpc.Helper;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Model
{
    public class ServerRunStateSet
    {
        /// <summary>
        /// 进程PID
        /// </summary>
        public int Pid
        {
            get;
            set;
        }
        /// <summary>
        /// 进程名
        /// </summary>
        public string PName
        {
            get;
            set;
        }
        /// <summary>
        /// 进程启动时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 占用内存
        /// </summary>
        public long WorkMemory
        {
            get;
            set;
        }
        /// <summary>
        /// 获取关联进程正在其上运行的计算机的名称
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// 运行系统类型
        /// </summary>
        public OSType OSType { get; set; }
        /// <summary>
        /// 取得執行應用程式的 .NET 安裝名稱。
        /// </summary>
        public string Framework { get; set; }
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
        /// 取得執行應用程式的平台
        /// </summary>
        public string RuntimeIdentifier { get; set; }
        /// <summary>
        /// 运行的用户身份标识
        /// </summary>
        [SqlSugar.SugarColumn(IsJson = true, ColumnDataType = "varchar(50)")]
        public string[] RunUserGroups { get; set; }
        /// <summary>
        /// 用户的身份标识
        /// </summary>
        public string RunUserIdentity { get; set; }
        /// <summary>
        /// 是否管理员或Root身份运行
        /// </summary>
        public bool RunIsAdmin { get; set; }
        /// <summary>
        /// 是否是小端
        /// </summary>
        public bool IsLittleEndian { get; set; }
        [SqlSugar.SugarColumn(IsJson = true)]
        public GCBody GCBody { get; set; }
    }
}
