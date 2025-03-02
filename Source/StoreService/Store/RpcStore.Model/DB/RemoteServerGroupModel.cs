using WeDonekRpc.Model;
using SqlSugar;

namespace RpcStore.Model.DB
{
    /// <summary>
    /// 远程服务组别
    /// </summary>
    [SugarTable("RemoteServerGroup")]
    public class RemoteServerGroupModel
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        public int RegionId
        {
            get;
            set;
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        public long SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 类型值
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
