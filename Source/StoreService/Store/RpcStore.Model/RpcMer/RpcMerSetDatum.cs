using SqlSugar;

namespace RpcStore.Model.RpcMer
{
    public class RpcMerSetDatum
    {
        /// <summary>
        /// 系统名
        /// </summary>
        public string SystemName
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
