using RpcStore.RemoteModel.TransmitScheme.Model;
using SqlSugar;

namespace RpcStore.Model.DB
{
    [SugarTable("ServerTransmitConfig")]
    public class ServerTransmitConfigModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        public long SchemeId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点组别编号
        /// </summary>
        public string ServerCode
        {
            get;
            set;
        }
        /// <summary>
        /// 负载配置
        /// </summary>
        [SugarColumn(IsJson = true)]
        public TransmitConfig[] TransmitConfig
        {
            get;
            set;
        }
    }
}
