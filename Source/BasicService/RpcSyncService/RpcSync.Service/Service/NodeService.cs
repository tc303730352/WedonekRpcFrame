using System.Collections.Concurrent;
using RpcSync.Model;
using RpcSync.Service.Interface;
using RpcSync.Service.Node;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;

namespace RpcSync.Service.Service
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class NodeService : INodeService
    {
        private readonly ConcurrentDictionary<string, INodeServer> _NodeServer = new ConcurrentDictionary<string, INodeServer>();

        private RootNode[] _EndNode = null;

        private string _NodeMd5;
        private readonly string _DictateMd5;
        public void LoadServerNode (ServerGroup[] groups, SystemType[] sysTypes)
        {
            string md5 = string.Join(',', sysTypes.Select(a => a.Id).OrderBy(a => a)).GetMd5();
            if (md5 == this._NodeMd5)
            {
                return;
            }
            this._NodeMd5 = md5;
            RootNode[] roots = sysTypes.ConvertAll(a => new RootNode
            {
                Dictate = a.TypeVal,
                Id = a.Id
            });
            roots.ForEach(a =>
            {
                if (!this._NodeServer.ContainsKey(a.Dictate))
                {
                    _ = this._NodeServer.TryAdd(a.Dictate, new EndNodeServer(a));
                }
            });
            this._EndNode = roots;
            groups.ForEach(a =>
            {
                INodeServer node = new NodeServer(sysTypes.FindAll(b => b.GroupId == a.Id));
                if (this._NodeServer.TryGetValue(a.TypeVal, out INodeServer old))
                {
                    _ = this._NodeServer.TryUpdate(a.TypeVal, node, old);
                }
                else
                {
                    _ = this._NodeServer.TryAdd(a.TypeVal, node);
                }
            });
        }
        private void _AddDictateNode (DictateNode node, DictateNode[] nodes)
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
            INodeServer t = new DictateNodeServer(lower.ConvertAll(b => b.Dictate), this);
            if (this._NodeServer.TryGetValue(node.Dictate, out INodeServer old))
            {
                _ = this._NodeServer.TryUpdate(node.Dictate, t, old);
            }
            else
            {
                _ = this._NodeServer.TryAdd(node.Dictate, t);
            }
        }
        public void LoadDictateNode (DictateNode[] nodes)
        {
            string md5 = string.Join(',', nodes.Select(a => a.Id).OrderBy(a => a)).GetMd5();
            if (md5 == this._DictateMd5)
            {
                return;
            }
            this._NodeMd5 = md5;
            DictateNode[] root = nodes.FindAll(a => a.ParentId == 0);
            root.ForEach(a =>
            {
                this._AddDictateNode(a, nodes);
            });
        }
        public RootNode[] GetRootNode ()
        {
            return this._EndNode;
        }
        public void Load (string key, List<RootNode> dictates)
        {
            if (this._NodeServer.TryGetValue(key, out INodeServer node))
            {
                node.Load(dictates);
            }
        }
        public void Load (string key, List<long> sysTypeId)
        {
            if (this._NodeServer.TryGetValue(key, out INodeServer node))
            {
                node.Load(sysTypeId);
            }
        }
    }
}
