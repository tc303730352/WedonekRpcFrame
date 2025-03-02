namespace RpcStore.RemoteModel.DictateNode.Model
{
    /// <summary>
    /// 树节点
    /// </summary>
    public class TreeDictateNode
    {
        /// <summary>
        /// 节点Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 节点名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 下级节点
        /// </summary>
        public TreeDictateNode[] Children { get; set; }
    }
}
