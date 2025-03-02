namespace RpcSync.Service.Node
{
    /// <summary>
    /// 根节点
    /// </summary>
    public class RootNode : IEquatable<RootNode>
    {
        /// <summary>
        /// 节点Id
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 指令级
        /// </summary>
        public string Dictate
        {
            get;
            set;
        }
        public override bool Equals(object obj)
        {
            if (obj is RootNode i)
            {
                return i.Id == Id;
            }
            return false;
        }

        public bool Equals(RootNode other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
