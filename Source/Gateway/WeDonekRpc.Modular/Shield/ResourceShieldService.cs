using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using WeDonekRpc.Modular.Domain;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Shield.Msg;

namespace WeDonekRpc.Modular.Shield
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class ResourceShieldService : IResourceShieldService
    {
        private static readonly ConcurrentDictionary<string, ResourceShield> _ShieldCache = new ConcurrentDictionary<string, ResourceShield>();
        private Timer _CheckTime;

        private static void _Refresh (RefreshApiShieId obj)
        {
            if (_ShieldCache.TryGetValue(obj.Path, out ResourceShield shield))
            {
                shield.ResetLock();
            }
        }

        private bool _Get (string path, out ResourceShield shield)
        {
            if (!_ShieldCache.TryGetValue(path, out shield))
            {
                shield = _ShieldCache.GetOrAdd(path, new ResourceShield(path, ShieldType.接口));
            }
            if (!shield.Init())
            {
                _ = _ShieldCache.TryRemove(path, out shield);
                shield.Dispose();
                return false;
            }
            return shield.IsInit;
        }
        public bool CheckIsShieId (string path)
        {
            if (!this._Get(path, out ResourceShield shield))
            {
                throw new ErrorException(shield.Error);
            }
            return shield.IsShieId;
        }
        public void Dispose ()
        {
            this._CheckTime?.Dispose();
            this._CheckTime = null;
            RpcClient.Route.RemoveRoute("RefreshApiShieId");
            _ShieldCache.Clear();
        }

        public void Init ()
        {
            if (this._CheckTime == null)
            {
                this._CheckTime = new Timer(_Check, null, 1000, 1000);
            }
            if (!RpcClient.Route.CheckIsExists("RefreshApiShieId"))
            {
                RpcClient.Route.AddRoute<RefreshApiShieId>(_Refresh);
            }
        }

        private static void _Check (object state)
        {
            if (_ShieldCache.IsEmpty) { return; }
            string[] keys = _ShieldCache.Keys.ToArray();
            long now = DateTime.Now.ToLong();
            keys.ForEach(a =>
            {
                _ShieldCache[a].CheckIsOverTime(now);
            });
        }
    }
}
