namespace WeDonekRpc.Client.Interface
{
    public interface IRemoteCursor
    {
        /// <summary>
        /// 服务节点数量
        /// </summary>
        int Num { get; }
        /// <summary>
        /// 当前索引
        /// </summary>
        int Current { get; }
        /// <summary>
        /// 获取一个节点
        /// </summary>
        /// <param name="remote">节点信息</param>
        /// <returns>是否成功</returns>
        bool ReadNode (bool isCloseTrace, out IRemote remote);
    }
}