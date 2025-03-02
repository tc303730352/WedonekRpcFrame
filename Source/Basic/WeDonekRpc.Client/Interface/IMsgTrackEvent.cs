using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
        /// <summary>
        /// 链路追踪
        /// </summary>
        public interface IMsgTrackEvent
        {
                /// <summary>
                /// 开始发送
                /// </summary>
                /// <param name="config">发送配置</param>
                /// <param name="msg">消息</param>
                /// <param name="remote">远程信息</param>
                /// <returns>追踪的ID</returns>
                string BeginTrack(IRemoteConfig config, TcpRemoteMsg msg, IRemote remote);

                /// <summary>
                /// 追踪结束
                /// </summary>
                /// <param name="trackId"></param>
                /// <param name="reply"></param>
                void EndTrack(string trackId, TcpRemoteReply reply);

                /// <summary>
                /// 追踪结束
                /// </summary>
                /// <param name="trackId"></param>
                /// <param name="error"></param>
                void EndTrack(string trackId, string error);
        }
}
