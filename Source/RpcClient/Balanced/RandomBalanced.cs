using RpcClient.Collect;
using RpcClient.Interface;

using RpcModel;

using RpcHelper;

namespace RpcClient.Balanced
{
        /// <summary>
        /// 随机
        /// </summary>
        internal class RandomBalanced : IBalanced
        {
                public BalancedType BalancedType
                {
                        get;
                } = BalancedType.random;
                public RandomBalanced(long[] server)
                {
                        this._ServerList = server;
                        this._ServerNum = server.Length;
                }
                /// <summary>
                /// 服务器列表
                /// </summary>
                private readonly long[] _ServerList = new long[0];

                private readonly int _ServerNum = 0;


                private bool _CheckIsUsable(long[] servers, int num, out IRemote server)
                {
                        long serverId = num == 1 ? servers[0] : servers[Tools.GetRandom() % num];
                        if (serverId == 0)
                        {
                                server = null;
                                return false;
                        }
                        else if (Collect.RemoteServerCollect.GetRemoteServer(serverId, out server) && server.IsUsable)
                        {
                                return true;
                        }
                        else if (servers.Length == 1)
                        {
                                return false;
                        }
                        else
                        {
                                servers = System.Array.FindAll(servers, a => a != serverId);
                                return this._CheckIsUsable(servers, servers.Length, out server);
                        }
                }

                public bool DistributeNode(IRemoteConfig config, out IRemote server)
                {
                        long serverId = this._ServerList[Tools.GetRandom() % this._ServerNum];
                        if (serverId == 0)
                        {
                                server = null;
                                return false;
                        }
                        else if (serverId != config.FilterServerId && RemoteServerCollect.GetRemoteServer(serverId, out server) && server.IsUsable)
                        {
                                return true;
                        }
                        else
                        {
                                long[] servers = System.Array.FindAll(this._ServerList, a => a != serverId && a != config.FilterServerId);
                                return this._CheckIsUsable(servers, servers.Length, out server);
                        }
                }
        }
}