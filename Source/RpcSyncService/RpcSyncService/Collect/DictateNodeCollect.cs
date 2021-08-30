using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using RpcSyncService.Model;
using RpcSyncService.Node;

using RpcHelper;
using RpcHelper.TaskTools;

namespace RpcSyncService.Collect
{
        internal class DictateNodeCollect
        {
                private static readonly ConcurrentDictionary<string, INodeServer> _NodeServer = new ConcurrentDictionary<string, INodeServer>();

                private static RootNode[] _EndNode = null;
                static DictateNodeCollect()
                {
                        TaskManage.AddTask(new TaskHelper("刷新服务器节点!", new TimeSpan(0, 0, Tools.GetRandom(60, 120)), new Action(_RefreshServerNode)));
                        RpcClient.RpcClient.Route.AddRoute("RefreshDictateNode", () =>
                        {
                                new AutoRetryEntrust(_LoadDictateNode).Execute();
                        });
                        RpcClient.RpcClient.Route.AddRoute("RefreshServerNode", () =>
                        {
                                new AutoRetryEntrust(_LoadServerNode).Execute();
                        });
                }
                private static int _ServerVerNum = 0;
                private static int _DictateVerNum = 0;
                private static void _RefreshServerNode()
                {
                        int verNum = RpcClient.RpcClient.Config.GetConfigVal("ServerNodeVer", 0);
                        if (verNum == _ServerVerNum)
                        {
                                _ServerVerNum = verNum;
                                new AutoRetryEntrust(_LoadServerNode).SyncExecute();
                        }
                        verNum = RpcClient.RpcClient.Config.GetConfigVal("DictateNodeVer", 0);
                        if (verNum == _DictateVerNum)
                        {
                                _DictateVerNum = verNum;
                                new AutoRetryEntrust(_LoadDictateNode).SyncExecute();
                        }
                }

                public static void Init()
                {
                        new AutoRetryEntrust(_LoadServerNode).SyncExecute();
                        new AutoRetryEntrust(_LoadDictateNode).SyncExecute();
                }

                private static void _LoadServerNode()
                {
                        if (!new DAL.ServerGroupDAL().GetServerGroup(out ServerGroup[] groups))
                        {
                                throw new ErrorException("rpc.server.group.get.error");
                        }
                        else if (!new DAL.RemoteServerTypeDAL().GetSystemType(out SystemType[] sysType))
                        {
                                throw new ErrorException("rpc.server.type.get.error");
                        }
                        else
                        {
                                RootNode[] roots = sysType.ConvertAll(a => new RootNode
                                {
                                        Dictate = a.TypeVal,
                                        Id = a.Id
                                });
                                roots.ForEach(a =>
                                {
                                        if (!_NodeServer.ContainsKey(a.Dictate))
                                        {
                                                _NodeServer.TryAdd(a.Dictate, new EndNodeServer(a));
                                        }
                                });
                                _EndNode = roots;
                                groups.ForEach(a =>
                                {
                                        INodeServer node = new NodeServer(sysType.FindAll(b => b.GroupId == a.Id));
                                        _NodeServer.AddOrUpdate(a.TypeVal, node, (c, b) => node);
                                });
                        }
                }
                private static void _AddDictateNode(DictateNode node, DictateNode[] nodes)
                {
                        if (node.IsEndpoint)
                        {
                                return;
                        }
                        DictateNode[] lower = nodes.FindAll(a => a.ParentId == node.Id);
                        if (lower.Length == 0)
                        {
                                return;
                        }
                        INodeServer t = new DictateNodeServer(lower.ConvertAll(b => b.Dictate));
                        _NodeServer.AddOrUpdate(node.Dictate, t, (c, b) => t);
                }
                private static void _LoadDictateNode()
                {
                        if (!new DAL.DictateNodeDAL().GetDictateNode(out DictateNode[] nodes))
                        {
                                throw new ErrorException("rpc.dictate.node.get.error");
                        }
                        DictateNode[] root = nodes.FindAll(a => a.ParentId == Guid.Empty);
                        root.ForEach(a =>
                        {
                                _AddDictateNode(a, nodes);
                        });
                }
                public static RootNode[] GetRootNode()
                {
                        return _EndNode;
                }
                public static void Load(string key, List<RootNode> dictates)
                {
                        if (_NodeServer.TryGetValue(key, out INodeServer node))
                        {
                                node.Load(dictates);
                        }
                }
        }
}
