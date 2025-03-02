using WeDonekRpc.ExtendModel;
using WeDonekRpc.Helper.Validate;
namespace RpcStore.RemoteModel.RetryTask.Model
{
    public class RetryTaskQuery
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        [NumValidate("rpc.mer.id.error", 1)]
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string QueryKey
        {
            get;
            set;
        }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 所在机房
        /// </summary>
        public int? RegionId { get; set; }
        /// <summary>
        /// 系统类别
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public AutoRetryTaskStatus[] Status { get; set; }

        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime? Begin { get; set; }
        /// <summary>
        /// 结束执行时间
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// 已重试数量
        /// </summary>
        public int? RetryNum { get; set; }

    }
}
