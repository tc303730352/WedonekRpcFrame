using WeDonekRpc.Helper;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Node
{
    internal class DictateNodeServer : INodeServer
    {
        private readonly INodeService _NodeService;
        private readonly string[] _LowerDictate;
        public DictateNodeServer (string[] lower, INodeService nodeService)
        {
            this._NodeService = nodeService;
            this._LowerDictate = lower;
        }
        public void Load (List<RootNode> dictates)
        {
            this._LowerDictate.ForEach(a => this._NodeService.Load(a, dictates));
        }

        public void Load (List<long> sysTypeId)
        {
            this._LowerDictate.ForEach(a => this._NodeService.Load(a, sysTypeId));
        }
    }
}
