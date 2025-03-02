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
    internal class RpcDirectShieIdService : IRpcDirectShieIdService
    {
        private static readonly ConcurrentDictionary<string, ResourceShield> _ShieldCache = new ConcurrentDictionary<string, ResourceShield>();
        private Timer _CheckTime;

        private static void _Refresh (RefreshRpcShieId obj)
        {
            if (_ShieldCache.TryGetValue(obj.SysDictate, out ResourceShield shield))
            {
                shield.ResetLock();
            }
        }

        private bool _Get (string dictate, out ResourceShield shield)
        {
            if (!_ShieldCache.TryGetValue(dictate, out shield))
            {
                shield = _ShieldCache.GetOrAdd(dictate, new ResourceShield(dictate, ShieldType.指令));
            }
            if (!shield.Init())
            {
                _ = _ShieldCache.TryRemove(dictate, out shield);
                shield.Dispose();
                return false;
            }
            return shield.IsInit;
        }
        public bool CheckIsShieId (string dictate)
        {
            if (!this._Get(dictate, out ResourceShield shield))
            {
                throw new ErrorException(shield.Error);
            }
            return shield.IsShieId;
        }
        public void Close ()
        {
            if (this._CheckTime != null)
            {
                this._CheckTime.Dispose();
                this._CheckTime = null;
            }
            RpcClient.Route.RemoveRoute("RefreshRpcShieId");
            if (_ShieldCache.Count > 0)
            {
                _ShieldCache.Clear();
            }
        }

        public void Init ()
        {
            if (this._CheckTime == null)
            {
                this._CheckTime = new Timer(this._Check, null, 1000, 1000);
            }
            if (!RpcClient.Route.CheckIsExists("RefreshRpcShieId"))
            {
                RpcClient.Route.AddRoute<RefreshRpcShieId>(_Refresh);
            }
        }

        private void _Check (object state)
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
