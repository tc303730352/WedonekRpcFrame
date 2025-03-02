using RpcCentral.Common;
using RpcCentral.DAL;
using RpcCentral.Model;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Model.Model;

namespace RpcCentral.Collect.Collect
{
    internal class RpcControlCollect : IRpcControlCollect
    {
        private readonly IRpcControlListDAL _Controls;
        private readonly ICacheController _Cache;
        private static readonly string _CacheKey = "ControlServer";
        public RpcControlCollect (IRpcControlListDAL controls, ICacheController cache)
        {
            this._Controls = controls;
            this._Cache = cache;
        }

        public RpcControlServer[] GetControlServer ()
        {
            if (this._Cache.TryGet(_CacheKey, out RpcControlServer[] servers))
            {
                return servers;
            }
            servers = this._Controls.GetControlServer().ConvertMap<RpcControl, RpcControlServer>();
            _ = this._Cache.Set(_CacheKey, servers, new TimeSpan(0, 10, 0));
            return servers;
        }

        public void Refresh ()
        {
            _ = this._Cache.Remove("ControlServer");
        }
    }
}
