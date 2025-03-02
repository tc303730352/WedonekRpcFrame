using WeDonekRpc.Model;
namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 服务节点组
    /// </summary>
    internal interface IRemoteGroup
    {
        /// <summary>
        /// 查找匹配的服务节点
        /// </summary>
        /// <param name="config"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        bool FindServer (IRemoteConfig config, out IRemote server);
        /// <summary>
        /// 查找匹配的服务节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sysType"></param>
        /// <param name="config"></param>
        /// <param name="model"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        bool FindServer<T> (string sysType, IRemoteConfig config, T model, out IRemote server);
        /// <summary>
        /// 获取所有满足条件的发送节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="systemType"></param>
        /// <param name="config"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        IRemoteCursor FindAllServer<T> (string systemType, IRemoteConfig config, T model);
    }
}