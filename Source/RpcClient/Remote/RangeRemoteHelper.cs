using System.Collections.Generic;
using System.Linq;

using RpcClient.Collect;
using RpcClient.Interface;
using RpcClient.Model;
using RpcClient.Remote.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.Remote
{
        /// <summary>
        /// 负载均衡器
        /// </summary>
        internal class RangeRemoteHelper : IRemoteServer
        {
                /// <summary>
                /// 节点类型负载器
                /// </summary>
                private readonly IBalanced _Def = null;
                /// <summary>
                /// 备份的负载器
                /// </summary>
                private readonly IBalanced _Back = null;
                /// <summary>
                /// 节点类型负载配置
                /// </summary>
                private readonly BalancedConfig _Config = null;
                /// <summary>
                /// 负载均衡分组
                /// </summary>
                private readonly Dictionary<TransmitType, RemoteGroup> _Group = null;

                /// <summary>
                /// 负载均衡组
                /// </summary>
                private readonly Dictionary<int, RemoteGroup> _TransmitList = null;
                /// <summary>
                /// 负载均衡器
                /// </summary>
                /// <param name="type">负载方式</param>
                /// <param name="servers"></param>
                public RangeRemoteHelper(BalancedType type, ServerConfig[] servers)
                {
                        this._Config = new BalancedConfig(type, servers);
                        this._Def = BalancedCollect.GetBalanced(type, this._Config.ToConfig());
                        RangeServer[] server = _GetRangeServer(servers);
                        //全局节点负载配置
                        RangeServer[] configs = server.FindAll(a => a.TransmitType != TransmitType.close && a.Id == 0);
                        this._Group = configs.GroupBy(a => a.TransmitType).ToDictionary(a => a.Key, a => new RemoteGroup(a.Key, a.ToArray(), this._Config));

                        //独立节点负载配置项
                        configs = server.FindAll(a => a.TransmitType != TransmitType.close && a.Id != 0);
                        this._TransmitList = configs.GroupBy(a => a.Id).ToDictionary(a => a.Key, a => new RemoteGroup(a.ToArray(), this._Config));

                        //备用节点
                        RangeServer[] list = server.FindAll(a => a.TransmitType == TransmitType.close && a.Id == 0);
                        if (list.Length != 0)
                        {
                                this._Back = BalancedCollect.GetBalanced(type, this._Config.ToConfig(list));
                        }
                }
                /// <summary>
                /// 将节点配置转换为转发配置列表
                /// </summary>
                /// <param name="servers">服务节点配置</param>
                /// <returns>转发配置</returns>
                private static RangeServer[] _GetRangeServer(ServerConfig[] servers)
                {
                        List<RangeServer> list = new List<RangeServer>();
                        servers.ForEach(a =>
                         {
                                 if (a.IsTransmit)
                                 {
                                         a.Transmit.ForEach(b =>
                                         {
                                                 list.Add(new RangeServer
                                                 {
                                                         Id = b.TransmitId,
                                                         ServerId = a.ServerId,
                                                         TransmitType = b.TransmitType,
                                                         Range = b.Range,
                                                         Value = b.Value
                                                 });
                                         });
                                 }
                                 else
                                 {
                                         list.Add(new RangeServer
                                         {
                                                 ServerId = a.ServerId,
                                                 TransmitType = TransmitType.close
                                         });
                                 }
                         });
                        return list.ToArray();
                }
                /// <summary>
                /// 获取转发的依据
                /// </summary>
                /// <param name="config">配置</param>
                /// <param name="data">数据</param>
                /// <returns>转发的依据值</returns>
                private static long _GetValue(IRemoteConfig config, object data)
                {
                        if (config.TransmitType == TransmitType.HashCode)
                        {
                                return data.GetHashCode();
                        }
                        else if (config.TransmitType == TransmitType.Number)
                        {
                                return (long)data;
                        }
                        else
                        {
                                char[] chars = ((string)data).ToCharArray();
                                return config.ZIndexBit.IsNull()
                                         ? Tools.GetZoneIndex(chars, 0, chars.Length - 1)
                                         : Tools.GetZoneIndex(chars, config.ZIndexBit[0], config.ZIndexBit[1]);
                        }
                }
                /// <summary>
                /// 获取参与转发的负载器
                /// </summary>
                /// <param name="config">配置</param>
                /// <param name="group">负载组</param>
                /// <returns></returns>
                private bool _GetRemoteGroup(IRemoteConfig config, out RemoteGroup group)
                {
                        if (config.TransmitId == 0)
                        {
                                return this._Group.TryGetValue(config.TransmitType, out group);
                        }
                        return this._TransmitList.TryGetValue(config.TransmitId, out group);
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="config"></param>
                /// <param name="data"></param>
                /// <param name="server"></param>
                /// <returns></returns>
                private bool _RangeFind(IRemoteConfig config, object data, out IRemote server)
                {
                        if (!this._GetRemoteGroup(config, out RemoteGroup group))
                        {
                                server = null;
                                return false;
                        }
                        else if (config.TransmitType == TransmitType.FixedType)
                        {
                                return group.RangeFind(config, data.ToString(), out server);
                        }
                        else
                        {
                                return group.RangeFind(config, _GetValue(config, data), out server);
                        }
                }
                /// <summary>
                /// 获取可用服务节点
                /// </summary>
                /// <typeparam name="T">消息体</typeparam>
                /// <param name="config">配置</param>
                /// <param name="model">消息数据</param>
                /// <param name="server">服务节点</param>
                /// <returns>是否已获取节点</returns>
                public bool DistributeNode<T>(IRemoteConfig config, T model, out IRemote server)
                {
                        if (config.TransmitType != TransmitType.close)
                        {
                                object res = config.GetIdentityVal<T>(model);
                                if (res == null)
                                {
                                        server = null;
                                        return false;
                                }
                                else if (this._RangeFind(config, res, out server))
                                {
                                        return true;
                                }
                                else
                                {
                                        return this._DistributeNodeByBack(config, out server);
                                }
                        }
                        else
                        {
                                return this._Def.DistributeNode(config, out server);
                        }
                }
                /// <summary>
                /// 重备用负载器获取节点
                /// </summary>
                /// <param name="config">配置</param>
                /// <param name="server">服务节点</param>
                /// <returns>是否已获取节点</returns>
                private bool _DistributeNodeByBack(IRemoteConfig config, out IRemote server)
                {
                        if (this._Back == null)
                        {
                                server = null;
                                return false;
                        }
                        return this._Back.DistributeNode(config, out server);
                }
                /// <summary>
                /// 分配节点
                /// </summary>
                /// <param name="config">配置</param>
                /// <param name="server">服务节点</param>
                /// <returns>是否已获取节点</returns>
                public bool DistributeNode(IRemoteConfig config, out IRemote server)
                {
                        return this._Def.DistributeNode(config, out server);
                }
        }
}
