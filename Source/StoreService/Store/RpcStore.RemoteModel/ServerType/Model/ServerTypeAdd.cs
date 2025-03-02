using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ServerType.Model
{
    /// <summary>
    /// 系统类别添加
    /// </summary>
    public class ServerTypeAdd : ServerTypeSet
    {
        /// <summary>
        /// 服务类别组ID
        /// </summary>
        [NumValidate("rpc.store.server.group.id.error", 1)]
        public long GroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 类型值
        /// </summary>
        [NullValidate("rpc.store.server.type.val.null")]
        [LenValidate("rpc.store.server.type.val.len", 2, 50)]
        [FormatValidate("rpc.store.server.type.val.error", ValidateFormat.字母点)]
        public string TypeVal
        {
            get;
            set;
        }

    }
}
