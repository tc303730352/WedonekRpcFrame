using SqlSugar;

namespace RpcStore.Model.DB
{
    [SugarTable("RpcMer")]
    public class RpcMerModel
    {
        /// <summary>
        /// 集群Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 系统名
        /// </summary>
        public string SystemName
        {
            get;
            set;
        }
        /// <summary>
        /// 应用APPId
        /// </summary>
        public string AppId
        {
            get;
            set;
        }
        /// <summary>
        /// 应用秘钥
        /// </summary>
        public string AppSecret
        {
            get;
            set;
        }
        /// <summary>
        /// 允许访问的服务器IP
        /// </summary>
        [SugarColumn(IsJson = true)]
        public string[] AllowServerIp
        {
            get;
            set;
        }
    }
}
