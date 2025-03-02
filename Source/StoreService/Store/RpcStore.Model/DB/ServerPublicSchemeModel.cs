using RpcStore.RemoteModel;
using SqlSugar;

namespace RpcStore.Model.DB
{
    [SugarTable("ServerPublicScheme")]
    public class ServerPublicSchemeModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 所属集群
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 方案名
        /// </summary>
        public string SchemeName { get; set; }
        /// <summary>
        /// 方案说明
        /// </summary>
        public string SchemeShow { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public SchemeStatus Status { get; set; }

        public DateTime LastTime { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
