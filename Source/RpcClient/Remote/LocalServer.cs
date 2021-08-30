using System.Collections.Generic;
using System.IO;

using RpcClient.Collect;
using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;

using SocketTcpClient.UpFile;
namespace RpcClient.Remote
{
        /// <summary>
        /// 本地服务
        /// </summary>
        internal class LocalServer : IRemote
        {

                public bool IsUsable
                {
                        get;
                } = true;
                public long ServerId => RpcStateCollect.ServerId;

                public int OfflineTime => 0;


                public long SystemType => RpcStateCollect.ServerConfig.SystemType;

                public string ServerIp => RpcStateCollect.ServerConfig.ServerIp;

                public int Port => RpcStateCollect.ServerConfig.ServerPort;

                public int RegionId => RpcStateCollect.ServerConfig.RegionId;

                public string ServerName => RpcStateCollect.ServerConfig.Name;

                public bool SendData(IRemoteConfig config, TcpRemoteMsg msg, out TcpRemoteReply reply, out string error)
                {
                        reply = RpcMsgCollect.MsgEvent(new RemoteMsg(config.SysDictate, msg));
                        error = null;
                        return true;
                }

                public bool SendData(string dicate, IRemoteConfig config, TcpRemoteMsg msg, out TcpRemoteReply reply, out string error)
                {
                        reply = RpcMsgCollect.MsgEvent(new RemoteMsg(config.SysDictate, msg));
                        error = null;
                        return true;
                }

                public bool SendFile(IRemoteConfig config, TcpRemoteMsg msg, FileInfo file, UpFileAsync func, UpProgress progress, out UpTask upTask, out string error)
                {
                        error = "rpc.local.server.no.up.file";
                        upTask = null;
                        return false;
                }

                public Dictionary<string, dynamic> ToDictionary(int state)
                {
                        return new Dictionary<string, dynamic>();
                }

                public Dictionary<string, dynamic> ToDictionary()
                {
                        return new Dictionary<string, dynamic>();
                }


        }
}
