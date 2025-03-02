using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.DictateNode.Model
{
    /// <summary>
    /// 广播指令节点信息
    /// </summary>
    public class DictateNodeAdd
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        [NumValidate("rpc.store.dictate.parent.id.error", 0)]
        public long ParentId { get; set; }
        /// <summary>
        /// 指令集
        /// </summary>
        [NullValidate("rpc.store.dictate.null")]
        [LenValidate("rpc.store.dictate.len", 1, 50)]
        [FormatValidate("rpc.store.dictate.error", ValidateFormat.字母点)]
        public string Dictate { get; set; }
        /// <summary>
        /// 指令名
        /// </summary>
        [NullValidate("rpc.store.dictate.name.null")]
        [LenValidate("rpc.store.dictate.name.len", 1, 50)]
        public string DictateName { get; set; }
        /// <summary>
        /// 是否终节点
        /// </summary>
        public bool IsEndpoint { get; set; }
    }
}
