using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.TransmitScheme.Model
{
    public class TransmitScheme
    {
        /// <summary>
        /// 方案ID
        /// </summary>
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
        /// <summary>
        /// 所属服务类别
        /// </summary>
        public string SystemType
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
        public string VerNum { get; set; }
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
