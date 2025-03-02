using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ContainerGroup.Model
{
    public class ContainerGroupSet
    {
        /// <summary>
        /// 节点组名
        /// </summary>
        [NullValidate("rpc.store.container.group.name.null")]
        [LenValidate("rpc.store.container.group.name.len", 1, 50)]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        [LenValidate("rpc.store.container.group.name.len", 0, 150)]
        public string Remark
        {
            get;
            set;
        }
    }
}
