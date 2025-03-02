using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Model.Server;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 服务根节点
    /// </summary>
    internal interface IRemoteRootNode : IRemote, IDataSyncClass
    {
        /// <summary>
        /// 刷新状态
        /// </summary>
        /// <param name="now"></param>
        void RefreshState (int now);
        /// <summary>
        /// 检查远程服务器是否可用
        /// </summary>
        /// <returns></returns>
        bool CheckIsUsable (int overTime);


        /// <summary>
        /// 获取平均响应时间
        /// </summary>
        /// <returns></returns>
        int GetAvgTime ();

        /// <summary>
        /// 获取远程服务器状态
        /// </summary>
        /// <returns></returns>
        RemoteState GetRemoteState ();

        /// <summary>
        /// 刷新限流配置
        /// </summary>
        void RefreshLimit ();

        /// <summary>
        /// 刷新降级配置
        /// </summary>
        void RefreshReduce ();
    }
}
