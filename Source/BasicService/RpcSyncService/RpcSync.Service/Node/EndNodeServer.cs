using RpcSync.Service.Interface;

namespace RpcSync.Service.Node
{
    internal class EndNodeServer : INodeServer
    {
        public EndNodeServer (RootNode node)
        {
            this._Dictate = node;
        }
        private readonly RootNode _Dictate;
        public void Load (List<RootNode> dictates)
        {
            dictates.Add(this._Dictate);
        }
        public void Load (List<long> sysTypeId)
        {
            sysTypeId.Add(this._Dictate.Id);
        }
    }
}
