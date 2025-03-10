﻿using System.Collections.Concurrent;
using System.Threading;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Modular.Config;
using WeDonekRpc.Modular.Model;
using WeDonekRpc.ModularModel.Identity.Msg;

namespace WeDonekRpc.Modular.Service
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class IdentityService : IIdentityService
    {
        private static readonly ConcurrentDictionary<string, UserIdentity> _Identity = new ConcurrentDictionary<string, UserIdentity>();
        private static readonly AsyncLocal<IdentityIdLocal> _IdentityId = new AsyncLocal<IdentityIdLocal>(_IdentityChange);

        private static void _IdentityChange ( AsyncLocalValueChangedArgs<IdentityIdLocal> e )
        {
            if ( e.CurrentValue != null && e.CurrentValue.isEnd )
            {
                _IdentityId.Value = null;
            }
        }

        private readonly IdentityConfig _Config = null;

        public string IdentityId
        {
            get => _IdentityId.Value != null ? _IdentityId.Value.value : this._Config.DefAppId;
            set
            {
                IdentityIdLocal local = _IdentityId.Value;
                if ( local != null && local.value != value )
                {
                    local.isEnd = true;
                }
                _IdentityId.Value = new IdentityIdLocal { value = value };
            }
        }
        public bool IsEnableIdentity
        {
            get;
            private set;
        }
        static IdentityService ()
        {
            RpcClient.Route.AddRoute<UpdateIdentity>(_UpdateIdentity);
            AutoClearDic.AutoClear<string, UserIdentity>(_Identity);
        }
        private static void _UpdateIdentity ( UpdateIdentity arg )
        {
            if ( _Identity.TryGetValue(arg.AppId, out UserIdentity identity) )
            {
                identity.ResetLock();
            }
        }
        public IdentityService ( IRpcService service )
        {
            this._Config = ModularConfig.GetIdentityConfig();
            service.SendIng += this.RpcEvent_SendEvent;
            service.ReceiveEnd += this.Service_ReceiveEnd;
        }

        private void Service_ReceiveEnd ( IMsg msg, TcpRemoteReply reply )
        {
            _IdentityId.Value = null;
        }

        private void RpcEvent_SendEvent ( ref SendBody send, int sendNum )
        {
            if ( sendNum == 0 && _IdentityId.Value != null )
            {
                send.config.SetAppIdentity(send.model, _IdentityId.Value.value);
            }
        }

        private static bool _GetIdentity ( string id, out UserIdentity identity )
        {
            if ( !_Identity.TryGetValue(id, out identity) )
            {
                identity = _Identity.GetOrAdd(id, new UserIdentity(id));
            }
            if ( !identity.Init() )
            {
                _ = _Identity.TryRemove(id, out identity);
                identity.Dispose();
                return false;
            }
            return identity.IsInit;
        }

        public UserIdentity GetIdentity ()
        {
            if ( !_GetIdentity(this.IdentityId, out UserIdentity identity) )
            {
                throw new ErrorException(identity.Error);
            }
            else if ( !identity.IsValid )
            {
                throw new ErrorException("rpc.identity.not.find");
            }
            else
            {
                return identity;
            }
        }

        public void CheckState ()
        {
            if ( !_GetIdentity(this.IdentityId, out UserIdentity identity) )
            {
                throw new ErrorException(identity.Error);
            }
            else if ( !identity.IsValid )
            {
                throw new ErrorException("rpc.identity.not.find");
            }
        }

        public void SetIdentityId ( string identityId )
        {
            if ( !_GetIdentity(identityId, out UserIdentity identity) )
            {
                throw new ErrorException(identity.Error);
            }
            else if ( !identity.IsValid )
            {
                throw new ErrorException("rpc.identity.not.find");
            }
            this.IdentityId = identityId;
        }
        public void Clear ()
        {
            IdentityIdLocal local = IdentityService._IdentityId.Value;
            if ( local != null )
            {
                local.isEnd = true;
                IdentityService._IdentityId.Value = null;
            }
        }
    }
}
