using WeDonekRpc.Helper;
using RpcSync.Model;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Node
{
    internal class NodeServer : INodeServer
    {
        public NodeServer (SystemType[] types)
        {
            this._LowerNode = types.ConvertAll(a => new RootNode
            {
                Dictate = a.TypeVal,
                Id = a.Id
            }); ;
        }
        private readonly RootNode[] _LowerNode;

        public void Load (List<RootNode> dictates)
        {
            dictates.AddRange(this._LowerNode);
        }
        public void Load (List<long> sysTypeId)
        {
            this._LowerNode.ForEach(c =>
            {
                sysTypeId.Add(c.Id);
            });
        }
    }
}
