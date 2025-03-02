using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ResourceShield.Model
{
    public class ResourceShieldAdd
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        [NumValidate("rpc.store.resourceshield.resourceId.error", 1)]
        public long ResourceId { get; set; }


        /// <summary>
        /// 服务节点Id
        /// </summary> 
        public long[] ServerId
        {
            get;
            set;
        }

        /// <summary>
        /// 过期时间
        /// </summary>
        [TimeValidate("rpc.store.shieId.overTime.error", 0)]
        public DateTime? BeOverdueTime
        {
            get;
            set;
        }
        /// <summary>
        /// 屏蔽说明
        /// </summary>
        [LenValidate("rpc.store.shieId.show.len", 0, 100)]
        public string ShieIdShow { get; set; }
    }
}
