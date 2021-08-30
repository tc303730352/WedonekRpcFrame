using System;

using RpcModular.Model;

using RpcHelper;

namespace RpcModular.Domain
{
        public class IdentityDomain
        {
                private readonly UserIdentity _Identity = null;
                internal IdentityDomain(UserIdentity obj)
                {
                        this._Identity = obj;
                }
                public string IdentityId => this._Identity.IdentityId;
                public string AppName => this._Identity.AppName;

                public void CheckGateway(string path)
                {
                        if (!this._Identity.IsValid)
                        {
                                throw new ErrorException("Identity.not.find");
                        }
                        else if (!this._Identity.CheckGateway(path))
                        {
                                throw new ErrorException("Identity.no.prower");
                        }
                }
             

                internal void CheckRoute(string msgKey)
                {
                        if (!this._Identity.IsValid)
                        {
                                throw new ErrorException("Identity.not.find");
                        }
                        else if (!this._Identity.CheckRoute(msgKey))
                        {
                                throw new ErrorException("Identity.no.prower");
                        }
                }
        }
}
