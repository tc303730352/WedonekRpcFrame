using System.IO;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    public interface IRemote
    {
        /// <summary>
        /// 可用状态
        /// </summary>
        bool IsUsable
        {
            get;
        }
        /// <summary>
        /// 离线时间
        /// </summary>
        int OfflineTime
        {
            get;
        }
        /// <summary>
        /// 降级持续时间
        /// </summary>
        int ReduceTime { get; }

        /// <summary>
        /// 区域Id
        /// </summary>
        int RegionId { get; }
        /// <summary>
        /// 远程地址
        /// </summary>
        string ServerIp { get; }
        /// <summary>
        /// 端口
        /// </summary>
        int Port { get; }
        /// <summary>
        /// 节点类型ID
        /// </summary>
        long SystemType { get; }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        long ServerId { get; }
        /// <summary>
        /// 服务名
        /// </summary>
        string ServerName { get; }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="config"></param>
        /// <param name="msg"></param>
        /// <param name="reply"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        bool SendData (IRemoteConfig config, TcpRemoteMsg msg, out TcpRemoteReply reply, out string error);
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="dicate"></param>
        /// <param name="config"></param>
        /// <param name="msg"></param>
        /// <param name="reply"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        bool SendData (string dicate, IRemoteConfig config, TcpRemoteMsg msg, out TcpRemoteReply reply, out string error);

        bool SendFile (IRemoteConfig config, TcpRemoteMsg msg, FileInfo file, UpFileAsync func, UpProgressAction progress, out IUpTask upTask, out string error);
    }
}
