using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
namespace RpcStore.RemoteModel.ContainerGroup.Model
{
    public class ContainerGroupAdd : ContainerGroupSet
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        [NumValidate("rpc.store.container.region.Id.error", 1)]
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 容器编号
        /// </summary>
        [NullValidate("rpc.store.container.host.mac.null")]
        [FormatValidate("rpc.store.container.host.mac.error", ValidateFormat.MAC)]
        public string HostMac
        {
            get;
            set;
        }
        /// <summary>
        /// 容器类型
        /// </summary>
        [EnumValidate("rpc.store.container.type.error", typeof(ContainerType))]
        public ContainerType ContainerType
        {
            get;
            set;
        }
    }
}
