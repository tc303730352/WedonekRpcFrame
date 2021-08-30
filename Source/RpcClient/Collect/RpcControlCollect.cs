using RpcClient.Interface;
using RpcClient.RpcApi;

using RpcModel.Model;

using RpcHelper;
namespace RpcClient.Collect
{
        [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.单例)]
        internal class RpcControlCollect : IRpcControlCollect
        {
                private RpcControlServer[] _ServerList = null;


                public bool GetServer(int id, out RpcControlServer server, out string error)
                {
                        if (!this.GetServers(out RpcControlServer[] servers, out error))
                        {
                                server = null;
                                return false;
                        }
                        server = servers.Find(a => a.Id == id);
                        if (server == null)
                        {
                                error = "rpc.control.not.find";
                                server = null;
                                return false;
                        }
                        return true;
                }

                public bool GetServers(out RpcControlServer[] servers, out string error)
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
