using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
namespace RpcStore.RemoteModel.ContainerGroup.Model
{
    public class ContainerGroupQuery
    {
        /// <summary>
        /// 查询Key
        /// </summary>
        [LenValidate("rpc.container.type.name.len", 0, 50)]
        public string QueryKey { get; set; }

        public int? RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 容器类型
        /// </summary>
        [EnumValidate("rpc.container.type.error", typeof(ContainerType))]
        public ContainerType? ContainerType
        {
            get;
            set;
        }
    }
}
