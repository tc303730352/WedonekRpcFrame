using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ServerGroup.Model
{
    public class ServerGroupAdd
    {
        /// <summary>
        /// 组类别
        /// </summary>
        [NullValidate("rpc.store.server.group.type.val.null")]
        [LenValidate("rpc.store.server.group.type.val.len", 2, 50)]
        [FormatValidate("rpc.store.server.group.type.val.error", ValidateFormat.字母点)]
        public string TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 类别名
        /// </summary>
        [NullValidate("rpc.store.server.group.type.name.null")]
        [LenValidate("rpc.store.server.group.type.name.len", 2, 50)]
        public string GroupName
        {
            get;
            set;
        }
    }
}
