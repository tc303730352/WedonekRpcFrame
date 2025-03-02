using WeDonekRpc.Model;
using SqlSugar;

namespace RpcSync.Model.DB
{
    [SugarTable("RemoteServerGroup")]
    public class RemoteServerGroupModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 集群Id
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 所属区域
        /// </summary>
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点类型
        /// </summary>
        public long SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点类型值
        /// </summary>
        public string TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        public RpcServerType ServiceType { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight
        {
            get;
            set;
        }
        /// <summary>
        /// 是否持有
        /// </summary>
        public bool IsHold
        {
            get;
            set;
        }
    }
}
