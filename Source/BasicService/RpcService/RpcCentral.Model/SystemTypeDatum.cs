using WeDonekRpc.Model;

namespace RpcCentral.Model
{
    public class SystemTypeDatum
    {
        /// <summary>
        /// 系统类别Id
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        public string SystemName
        {
            get;
            set;
        }
        /// <summary>
        /// 所属组别
        /// </summary>
        public long GroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 默认端口号
        /// </summary>
        public int DefPort { get; set; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public RpcServerType ServiceType
        {
            get;
            set;
        }
    }
}
