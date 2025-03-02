using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;

namespace RpcSync.Service.Service
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class NodeRefreshService : INodeRefreshService
    {
        private readonly IIocService _Unity;
        private Timer _Refresh;
        public NodeRefreshService (IIocService unity)
        {
            this._Unity = unity;
        }
        private void _RefreshServerNode (object state)
        {
            using (IocScope scope = this._Unity.CreateScore())
            {
                INodeLoadService node = scope.Resolve<INodeLoadService>();
                node.LoadServerNode();
                node.LoadDictateNode();
            }
        }
        public void Init ()
        {
            if (this._Refresh == null)
            {
                this._Refresh = new Timer(new TimerCallback(this._RefreshServerNode), null, 200, Tools.GetRandom(60, 120) * 1000);
            }
        }
    }
}
