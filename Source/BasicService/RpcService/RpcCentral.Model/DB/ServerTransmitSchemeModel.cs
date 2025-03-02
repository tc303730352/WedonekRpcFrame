using SqlSugar;
using WeDonekRpc.Model;

namespace RpcCentral.Model.DB
{
    [SugarTable("ServerTransmitScheme")]
    public class ServerTransmitSchemeModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        public long SystemTypeId
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
        /// 方案名称
        /// </summary>
        public string Scheme
        {
            get;
            set;
        }

        /// <summary>
        /// 负载均衡类型
        /// </summary>
        public TransmitType TransmitType
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public int VerNum
        {
            get;
            set;
        }
        /// <summary>
        /// 备注说明
        /// </summary>
        public string Show
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        }
        public DateTime AddTime { get; set; }
    }
}
