
using System;
using System.IO;
using System.Security.Principal;

using ApiGateway.Interface;

using RpcModular;
using RpcModular.Domain;

using RpcHelper;

namespace ApiGateway.Collect
{
        internal class UserIdentityCollect : IUserIdentityCollect
        {
                private static readonly IIdentityService _Identity = RpcClient.RpcClient.Unity.Resolve<IIdentityService>();
                [ThreadStatic]
                public static IdentityDomain Identity;

                public bool IsEnableIdentity
                {
                        get;
                } = _Identity.IsEnableIdentity;

                public void CheckIdentity(string identityId)
                {
                        if (!identityId.IsNull())
                        {
                                _Identity.IdentityId = identityId;
                        }
                        _Identity.CheckState();
                }

                public void InitIdentity(string identity, string path)
                {
                        if (!identity.IsNull())
                        {
                                _Identity.IdentityId = identity;
                        }
                        if (_Identity.Check())
                        {
                                IdentityDomain domain = _Identity.GetIdentity();
                                domain.CheckGateway(path);
                                Identity = domain;
                        }
                }

        }
}
