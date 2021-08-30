using System;
using System.Collections.Concurrent;

using RpcClient.Attr;
using RpcClient.Interface;

using RpcHelper;

using RpcModular.Domain;
using RpcModular.Model;

namespace RpcModular.Service
{

        [ClassLifetimeAttr(ClassLifetimeType.单例)]
        internal class IdentityService : IIdentityService
        {
                private static readonly ConcurrentDictionary<string, UserIdentity> _Identity = new ConcurrentDictionary<string, UserIdentity>();

                [ThreadStatic]
                private static string _IdentityId = null;
                private IdentityConfig _Config = null;

                public string IdentityId
                {
                        get => _IdentityId ?? this._Config.DefAppId;
                        set => _IdentityId = value;
                }
                public bool IsEnableIdentity
                {
                        get;
                        private set;
                }
                public IdentityService()
                {
                        this._Config = RpcClient.RpcClient.Config.GetConfigVal("identity", new IdentityConfig());
                        RpcClient.RpcClient.Config.AddRefreshEvent(this._Refresh);
                }
                private void _Refresh(IConfigServer config, string name)
                {
                        if (name.StartsWith("identity") || name == string.Empty)
                        {
                                this._Config = config.GetConfigVal("identity", new IdentityConfig());
                                this.IsEnableIdentity = this._Config.IsEnable;
                        }
                }

                private static bool _GetIdentity(string id, out UserIdentity identity)
                {
                        if (!_Identity.TryGetValue(id, out identity))
                        {
                                identity = _Identity.GetOrAdd(id, new UserIdentity(id));
                        }
                        if (!identity.Init())
                        {
                                _Identity.TryRemove(id, out identity);
                                identity.Dispose();
                                return false;
                        }
                        return identity.IsInit;
                }

                public IdentityDomain GetIdentity()
                {
                        if (!_GetIdentity(this.IdentityId, out UserIdentity identity))
                        {
                                throw new ErrorException(identity.Error);
                        }
                        else
                        {
                                return new IdentityDomain(identity);
                        }
                }

                public bool Check()
                {
                        if (this.IdentityId != null)
                        {
                                return true;
                        }
                        else if (this._Config.IsEnable)
                        {
                                throw new ErrorException("identity.null");
                        }
                        _IdentityId = null;
                        return false;
                }

                public void CheckState()
                {
                        if (!_GetIdentity(this.IdentityId, out UserIdentity identity))
                        {
                                throw new ErrorException(identity.Error);
                        }
                }
        }
}
