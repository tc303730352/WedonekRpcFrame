using RpcSync.Collect.Model;
using RpcSync.Service.Interface;
using RpcSync.Service.Node;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcSync.Service.Broadcast
{
    /// <summary>
    /// 加载所有根节点
    /// </summary>
    [IocName("ALLOnly")]
    internal class LoadALLOnlyNode : IInitBroadcast
    {
        private readonly INodeService _Service;
        public LoadALLOnlyNode (INodeService service)
        {
            this._Service = service;
        }
        public void InitBroadcastBody (BroadcastMsg msg, MsgSource source, ref BroadcastBody body)
        {
            RootNode[] nodes = this._Service.GetRootNode();
            body.Dictate = nodes.ConvertAll(a => a.Dictate);
        }
    }
}
