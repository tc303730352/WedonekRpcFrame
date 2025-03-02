namespace RpcStore.RemoteModel.DictateNode.Model
{
    public class DictateNodeData
    {
        public long Id { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 指令集
        /// </summary>
        public string Dictate { get; set; }
        /// <summary>
        /// 指令名
        /// </summary>
        public string DictateName { get; set; }
        /// <summary>
        /// 是否终节点
        /// </summary>
        public bool IsEndpoint { get; set; }
    }
}
