using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.RpcApi;
using WeDonekRpc.Helper;
using WeDonekRpc.Model.Model;
namespace WeDonekRpc.Client.Collect
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class RpcControlCollect : IRpcControlCollect
    {
        private RpcControlServer[] _ServerList = null;


        public bool GetServer (int id, out RpcControlServer server, out string error)
        {
            if (!this.GetServers(out RpcControlServer[] servers, out error))
            {
                server = null;
                return false;
            }
            server = servers.Find(a => a.Id == id);
            if (server == null)
            {
                error = "rpc.store.control.not.find";
                server = null;
                return false;
            }
            return true;
        }

        public bool GetServers (out RpcControlServer[] servers, out string error)
        {
            if (this._ServerList != null)
            {
                error = null;
                servers = this._ServerList;
                return true;
            }
            else if (RpcServiceApi.GetControlServer(out servers, out error))
            {
                this._ServerList = servers;
                return true;
            }
            return false;
        }
    }
}
